using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

// 定义委托 Connect成功连接  Error发生错误  DataIn消息到达  Disconnect断开连接
public delegate void DSCClientOnConnectedHandler(object sender, DSCClientConnectedEventArgs e);
public delegate void DSCClientOnErrorHandler(object sender, DSCClientErrorEventArgs e);
public delegate void DSCClientOnDataInHandler(object sender, DSCClientDataInEventArgs e);
public delegate void DSCClientOnDisconnectedHandler(object sender, DSCClientConnectedEventArgs e);

// Connected参数（Disconnected）
public class DSCClientConnectedEventArgs : EventArgs
{
    public Socket socket;

    public DSCClientConnectedEventArgs(Socket soc)
    {
        this.socket = soc;
    }
}
// Error参数
public class DSCClientErrorEventArgs : EventArgs
{
    public SocketException exception;

    public DSCClientErrorEventArgs(SocketException e)
    {
        this.exception = e;
    }
}
// DataIn参数
public class DSCClientDataInEventArgs : EventArgs
{
    public byte[] Data;
    public Socket socket;

    public DSCClientDataInEventArgs(Socket soc, byte[] datain)
    {
        this.socket = soc;
        this.Data = datain;  // 消息
    }
}

public class XTcpClient
{
    /*
         ManualResetEvent 允许线程通过发信号互相通信。（通常，此通信涉及一个线程在其他线程进行之前必须完成的任务）
         当一个（控制）线程开始一个活动（此活动必须完成后，其他线程才能开始）时，它调用 Reset 以将 ManualResetEvent 置于非终止状态。
         调用 WaitOne 的线程将阻塞，并等待信号。
         当 控制线程 完成活动时，它调用 Set 等待线程可以继续进行的信号，并释放所有等待线程。
         一旦它被终止，ManualResetEvent 将保持终止状态（即对 WaitOne 的调用的线程将立即返回，并不阻塞），直到它被手动重置。
    */
    private static ManualResetEvent connectDone = new ManualResetEvent(false);  // 目前暂未使用...

    // 定义三类事件
    public event DSCClientOnConnectedHandler OnConnected;
    public event DSCClientOnErrorHandler OnError;
    public event DSCClientOnDisconnectedHandler OnDisconnected;

    private Socket m_Socket;
    private IPEndPoint m_Remote;  // host
    private Thread m_SelectThread;
    private bool m_bStopRun;
    private ArrayList m_CheckRead, m_CheckSend, m_CheckError;
    private Queue<byte[]> m_SendBuff;  // 准备发送的数据 队列
    private Queue<MessageData> m_Datas;  // 已经接收的数据 队列
    private object _lock = new object();

    private void _Init()  // 初始化
    {
        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_SelectThread = new Thread(new ThreadStart(_LoopRun));  // 开启一个新的子线程，防止阻塞游戏主线程
        m_SelectThread.IsBackground = true;  // 设置
        m_bStopRun = false;
        m_CheckRead = new ArrayList();  // 检查
        m_CheckSend = new ArrayList();
        m_CheckError = new ArrayList();
        m_SendBuff = new Queue<byte[]>();  // 发送的消息队列
        m_Datas = new Queue<MessageData>();  // 接受到的消息队列
    }

    private void _BeginConnect()  // 开始异步连接
    {
        m_Socket.BeginConnect(m_Remote, new AsyncCallback(_EndConnect), m_Socket);

        // connectDone.WaitOne();  // 阻塞在这里
    }

    private void _EndConnect(IAsyncResult async)  // 成功连接自动回调
    {
        if (Connected)
        {
            m_SelectThread.Start();
            m_Socket.EndConnect(async);
        }

        if (OnConnected != null)
        {
            this.OnConnected(this, new DSCClientConnectedEventArgs(m_Socket));
        }

        // connectDone.Set();  // 与Reset()对应
    }

    private void _LoopRun()  // 开始线程
    {
        while (!m_bStopRun && Connected)  // 运行中 连接中
        {
            m_CheckRead.Clear();
            m_CheckSend.Clear();
            m_CheckError.Clear();
            m_CheckRead.Add(m_Socket);
            m_CheckSend.Add(m_Socket);
            m_CheckError.Add(m_Socket);

            /*
                使用socket 的 selct 用于检查多个套接字的状态（非阻塞 直接返回）
                在C#中，select方法内化为Socket类的静态方法之一，fd_set也用更为方便的ArrayList类来代替
                满足需要的套接字都留在了ArrayList类的对象中，用ArrayList对象的count属性判断个数。
                可以用数组下标形式逐个访问套接字以进行I/O操作。（这里明显只有一个套接字）
            */
            Socket.Select(m_CheckRead, null, null, 100);  // 关心接收
            if (m_CheckRead.Count > 0)
            {
                _OnRead();
            }

            Socket.Select(null, m_CheckSend, null, 100);  // 关心发送
            if (m_CheckSend.Count > 0)
            {
                _OnSend();
            }

            Socket.Select(null, null, m_CheckError, 100);  // 关心错误
            if (m_CheckError.Count > 0)
            {
                _OnError(null);
            }
        }
    }

    // ############# 当有数据需要读取时，这里 自动 被调用
    private void _OnRead()
    {
        /*
            socket.Available 是从已经从网络接收的、可供读取的数据的字节数。
            这个值是指 缓冲区 中已接收数据的字节数，不是实际的数据大小。
            而且如果网络有延迟，Send之后马上读取Available属性不一定能读到正确的值，
            所以不能利用socket.Available来判断总共要接受的字节数。
        */
        if (m_Socket.Available > 0)
        {
            try
            {
                byte[] buffer = new byte[13];
                m_Socket.Receive(buffer, 13, SocketFlags.Peek);  // 开始读取 Peek 快速查看传入消息
                Message_Head head = MessageParse.UnParseHead(buffer);  // 解析消息头
                if (head == null)
                {
                    _OnError(new SocketException((int)SocketError.ProtocolOption));
                    Close();
                }
                else {
                    // 13 = 5（协议头）+4（版本号）+4（消息体长度Length）
                    // 消息体长度 = command（int型4位）+msg（byte[]）
                    int iLength = head.Length + 13;  // 总长度
                    if (iLength <= m_Socket.Available)  // 消息全部到达
                    {
                        buffer = new byte[iLength];
                        m_Socket.Receive(buffer, 0, buffer.Length, SocketFlags.None);  // 读取消息到buffer中
                        MessageData data = MessageParse.UnParse(buffer);  // 解析成Data
                        if (data != null)
                        {
                            lock (_lock)
                            {  // 上锁，其他线程会被阻塞在外面直到锁被解开
                                m_Datas.Enqueue(data);  // 数据入队列
                            }
                        }
                        else {
                            _OnError(new SocketException((int)SocketError.ProtocolOption));  // 10042 Option unknown, or unsupported.
                            Close();
                        }
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                Close();
            }
            catch (SocketException ex)
            {
                _OnError(ex);
                Close();
            }
        }
    }

    // ############# 当有数据需要发送时，这里 自动 被调用
    private void _OnSend()
    {
        Monitor.Enter(m_SendBuff);
        while (m_SendBuff.Count > 0 && Connected)
        {
            byte[] buffer = m_SendBuff.Dequeue();
            m_Socket.Send(buffer);  // 开始发送
        }
        Monitor.Exit(m_SendBuff);
    }

    // ############# 当出现错误时 自动 被调用
    private void _OnError(SocketException ex)
    {
        if (OnError != null)
        {
            this.OnError(this, new DSCClientErrorEventArgs(ex));
        }
    }

    #region Interface 外部可以调用这里的方法

    public void Connect(string ip, int port)  // 发起连接
    {
        _Init();
        m_Remote = new IPEndPoint(IPAddress.Parse(ip), port);  // 获得服务器的host
        _BeginConnect();
    }

    public void Send(byte[] buffer)  // 发消息
    {
        if (buffer != null)
        {
            Monitor.Enter(m_SendBuff);  // 用法同下面的Loop，任意两个线程都不可以同时进入这里的Moniter.Enter()方法
            m_SendBuff.Enqueue(buffer);  // 消息入队
            Monitor.Exit(m_SendBuff);
        }
    }

    public MessageData Loop() // 取数据
    { 
        MessageData data = null;
        if (m_Datas.Count > 0)
        {
            /*
                Lock 是 Moniter 的封装
                    lock(obj){
                        //代码段
                    } 
                        就等同于 
                    Monitor.Enter(obj)； 
                    //代码段
                    Monitor.Exit(obj)；
                
                Enter(Object) 在指定对象上获取排他锁。
　　　　         Exit(Object) 释放指定对象上的排他锁。
                这里可以考虑使用try catch 保证 enter即使失败也能成功 exit
	        */
            Monitor.Enter(m_Datas);  // 任意两个线程都不可以同时进入这里的Moniter.Enter()方法
            if (m_Datas.Count > 0)
            {
                data = m_Datas.Dequeue();  // 数据出队列
            }
            Monitor.Exit(m_Datas);
        }
        return data;
    }

    public void Close()  // 关闭连接
    {
        if (OnDisconnected != null)
        {  // 通知委托关闭的事件
            this.OnDisconnected(this, new DSCClientConnectedEventArgs(m_Socket));
        }
        if (Connected)
        {
            m_bStopRun = true;
            Thread.Sleep(10);
            m_Socket.Shutdown(SocketShutdown.Both);  // 客户端服务端 socket 均关闭（这里无需半关闭）
            m_Socket.Close();
        }
        m_Socket = null;
    }

    public void ReConnect()  // 重新连接
    {
        Close();  // 先关闭
        _Init();  // 初始化
        _BeginConnect();  // 再连接
    }

    public bool Connected  // 返回是否连接
    {
        get { return m_Socket != null && m_Socket.Connected; }
    }

    #endregion

}

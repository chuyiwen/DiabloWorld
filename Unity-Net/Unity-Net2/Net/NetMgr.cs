using UnityEngine;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using LitJson;
using System.Threading;
public class BufferObject
{
    public Socket _tcpSock = null;
    public int BufferSize;
    public byte[] buffer;
    public int commandId;
    public void CreateBuffer()
    {
        buffer = new byte[BufferSize];
    }
    public StringBuilder Data = new StringBuilder();
}
public class NetMgr
{
    //包头长度
    private const int RecviceHeadLength = 17;
    //是否接收数据
    private static bool m_bIsReceive = false;
    //当前接收大小
    private static int m_nReceiveLength = 0;
    //当前创建的socket
    private static Socket _tcpSock = null;
    // 超时事件 允许线程通过发信号互相通信
    private static ManualResetEvent timeoutobject = new ManualResetEvent(false);
    //是否连接
    private static bool m_bIsConnect = false;
    //Socket错误信息
    private static Exception m_socketexception;
    public void Start(string host, int port)
    {
        _tcpSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ip = IPAddress.Parse(host);
        IPEndPoint ipe = new IPEndPoint(ip, port);
        Connect(ipe, 5);  // 超时时间是5秒
    }
    private void Connect(IPEndPoint ipe, int timeoutmsec)
    {
        timeoutobject.Reset();
        // 在异步方式下传入的连接尝试，它允许其他动作而不必等待连接建立才继续执行后面程序
        IAsyncResult ResultType = _tcpSock.BeginConnect(ipe, new AsyncCallback(CallbackConnect), _tcpSock);
        if (timeoutobject.WaitOne(timeoutmsec, false))
        {  // 这将引起等待线程无限期的阻塞（如果没有设置Timeout，这里设置了）并等待类来通知
            if (m_bIsConnect)
            {
                Receive(_tcpSock);  // 设置回调函数为CallbackConnect，如果连接成功，则进入回调
                                    // 在回调中将m_bIsConnect设置为true，所以连接成功进入Recive函数
            }
        }
        else {
            _tcpSock.Close();  // 这里是不是可以让他重新连接？
            Debug.Log("Connect not to Server!");
        }
        //connectDone.WaitOne ();
        //sendDone.WaitOne ();

    }
    // 连接回调
    private void CallbackConnect(IAsyncResult ar)
    {
        try
        {
            m_bIsConnect = false;
            var socket = ar.AsyncState as Socket;

            if (socket != null)
            {
                socket.EndConnect(ar);   // 和BeginConnect是一对的，实现与服务端的连接
                m_bIsConnect = true;  // 设置为连接成功
            }
        }
        catch (Exception ex)
        {
            m_bIsConnect = false;
            m_socketexception = ex;
        }
        finally
        {
            timeoutobject.Set();  // 发起信号，停止阻塞
        }
    }
    public void Receive(Socket _tcpSock)  // 在这里开始接受消息
    {
        BufferObject ReceiveBuffer = new BufferObject();  // BufferObject是自定义的类型
        ReceiveBuffer._tcpSock = _tcpSock;  // socket
        ReceiveBuffer.BufferSize = RecviceHeadLength;  // 17
        ReceiveBuffer.CreateBuffer();  // new 一个 byte[]
        m_nReceiveLength = ReceiveBuffer.BufferSize;  // 还有一个 commandId 后面接收到的时候才赋值
        m_bIsReceive = false;
        // 异步接受消息
        // buffer[] offset size flag callback   object
        _tcpSock.BeginReceive(ReceiveBuffer.buffer, 0, ReceiveBuffer.BufferSize, 0, new AsyncCallback(ReceiveCallback), ReceiveBuffer);
    }
    // 接受回调
    public void ReceiveCallback(IAsyncResult ar)
    {// 有没有处理粘包分包？？？
        try
        {
            BufferObject ReceiveBuffer = (BufferObject)ar.AsyncState;  // object 强转
            Socket m_Socket = ReceiveBuffer._tcpSock;
            int bytesRead = ReceiveBuffer._tcpSock.EndReceive(ar);  // 和BeginReceive是一对的。返回读取到的字节数
                                                                    //如果没有接收完毕则继续接收
            if (m_nReceiveLength > 0)
            {
                // Data = 协议头（协议号5+版本号4+长度4+命令号4） + 消息
                ReceiveBuffer.Data.Append(Encoding.Default.GetString(ReceiveBuffer.buffer, 0, bytesRead));
                m_nReceiveLength -= bytesRead;
                if (m_nReceiveLength <= 0)  // 17字节接收完了 注意是 <= 0
                {
                    if (m_bIsReceive)
                    {// true 包括消息全部到达
                        if (ReceiveBuffer.Data.Length == ReceiveBuffer.BufferSize)
                        {
                            JsonReader jsonR = new JsonReader(ReceiveBuffer.Data.ToString());  //取出消息
                            JsonData jData = JsonMapper.ToObject(jsonR);
                            // 接收到了消息，通知Manager   而发送是由Button事件发送
                            SocketManager.Instance.RecvMessage(ReceiveBuffer.commandId, jData);
                            Receive(m_Socket);  // 准备接收下一条消息
                            return;
                        }
                    }
                    else {// false 17字节到达但是消息没到达
                        if (ReceiveBuffer.Data.Length == ReceiveBuffer.BufferSize)
                        {  // 17?
                            int commandId, size;  // 一条消息全部到达
                            unPackHead(ReceiveBuffer.buffer, out commandId, out size);
                            m_bIsReceive = true;  // 接收到了
                            BufferObject RecvDataBuffer = new BufferObject();
                            RecvDataBuffer._tcpSock = ReceiveBuffer._tcpSock;
                            RecvDataBuffer.BufferSize = size - 4;  // size是（消息长度+命令号） 这里减去命令号，剩下消息长度
                            RecvDataBuffer.commandId = commandId;  // 取出命令号
                            m_nReceiveLength = RecvDataBuffer.BufferSize;
                            RecvDataBuffer.CreateBuffer();
                            // 继续接收 offset = 0， size = RecvDataBuffer.BufferSize
                            /*
                                意思是 系统只是在数据包到达时，尽可能的读取要求的字节数。
                                1.接收到消息 => callback被调用
                                2.接收够（或者已经够）RecvDataBuffer.BufferSize => 直接调用callback
                            */
                            _tcpSock.BeginReceive(RecvDataBuffer.buffer, 0, RecvDataBuffer.BufferSize, 0, new AsyncCallback(ReceiveCallback), RecvDataBuffer);
                            return;
                        }
                    }
                }
                // 继续接收
                _tcpSock.BeginReceive(ReceiveBuffer.buffer, 0, m_nReceiveLength, 0, new AsyncCallback(ReceiveCallback), ReceiveBuffer);
            }
            else  // 接受完了退出
            {
                // NONE
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    // commandId size 是 out 的
    private void unPackHead(byte[] head, out int commandId, out int size)
    {  // 解包
        int head0 = head[0];
        int head1 = head[1];
        int head2 = head[2];
        int head3 = head[3];
        int protoVersion = head[4];
        // buffer  sourceIndex起始
        int serverVersion = System.BitConverter.ToInt32(head, 5);
        // todo check the head, version
        System.Array.Reverse(head, 9, 4); // 从网络大端 转 小端
        size = System.BitConverter.ToInt32(head, 9); // 9 - 12 4字节 取出长度

        System.Array.Reverse(head, 13, 4); // 从网络大端 转 小端
        commandId = System.BitConverter.ToInt32(head, 13);  // 13 - 16 4字节 取出命令号
    }

    private byte[] packMsg(byte[] msg, int commandId)
    {  // 打包数据
        byte[] head = new byte[17];
        head[0] = 0;  // 协议头  随意
        head[1] = 1;
        head[2] = 2;
        head[3] = 3;
        head[4] = 4; // protoVersion 协议版本？

        // source sourceIndex dest destIndex（下标为5，前面已经占了5位） length 4个字节 内容随意
        Array.Copy(System.BitConverter.GetBytes(0), 0, head, 5, 4); // serverVersion  // 服务版本？

        // 以上共 5 + 4 = 9个字节

        byte[] lengthByte = new byte[4];
        lengthByte = System.BitConverter.GetBytes(msg.Length + 4);  // 消息长度+命令号
        Array.Reverse(lengthByte);  // 翻转byte[]  ？？？  大端小端？
                                    /* 
                                        网络数据传输用的是大端，Python的Struct在用"!"时也是用的大端解
                                        但是C#用的是小端，所以在发送或者接收某个数值的byte时需要reverse一下       
                                    */
        Array.Copy(lengthByte, 0, head, 9, 4);  // 在后面继续加入 长度 4个字节
        byte[] commandIdByte = new byte[4];
        commandIdByte = System.BitConverter.GetBytes(commandId);
        Array.Reverse(commandIdByte);  // 同上 从 小端 转换成 大端
        Array.Copy(commandIdByte, 0, head, 13, 4);  // 在后面继续加入 命令号 4个字节

        // 以上共 9 + 4 + 4 = 17个字节

        // 消息体由 协议头 + 消息 构成
        byte[] ret = new byte[head.Length + msg.Length];  // 在头部后面继续加入msg
        Array.Copy(head, 0, ret, 0, head.Length);
        Array.Copy(msg, 0, ret, head.Length, msg.Length);
        return ret;
    }

    public void SendMsg(byte[] msg, int commandId)
    {
        byte[] data = packMsg(msg, commandId);
        _tcpSock.Send(data, data.Length, 0);//发送信息
    }
}
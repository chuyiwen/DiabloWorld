#coding:utf8

# 客户端测试

import time

from socket import AF_INET,SOCK_STREAM,socket
from thread import start_new
import struct,json
HOST='192.168.1.120'  # 这里 127.0.0.1
PORT=11009
BUFSIZE=1024
ADDR=(HOST , PORT)
client = socket(AF_INET,SOCK_STREAM)
client.connect(ADDR)

def sendData(sendstr,commandId):
    """78,37,38,48,9,0"""
    # 协议头
    HEAD_0 = chr(78)
    HEAD_1 = chr(37)
    HEAD_2 = chr(38)
    HEAD_3 = chr(48)
    ProtoVersion = chr(9)
    ServerVersion = 0
    sendstr = sendstr  # 发送的数据
    '''
    在 strct pack 中
    s 表示 string类型
    I 表示 4位整型
    ! 表示采用网络序（大端序），python中采用大端序，而在C#中采用的小端序，这里需要注意
    '''
    # 消息由 消息头（head 4 + proto 1 + server 4 + len 4） + 消息体（command 4 + sendstr ？） 构成
    data = struct.pack('!sssss3I',HEAD_0,HEAD_1,HEAD_2,\
                       HEAD_3,ProtoVersion,ServerVersion,\
                       len(sendstr)+4,commandId)
    senddata = data+sendstr
    return senddata

# 解析数据
def resolveRecvdata(data):
    head = struct.unpack('!sssss3I',data[:17])
    lenght = head[6]  # head 是个列表
    data = data[17:17+lenght]  # command + msg
    return data

# 账号登录 101
def login():
    client.sendall(sendData(json.dumps({"username":"test106","password":"111111"}),101))

# 角色登录 103
def rolelogin():
    client.sendall(sendData(json.dumps({"userId":1915,"characterId":1000001}),103))

# 战斗 4501
def fight():
    client.sendall(sendData(json.dumps({"zjid":1000,"characterId":1000001}),4501))

# 模拟账号登录角色登录
login()
rolelogin()
# 模拟战斗
def start():
    for i in xrange(100):
        fight()

# 开启 10 个线程 fight
for i in range(10):
    start_new(start,())

# 不直接退出，需要手动退出
while True:
    pass


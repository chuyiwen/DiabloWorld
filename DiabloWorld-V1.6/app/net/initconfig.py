#coding:utf8
'''
Created on 2013-8-14

@author: lan (www.9miao.com)
'''
from firefly.server.globalobject import GlobalObject
from firefly.netconnect.datapack import DataPackProtoc

# 断开连接回调函数
def callWhenConnLost(conn):
    dynamicId = conn.transport.sessionno  # 动态id
    GlobalObject().remote['gate'].callRemote("netconnlost",dynamicId)  # 通知 gate 服务器与 id 断开连接

# 断开连接自动回调
GlobalObject().netfactory.doConnectionLost = callWhenConnLost
# 协议头 78 37 38 48 9 0
# 前四位是随意，第五位是Server，第六位是Version
dataprotocl = DataPackProtoc(78,37,38,48,9,0)
# 设置协议头
GlobalObject().netfactory.setDataProtocl(dataprotocl)


# 加载模块
def loadModule():
    import netapp
    import gatenodeapp
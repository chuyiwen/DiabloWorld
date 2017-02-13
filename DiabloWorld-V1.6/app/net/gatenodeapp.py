#coding:utf8
'''
Created on 2013-8-14

@author: lan (www.9miao.com)
'''
from firefly.server.globalobject import GlobalObject,remoteserviceHandle

# gate 是父节点名字，此时 net 成为子节点。
# 使用remoteserviceHandle方法（firefly内部已经已经好）修饰供gate调用的自定义方法pushObject。
# 没有被remoteserviceHandle修饰的方法，gate服务器不可调用。
@remoteserviceHandle('gate')
def pushObject(topicID,msg,sendList):
    # 返回数据消息
    GlobalObject().netfactory.pushObject(topicID, msg, sendList)

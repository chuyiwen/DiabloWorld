#coding:utf8
'''
Created on 2013-8-14

@author: lan (www.9miao.com)
'''
from firefly.server.globalobject import GlobalObject
from firefly.utils.services import CommandService


remoteservice = CommandService("gateremote")  # 远程服务节点
GlobalObject().remote['gate']._reference.addService(remoteservice)  # 添加到 gate


def remoteserviceHandle(target):
    """
    """
    remoteservice.mapTarget(target)



#coding:utf8
'''
Created on 2013-8-2

@author: lan (www.9miao.com)
'''
from server.globalobject import GlobalObject

def netserviceHandle(target):
    '''服务处理
    @param target: func Object
    '''
    GlobalObject().netfactory.service.mapTarget(target)  # 根据函数名映射

# 111 echo服务器
@netserviceHandle
def echo_111(_conn,data):
    return data




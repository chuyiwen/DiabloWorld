#coding:utf8
'''
Created on 2013-8-14

@author: lan (www.9miao.com)
'''
from dataloader import registe_madmin,CheckMemDB,MAdminManager,initData
from firefly.server.globalobject import GlobalObject

def doWhenStop():
    """服务器关闭前的处理
    """
    print "##############################"
    print "##########checkAdmins#############"
    print "##############################"
    MAdminManager().checkAdmins()
    
GlobalObject().stophandler = doWhenStop  # 停止时候的操作
    

def loadModule():
#     mclient.flush_all()
    registe_madmin()  # 注册数据库与memcached对应
    initData()  # 载入角色初始数据
    CheckMemDB(1800)  # 同步内存数据到数据库
    
    
    
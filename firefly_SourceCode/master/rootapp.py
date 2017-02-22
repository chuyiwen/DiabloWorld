#coding:utf8
'''
Created on 2013-8-7

@author: lan (www.9miao.com)
'''
from firefly.server.globalobject import GlobalObject
from twisted.python import log


def _doChildConnect(name,transport):
    """当server节点连接到master的处理
    """
    server_config = GlobalObject().json_config.get('servers',{}).get(name,{})  # servers下的gate dbfront net admin game1 等模块
    remoteport = server_config.get('remoteport',[])  # 获得 remoteport（gate的port）[{"rootport":"10000", "rootname":"gate"}]
    child_host = transport.broker.transport.client[0]  # 子节点主机地址
    root_list = [rootport.get('rootname') for rootport in remoteport]  # 如果有多个{}，就获取多个rootname
    GlobalObject().remote_map[name] = {"host":child_host,"root_list":root_list}  # map 映射
    #通知有需要连的node节点连接到此root节点
    for servername,remote_list in GlobalObject().remote_map.items():
        remote_host = remote_list.get("host","")  # 获得 远程子节点的主机地址
        remote_name_host = remote_list.get("root_list","")  # 获得 远程子节点的名字
        if name in remote_name_host:  # 可连接
            # 子节点主机地址  "remote_connect"  root节点名  root主机地址
            GlobalObject().root.callChild(servername,"remote_connect",name,remote_host)
    #查看当前是否有可供连接的root节点
    master_node_list = GlobalObject().remote_map.keys()  # 远程节点列表
    for root_name in root_list:  # server节点的root
        if root_name in master_node_list:  # 可连接
            root_host = GlobalObject().remote_map[root_name]['host']  # root的主机地址
            # 子节点主机地址  "remote_connect"  root节点名  root主机地址
            GlobalObject().root.callChild(name,"remote_connect",root_name,root_host)
    
def _doChildLostConnect(childId):
    """
    当server节点断开连接的处理
    """
    try:
        del GlobalObject().remote_map[childId]  # 移除
    except Exception,e:
        log.msg(str(e))

GlobalObject().root.doChildConnect = _doChildConnect
GlobalObject().root.doChildLostConnect = _doChildLostConnect
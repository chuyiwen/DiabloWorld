#coding:utf8
'''
Created on 2013-8-14

@author: lan (www.9miao.com)
'''
from firefly.server.globalobject import rootserviceHandle,GlobalObject
from app.gate.gateservice import localservice
from app.gate.core.UserManager import UsersManager
from app.gate.core.VCharacterManager import VCharacterManager
from app.gate.core.scenesermanger import SceneSerManager

@rootserviceHandle
def forwarding(key,dynamicId,data):
    """
    """
    if localservice._targets.has_key(key):
        return localservice.callTarget(key,dynamicId,data)
    else:
        user = UsersManager().getUserByDynamicId(dynamicId)
        if not user:
            return
        oldvcharacter = VCharacterManager().getVCharacterByClientId(dynamicId)
        if not oldvcharacter:
            return
        if oldvcharacter.getLocked():#判断角色对象是否被锁定
            return
        node = VCharacterManager().getNodeByClientId(dynamicId)  # 节点id
        return GlobalObject().root.callChild(node,key,dynamicId,data)  # 调用node
    

@rootserviceHandle
def pushObject(topicID,msg,sendList):
    """
    添加消息到 net
    """
    GlobalObject().root.callChild("net","pushObject",topicID,msg,sendList)  # 调用net
    
@rootserviceHandle
def opera_player(pid,oprea_str):
    """
    """
    # vcharacter是虚拟角色，VCharacterManager()虚拟角色管理器，{角色id:虚拟角色实例}
    vcharacter = VCharacterManager().getVCharacterByCharacterId(pid)
    if not vcharacter:  # 没有管理器
        node = "game1"
    else:  # 已存在，直接获取
        node = vcharacter.getNode()
    GlobalObject().root.callChild(node,99,pid,oprea_str)  # 调用node
    

def SavePlayerInfoInDB(dynamicId):
    '''将玩家信息写入数据库'''
    vcharacter = VCharacterManager().getVCharacterByClientId(dynamicId)
    node = vcharacter.getNode()
    d = GlobalObject().root.callChild(node,2,dynamicId)  # 调用node
    return d

def SaveDBSuccedOrError(result,vcharacter):
    '''写入角色数据成功后的处理
    @param result: 写入后返回的结果
    @param vcharacter: 角色的实例
    '''
    vcharacter.release()#释放角色锁定
    return True

def dropClient(deferResult,dynamicId,vcharacter):
    '''清理客户端的记录
    @param result: 写入后返回的结果
    '''
    node = vcharacter.getNode()
    if node:#角色在场景中的处理
        SceneSerManager().dropClient(node, dynamicId)
        
    VCharacterManager().dropVCharacterByClientId(dynamicId)  # 从管理器中移除
    UsersManager().dropUserByDynamicId(dynamicId)  # 从用户列表中移除

@rootserviceHandle
def netconnlost(dynamicId):
    '''客户端断开连接时的处理
    @param dynamicId: int 客户端的动态ID
    '''
    vcharacter = VCharacterManager().getVCharacterByClientId(dynamicId)
    if vcharacter and vcharacter.getNode()>0:#判断是否已经登入角色
        vcharacter.lock()#锁定角色
        d = SavePlayerInfoInDB(dynamicId)#保存角色,写入角色数据
        d.addErrback(SaveDBSuccedOrError,vcharacter)#解锁角色
        d.addCallback(dropClient,dynamicId,vcharacter)#清理客户端的数据
    else:
        UsersManager().dropUserByDynamicId(dynamicId)






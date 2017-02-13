#coding:utf8
'''
Created on 2012-3-1

@author: sean_lan
'''
from app.gate.core.User import User
from app.gate.core.UserManager import UsersManager
from app.gate.core.virtualcharacter import VirtualCharacter
from app.gate.core.VCharacterManager import VCharacterManager
from app.share.dbopear import dbuser

from app.gate.core.scenesermanger import SceneSerManager
from firefly.server.globalobject import GlobalObject


def loginToServer(dynamicId,username ,password):
    '''登陆服务器
    @param dynamicId: int 客户端动态ID
    @param username: str 用户名
    @param password: str 用户密码
    '''
    if password=='crotaii':  # ？？？什么鬼啊这是
        return{'result':False}
    userinfo = dbuser.CheckUserInfo(username)  # 检测用户信息

    # 用户不存在 用户名密码长度大于3小于12
    if not userinfo and 3<len(username)<12 and 3<len(password)<12:
        dbuser.creatUserInfo(username, password)  # 用户不存在，创建用户信息

    oldUser = UsersManager().getUserByUsername(username)  # 根据用户名获取用户信息
    # 存在账号
    if oldUser:
        oldUser.dynamicId = dynamicId  # 更新下动态id
        UserCharacterInfo = oldUser.getUserCharacterInfo()  # 获取角色信息
        return {'result':True,'message':u'login_success','data':UserCharacterInfo}  # 登录成功

    # 创建新账号，构造 User
    user = User(username,password,dynamicId = dynamicId)
    if user.id ==0:  # 密码错误
        return {'result':False,'message':u'psd_error'}
    if not user.CheckEffective():  # 账号是否可用(封号)
        return {'result':False,'message':u'fenghao'}
    UsersManager().addUser(user)  # 添加一个用户
    UserCharacterInfo = user.getUserCharacterInfo()  # 获取角色信息
    return{'result':True,'message':u'login_success','data':UserCharacterInfo}


def activeNewPlayer(dynamicId,userId,nickName,profession):
    '''创建角色
    arguments=(userId,nickName,profession)
    userId用户ID
    nickName角色昵称
    profession职业选择
    '''
    user=UsersManager().getUserByDynamicId(dynamicId)
    if not user: # 用户不存在
        return {'result':False,'message':u'conn_error'}
    if not user.checkClient(dynamicId):  # 检测客户端ID是否匹配
        return {'result':False,'message':u'conn_error'}
    if user is None: # 用户断开连接
        return {'result':False,'message':u'disconnect'}
    result = user.creatNewCharacter(nickName, profession)  # 创建新角色
    return result


def deleteRole(dynamicId, userId, characterId,password):
    '''删除角色
    @param dynamicId: int 客户端的ID
    @param userId: int 用户端ID
    @param characterId: int 角色的ID
    @param password: str 用户的密码
    '''
    user=UsersManager().getUserByDynamicId(dynamicId)
    if not user.checkClient(dynamicId):  # 检测客户端ID是否匹配
        return {'result':False,'message':u'conn_error'}
    if user is None: # 用户断开连接
        return {'result':False,'message':u'disconnect'}
    result = user.deleteCharacter(characterId,password)  # 删除角色
    return result

def roleLogin(dynamicId, userId, characterId):
    '''角色登陆
    @param dynamicId: int 客户端的ID
    @param userId: int 用户的ID
    @param characterId: int 角色的ID
    '''
    user=UsersManager().getUserByDynamicId(dynamicId)
    if not user:
        return {'result':False,'message':u'conn_error'}
    characterInfo = user.getCharacterInfo()
    if not characterInfo:  # 没有角色信息
        return {'result':False,'message':u'norole'}
    _characterId = user.characterId
    if _characterId!=characterId:  # 角色id不对应
        return {'result':False,'message':u'norole'}
    # 虚拟角色管理类，并由这个管理类来管理一批登陆的虚拟角色
    # 获取最后次下线的虚拟角色
    oldvcharacter = VCharacterManager().getVCharacterByCharacterId(characterId)
    data = {'placeId':characterInfo.get('town',1000)}  # 获取角色最后所在城镇id，默认为1000
    if oldvcharacter:
        oldvcharacter.setDynamicId(dynamicId)  # 更新动态id
    else:
        vcharacter = VirtualCharacter(characterId,dynamicId)  # 构造一个虚拟角色
        VCharacterManager().addVCharacter(vcharacter)  # 添加到管理类中
    return {'result':True,'message':u'login_success','data':data}
    
def enterScene(dynamicId, characterId, placeId,force):
    '''进入场景
    @param dynamicId: int 客户端的ID
    @param characterId: int 角色的ID
    @param placeId: int 场景的ID
    @param force: bool 
    '''
    vplayer = VCharacterManager().getVCharacterByClientId(dynamicId)
    if not vplayer:
        return None
    nownode = SceneSerManager().getBestScenNodeId()  # 获取最佳的game服务器
    # 调用子节点 childname, *args
    d = GlobalObject().root.callChild(nownode,601,dynamicId, characterId, placeId,force,None)
    vplayer.setNode(nownode)  # 设置节点服务器
    SceneSerManager().addClient(nownode, vplayer.dynamicId)  # 添加到场景服务器
    return d


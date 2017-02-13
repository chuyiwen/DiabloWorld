#coding:utf8
'''
Created on 2012-2-27

@author: sean_lan
'''
from app.gate.appinterface import login
import json
from app.gate.gateservice import localserviceHandle  # 服务处理


@localserviceHandle
def loginToServer_101(key,dynamicId,request_proto):
    """
    账号登录
    """
    argument = json.loads(request_proto)
    dynamicId = dynamicId
    username = argument.get('username')
    password = argument.get('password')
    data = login.loginToServer(dynamicId, username, password)  # 账号登录 返回角色信息
    response = {}
    _data = data.get('data')
    response['result']=data.get('result',False)
    responsedata = {}
    response['data'] = responsedata
    if _data:
        responsedata['userId'] = _data.get('userId',0)  # 默认 id为0
        responsedata['hasRole'] = _data.get('hasRole',False)  # 默认 没有角色
        responsedata['characterId'] = _data.get('defaultId',False)  # ？？？ 怎么是False？默认不应该是0吗  （在User中赋值）
    return json.dumps(response)


@localserviceHandle
def activeNewPlayer_102(key,dynamicId,request_proto):
    """
    创建角色
    """
    argument = json.loads(request_proto)
    userId = argument.get('userId')
    nickName = argument.get('rolename')
    profession = int(argument.get('profession'))
    data  = login.activeNewPlayer(dynamicId, userId, nickName, profession)  # 创建
    return json.dumps(data)


def SerializePartialEnterScene(result,response):
    '''序列化进入场景的返回消息
    '''
    return json.dumps(result)


@localserviceHandle
def roleLogin_103(key,dynamicId, request_proto):
    '''角色登陆'''
    argument = json.loads(request_proto)
    userId = argument.get('userId')
    characterId = argument.get('characterId')
    data = login.roleLogin(dynamicId, userId, characterId)  # 角色登录
    if not data.get('result'):
        return json.dumps(data)
    placeId = data['data'].get('placeId',1000)
    response = {}
    dd = login.enterScene(dynamicId, characterId, placeId, True)  # 进入场景
    if not dd:
        return
    # defer回调，结果出来后自动调用
    # SerializePartialEnterScene 序列化进入场景的返回消息
    dd.addCallback(SerializePartialEnterScene,response)
    return dd




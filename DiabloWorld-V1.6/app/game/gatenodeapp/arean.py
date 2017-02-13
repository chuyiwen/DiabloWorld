#coding:utf8
'''
Created on 2013-7-17

@author: lan (www.9miao.com)
'''
from app.game.gatenodeservice import remoteserviceHandle
from app.game.appinterface import arena
import json

# request_proto 请求来自客户端，经过gate转发到达这里

@remoteserviceHandle
def GetJingJiInfo_3700(dynamicId, request_proto):
    '''获取竞技场信息
    '''
    argument = json.loads(request_proto)
    characterId = argument.get('characterId')
    data = arena.GetJingJiInfo3700(dynamicId, characterId)  # 获取竞技场信息
    return json.dumps(data)

@remoteserviceHandle
def ArenaBattle_3704(dynamicId, request_proto):
    '''竞技场战斗
    战斗系统由服务器这边统一处理计算，处理后数据发送回客户端
    '''
    argument = json.loads(request_proto)
    characterId = argument.get('characterId')
    tocharacterId = argument.get('tid')
    data = arena.ArenaBattle_3704(dynamicId, characterId, tocharacterId)  # 竞技场战斗
    response = {}
    response['result'] = data.get('result',False)
    response['message'] = data.get('message','')
    _responsedata = data.get('data')
    if _responsedata:
        battle = _responsedata.get('fight')
        setData = _responsedata.get('setData')
        fightdata = battle.formatFightData()  # 格式化战斗的信息
        # response['data'] = fightdata fightdata还没经过处理呢...应该处理完再赋值吧？
        fightdata['battleResult'] = battle.battleResult
        fightdata['setData'] = setData
        response['data'] = fightdata
    return json.dumps(response)


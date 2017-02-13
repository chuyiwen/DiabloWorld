#coding:utf8
'''
Created on 2012-7-1
竞技场操作
@author: Administrator
'''
from app.game.core.PlayersManager import PlayersManager

# 3700
def GetJingJiInfo3700(dynamicId,characterId):
    '''获取竞技场信息
    '''
    player = PlayersManager().getPlayerByID(characterId)
    if not player or not player.CheckClient(dynamicId):  # 没有角色 或 角色动态id不匹配
        return {'result':False,'message':u""}
    data = player.arena.getArenaAllInfo()  # 获取竞技场所有的信息
    return {'result':True,'data':data}


# 3704
def ArenaBattle_3704(dynamicId,characterId,tocharacterId):
    '''竞技场战斗
    '''
    player = PlayersManager().getPlayerByID(characterId)
    if not player or not player.CheckClient(dynamicId):  # 没有角色 或 角色动态id不匹配
        return {'result':False,'message':""}
    result = player.arena.doFight(tocharacterId)  # 执行战斗
    return result
    

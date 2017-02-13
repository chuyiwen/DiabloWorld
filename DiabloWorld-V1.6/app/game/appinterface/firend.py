#coding:utf8
'''
Created on 2011-4-14
好友信息
@author: sean_lan
'''
from app.game.core.PlayersManager import PlayersManager
from app.game.core.character.PlayerCharacter import PlayerCharacter
from app.game.core.language.Language import Lg


def GetFriendList(dynamicId,characterId,tag,index):
    '''获取好友排行信息
    '''
    player = PlayersManager().getPlayerByID(characterId)
    if not player or not player.CheckClient(dynamicId):
        return {'result':False,'message':Lg().g(18)}
    response = player.friend.getFriendTop(tag,index)  # 获取好友的排行信息
    return response
    

def GetSingleCharacterInfo(dynamicId,characterId,chtype,tid):
    '''获取单个角色的信息
    @param chtype: int 角色的类型 1角色自身 2好友 3宠物
    '''
    player = PlayersManager().getPlayerByID(characterId)
    if not player or not player.CheckClient(dynamicId):
        return {'result':False,'message':Lg().g(18)}
    if chtype in [1,2]:  # 角色自身或者好友
        return _getOtherCharacterInfo(tid)  # 获取其他玩家的信息
    else:  # 宠物
        return _getOnePetInfo(characterId,tid)  # 获取宠物的信息

    
def _getOnePetInfo(characterId,tid):
    '''获取宠物的信息
    '''
    player = PlayersManager().getPlayerByID(characterId)
    pet = player.pet.getPet(tid)  # 获取指定的宠物
    if not pet:
        return {'result':False,'message':u'该宠物信息不存在'}
    info = pet.formatInfoForWeiXin()  # 格式化角色信息
    return {'result':True,'message':u'','data':info}
    
    
def _getOtherCharacterInfo(tid):
    '''获取其他玩家的信息
    '''
    player = PlayersManager().getPlayerByID(tid)
    if not player:
        try:
            player = PlayerCharacter(tid)  # 根据id构造玩家
        except:
            player = None
    if not player:
        return {'result':False,'message':u'该角色信息不存在'}
    info = player.formatInfoForWeiXin()  # 格式化角色信息
    return {'result':True,'message':u'','data':info}
    
    
def GuYongHaoYou(dynamicId,characterId,tid):
    '''雇佣好友
    '''
    player = PlayersManager().getPlayerByID(characterId)  # 根据角色id获取玩家角色实例
    if not player or not player.CheckClient(dynamicId):
        return {'result':False,'message':Lg().g(18)}
    response = player.friend.GuYongHaoYou(tid)  # 雇用好友
    return response
    
def getGuyongRecord(dynamicId,characterId):
    '''获取雇用记录
    '''
    player = PlayersManager().getPlayerByID(characterId)  # 根据角色id获取玩家角色实例
    if not player or not player.CheckClient(dynamicId):
        return {'result':False,'message':Lg().g(18)}
    response = player.friend.GetGuyongRecord()  # 获取雇用记录
    return response
    
    
    
    

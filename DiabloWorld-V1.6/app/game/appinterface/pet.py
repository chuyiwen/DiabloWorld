#coding:utf8
'''
Created on 2011-12-19
角色武将信息
@author: lan (www.9miao.com)
'''

from app.game.core.PlayersManager import PlayersManager


def GetAllPetListFormatForWeixin(dynamicId,characterId):
    '''获取所有能上阵的武将的信息
    '''
    player = PlayersManager().getPlayerByID(characterId)
    if not player or not player.CheckClient(dynamicId):
        return {'result':False,'message':u""}
    petList = player.pet.FormatPetList()  # 格式化所有宠物的信息
    return {'result':True,'data':petList}
    
    
def GetCharacterMatrixInfo(dynamicId,characterId):
    '''获取角色的阵法信息
    '''
    player = PlayersManager().getPlayerByID(characterId)
    if not player or not player.CheckClient(dynamicId):
        return {'result':False,'message':u""}
    matrixinfo = player.matrix.FormatMatrixInfoForWeixin()  # 格式化角色的阵法设置信息
    return {'result':True,'data':matrixinfo}
    

def GetPetListInfo(dynamicId,characterId):
    '''获取角色的武将列表信息'''
    player = PlayersManager().getPlayerByID(characterId)
    if not player:
        return {'result':False,'message':u""}
    petList = player.pet.getCharacterPetListInfo()  # 获取角色宠物列表
    return {'result':True,'data':petList}

def GetPetMatrixList(dynamicId,characterId):
    '''获取阵法设置'''
    player = PlayersManager().getPlayerByID(characterId)
    if not player:
        return {'result':False,'message':u""}
    data = player.matrix.getMatrixListSetting()  # 获取阵法列表设置
    return {'result':True,'data':data}

def SettingMatrix(dynamicId,characterId,petId,chatype,operationType,fromPos,toPos):
    '''阵法设置'''
    player = PlayersManager().getPlayerByID(characterId)
    if not player:
        return {'result':False,'message':u""}
    result = player.matrix.updateMatrix(petId,chatype,operationType,fromPos,toPos)  # 更新阵法位置信息
    return result

def GetOnePetInfo(dynamicId,characterId, petId,masterId):
    '''获取单个武将的信息'''
    # 这里的 masterId？？？ characterId就够了吧
    player = PlayersManager().getPlayerByID(characterId)
    if not player:
        return {'result':False,'message':u""}
    toplayer = PlayersManager().getPlayerByID(masterId)  # 根据角色id获取玩家角色实例
    if not toplayer:
        return {'result':False,'message':u""}
    else:
        pet = toplayer.pet.getPet(petId)
    return {'result':True,'data':pet}

def SwallowPet(dynamicId, characterId,petid,tpetid):
    """武将吞噬
    """
    player = PlayersManager().getPlayerByID(characterId)
    if not player:
        return {'result':False,'message':u""}
    result = player.pet.SwallowPet(petid,tpetid)  # 武将吞噬
    return result

#coding:utf8
'''
Created on 2013-3-21
合成信息
@author: lan (www.9miao.com)
'''
from app.game.gatenodeservice import remoteserviceHandle
import json
from app.game.appinterface import compound  # 导入 compound

# 2109
@remoteserviceHandle
def GetCompoundPackage_2109(dynamicId,request_proto):
    '''获取合成包裹的信息
    '''
    argument = json.loads(request_proto)
    characterId = argument.get('characterId')
    response = compound.GetCompoundPackage_2109(dynamicId, characterId)
    return json.dumps(response)

# 211
@remoteserviceHandle
def GetOneItemInfo_211(dynamicId,request_proto):
    '''获取单个物品的信息
    '''
    argument = json.loads(request_proto)
    characterId = argument.get('characterId')
    itemid = argument.get('itemid')
    response = compound.GetOneItemInfo(dynamicId, characterId,itemid)
    return json.dumps(response)

# 205
@remoteserviceHandle
def GetCompoundItem_205(dynamicId,request_proto):
    """获取当前碎片能合成的物品的信息
    """
    argument = json.loads(request_proto)
    characterId = argument.get('characterId')
    tempid = argument.get('tempid')
    response = compound.GetCompoundItem(dynamicId, characterId, tempid)
    return json.dumps(response)

# 2116
@remoteserviceHandle
def CompoundItem_2116(dynamicId,request_proto):
    '''合成物品
    '''
    argument = json.loads(request_proto)
    characterId = argument.get('characterId')
    tempid = argument.get('tempid')
    response = compound.CompoundItem(dynamicId, characterId, tempid)
    return json.dumps(response)



    


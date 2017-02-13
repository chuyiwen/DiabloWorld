#coding:utf8
'''
Created on 2013-3-21

@author: lan (www.9miao.com)
'''
from app.game.gatenodeservice import remoteserviceHandle
import json
from app.game.appinterface import packageInfo  # 导入 packageInfo

# 203
@remoteserviceHandle
def getItemsInEquipSlot_203(dynamicId,request_proto):
    """获取角色的装备栏信息
    """
    argument = json.loads(request_proto)
    characterId = argument.get('characterId')
    response = packageInfo.getItemsInEquipSlotNew(dynamicId, characterId)
    return json.dumps(response)

# 204
@remoteserviceHandle
def getItemInPackage_204(dynamicId,request_proto):
    """获取角色的包裹信息
    """
    argument = json.loads(request_proto)
    characterId = argument.get('characterId')
    response = packageInfo.GetPackageInfo(dynamicId, characterId)
    return json.dumps(response)
    
# 210
@remoteserviceHandle
def UserItemNew_210(dynamicId,request_proto):
    '''使用物品'''
    argument = json.loads(request_proto)
    characterId = argument.get('characterId')
    tempid = argument.get('itemid')
    response = packageInfo.UserItemNew(dynamicId,characterId,tempid)
    return json.dumps(response)

# 215
@remoteserviceHandle
def unloadedEquipment_215(dynamicId,request_proto):
    '''卸下装备'''
    argument = json.loads(request_proto)
    characterId = argument.get('characterId')
    itemId = argument.get('itemid')
    response = packageInfo.unloadedEquipment_new(dynamicId, characterId, itemId)
    return json.dumps(response)


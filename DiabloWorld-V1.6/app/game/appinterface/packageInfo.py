#coding:utf8
'''
Created on 2011-4-13

@author: sean_lan
'''
from app.game.core.PlayersManager import PlayersManager
    
def getItemsInEquipSlotNew(dynamicId,characterId):
    '''获取角色的装备栏信息
    @param dynamicId: int 客户端的id
    @param characterId: int 角色的id
    '''
    player = PlayersManager().getPlayerByID(characterId)
    if not player or not player.CheckClient(dynamicId):
        return {'result':False,'message':""}
    equipmentList = player.pack.getEquipmentSlotItemList()  # 获取装备栏信息
    keys_copy = dict(equipmentList)  # 二维列表（pos,itemid） 转字典
    equipmentList_copy = []
    for position in range(1,7):
        item = keys_copy.get(position,None)  # 根据位置获取对应格子的信息
        if item:
            _item = {}
            _item['itemid'] = item.baseInfo.id
            _item['icon'] = item.baseInfo.getItemTemplateInfo().get('icon',0)  # 获取物品的icon
            _item['tempid'] = item.baseInfo.getItemTemplateId()  # 获取物品模板Id
            _item['exp'] = item.exp
            iteminfo = {'pos':position,'item':_item}
            equipmentList_copy.append(iteminfo)
    playerInfo = player.formatInfoForWeiXin()  # 格式化角色信息
    data = {}
    data['equip'] = equipmentList_copy
    data['attack'] = playerInfo['attack']
    data['fangyu'] = playerInfo['fangyu']
    data['minjie'] = playerInfo['minjie']
    return {'result':True,'message':u'','data':data}


def UserItemNew(dynamicId,characterId,tempid):
    '''使用物品
    '''
    player = PlayersManager().getPlayerByID(characterId)
    if not player or not player.CheckClient(dynamicId):
        return {'result':False,'message':""}
    data = player.pack.equipEquipmentByItemId(tempid)  # 穿上装备
    return data
    
def GetPackageInfo(dynamicId,characterId):
    '''获取包裹的信息
    '''
    player = PlayersManager().getPlayerByID(characterId)
    if not player or not player.CheckClient(dynamicId):
        return {'result':False,'message':""}
    data = player.pack.getPackageItemList()  # 获取包裹的物品信息
    return data
    
    
def unloadedEquipment_new(dynamicId, characterId, itemId):
    '''卸下装备
    '''
    player = PlayersManager().getPlayerByID(characterId)
    if not player or not player.CheckClient(dynamicId):
        return {'result':False,'message':""}
    data = player.pack.unloaded(itemId)  # 卸下装备
    return data
    


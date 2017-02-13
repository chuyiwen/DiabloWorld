#coding:utf8
'''
Created on 2013-8-14

@author: lan (www.9miao.com)
'''
from app.game.gatenodeservice import remoteserviceHandle
from app.game.core.PlayersManager import PlayersManager


# 2
@remoteserviceHandle
def NetConnLost_2(dynamicId):
    '''loginout
    '''
    player = PlayersManager().getPlayerBydynamicId(dynamicId)
    if not player:
        return True
    player.updatePlayerDBInfo()  # 更新角色在数据库中的数据
    PlayersManager().dropPlayer(player)  # 移除角色
    return True
    

#coding:utf8
'''
Created on 2013年9月4日

@author: MSI
'''
from app.game.gatenodeservice import remoteserviceHandle
from app.game.core.PlayersManager import PlayersManager
from app.game.core.character.PlayerCharacter import PlayerCharacter

# 99
@remoteserviceHandle
def operaplayer_99(pid,oprea_str):
    """执行后台管理脚本
    """
    player = PlayersManager().getPlayerByID(pid)
    isOnline = 1
    if not player:
        player = PlayerCharacter(pid)
        isOnline = 0
    # player.finance.addCoin(1000)脚本例子，通过角色类进行角色的各种操作，player.XXX.XXX
    exec(oprea_str)
    if isOnline == 0:
        # 不在线就更新角色在数据库中的数据？？？不懂这里
        player.updatePlayerDBInfo()
    
    
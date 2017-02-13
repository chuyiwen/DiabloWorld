#coding:utf8
'''
Created on 2013-8-14

@author: lan (www.9miao.com)
'''
from dataloader import load_config_data,registe_madmin
from firefly.server.globalobject import GlobalObject
from app.game.core.PlayersManager import PlayersManager
from twisted.python import log

def doWhenStop():
    """服务器关闭前的处理
    """
    for player in PlayersManager()._players.values():  # 遍历所有玩家
        try:
            player.updatePlayerDBInfo()  # 将所有玩家更新到数据库
            PlayersManager().dropPlayer(player)  # 移除玩家
        except Exception as ex:
            log.err(ex) 
    
GlobalObject().stophandler = doWhenStop  # 停止时候的操作

def loadModule():
    """
    """
    load_config_data()  # 加载配置数据
    registe_madmin()  # 注册数据库与memcached对应

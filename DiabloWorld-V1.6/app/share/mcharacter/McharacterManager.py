#-*-coding:utf8-*-
'''
Created on 2013-4-27

@author: lan
'''
from firefly.utils.singleton import Singleton
from app.share.dbopear import dbCharacter
from firefly.dbentrust.memclient import mclient
from mcharacter import Mcharacter

class McharacterManager:
    
    __metaclass__ = Singleton  # 单例
    
    def __init__(self):
        '''初始化
        @param fortresss: dict{fortressID:fortress} 所有要塞的集合
        '''
        
    def initData(self):
        """初始化角色信息
        """
        allmcharacter = dbCharacter.getALlCharacterBaseInfo()  # 所有角色基础信息
        for cinfo in allmcharacter:
            pid = cinfo['id']
            mcha = Mcharacter(pid,'character%d'%pid,mclient)  # 构造 Mcharacter
            mcha.initData(cinfo)
            mcha.insert()

    # 通过 id 获取角色信息
    def GetCharacterInfoById(self,pid):
        '''
        '''
        mcha = Mcharacter(pid,'character%d'%pid,mclient)
        return mcha.mcharacterinfo

    # 通过 id 获取 Mcharacter
    def getMCharacterById(self,pid):
        '''
        '''
        mcha = Mcharacter(pid,'character%d'%pid,mclient)
        return mcha
            
        
        
        
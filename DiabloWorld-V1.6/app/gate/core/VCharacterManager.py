#coding:utf8
'''
Created on 2012-3-2

@author: sean_lan
'''
from firefly.utils.singleton import Singleton

class VCharacterManager:
    '''角色管理器'''
    
    __metaclass__ = Singleton  # 单例

    def __init__(self):
        '''记录角色ID与客户端id的关系'''
        self.character_client = {}  # 角色id -> 客户端id
        self.client_character = {}  # 客户端id -> 角色id
        
    def addVCharacter(self,vcharacter):
        '''添加一个角色的虚拟角色类
        @param vcharacter: VirtualCharacter Object
        '''
        characterId = vcharacter.getCharacterId()  # 获取角色的ID
        self.character_client[characterId] = vcharacter  # 添加到 角色id -> 客户端id
        
    def getVCharacterByClientId(self,clientId):
        '''根据客户端ID获取虚拟角色
        @param clientId: int 客户端的id
        '''
        for vcharacter in self.character_client.values():  # 遍历 角色字典
            if vcharacter.getDynamicId()==clientId:
                return vcharacter  # 返回虚拟角色
        return None
    
    def getVCharacterByCharacterId(self,characterId):
        '''根据角色的ID获取角色'''
        vcharacter = self.character_client.get(characterId)  # 从角色字典 获取虚拟角色
        return vcharacter
        
    def dropVCharacterByClientId(self,clientId):
        '''删除角色
        @param clientId: int 客户端的动态ID
        '''
        try:
            vcharacter = self.getVCharacterByClientId(clientId)  # 根据客户端ID获取虚拟角色
            if vcharacter:
                characterId = vcharacter.getCharacterId()  # 获取角色id
                del self.character_client[characterId]  # 从角色字典 删除虚拟角色
            else:
                pass
        finally:
            pass
        
    def dropVCharacterByCharacterId(self,characterId):
        '''删除角色
        @param clientId: int 客户端的动态ID
        '''
        try:
            del self.character_client[characterId]  # 从角色字典 删除虚拟角色
        finally:
            pass
        
    def getNodeByClientId(self,dynamicId):
        '''根据客户端的ID获取服务节点的id
        @param dynamicId: int 客户端的id
        '''
        vcharacter = self.getVCharacterByClientId(dynamicId)  # 根据客户端ID获取虚拟角色
        if vcharacter:
            return vcharacter.getNode()  # 返回节点
        return -1
    
    def getNodeByCharacterId(self,characterId):
        '''根据角色的ID获取服务节点的ID
        @param characterId: int 角色的ID 
        '''
        vcharacter = self.character_client.get(characterId)  # 从角色字典 获取虚拟角色
        if vcharacter:
            return vcharacter.getNode()  # 返回节点
        return -1
    
    def getClientIdByCharacterId(self,characterId):
        '''根据角色的ID获取客户端的ID
        @param characterId: int 角色的ID
        '''
        vcharacter = self.character_client.get(characterId)  # 从 角色字典 获取虚拟角色
        if vcharacter:
            return vcharacter.getDynamicId()  # 返回客户端id
        return -1
    
    def getCharacterIdByClientId(self,dynamicId):
        '''根据客户端的ID获取角色的ID
        @param characterId: int 角色的ID
        '''
        vcharacter = self.getVCharacterByClientId(dynamicId)  # 根据客户端ID获取虚拟角色
        if vcharacter:
            return vcharacter.getCharacterId()  # 返回角色id
        return -1
    
            
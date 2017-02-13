#coding:utf8
'''
Created on 2011-9-5

@author: lan (www.9miao.com)
'''

PLAYER_PLAYER =1
PLAYER_PET = 2
MONSTER_MONSTER =1
MATRIXLIST = [100001,100002,100003,100004,100005,100006,100007,100008,100009]

class BattleSide(object):
    '''战斗方类'''
    
    def __init__(self,members,state = 1,matrixType = PLAYER_PET,
                 matrixSetting = {},
                 preDict = {'extVitper':1,'extStrper':1,
                            'extDexper':1,'extWisper':1,'extSpiper':1}):
        '''初始化战斗方'''
        self.matrixType = matrixType  # 战斗方
        self.preDict = preDict
        self.members = []
        self.matrixSetting = {}
        self.lord = members[0].baseInfo.id#主将的ID
        if members[0].getCharacterType()==1 and matrixType==PLAYER_PET:  # PLAYER_PET 类型
            player = members[0]  # 玩家
            self.matrixType = PLAYER_PET
            self.members = []
            for eyeNo in range(1,10):
                memID = player.matrix._matrixSetting.get('eyes_%d'%eyeNo)  # 得到阵法对应位置的id
                if memID<0:
                    continue
                if not memID:
                    self.members.append(player)
                    memID = player.baseInfo.id
                    self.matrixSetting[memID] = eyeNo
                elif memID>=3000000:
                    pet = player.pet.getPet(memID)
                    if pet and pet.attribute.getHp()>0:
                        self.members.append(pet)
                        self.matrixSetting[memID] = eyeNo
                else:
                    from app.game.core.character import PlayerCharacter
                    pet = PlayerCharacter.PlayerCharacter(memID)
                    self.members.append(pet)
                    self.matrixSetting[memID] = eyeNo
        elif matrixType==PLAYER_PLAYER:  # PLAYER_PLAYER 类型
            self.members = members
            self.matrixSetting = matrixSetting
            if not matrixSetting:  # 没有阵法位置表
                self.autoPosition()
        else:  # MONSTER_MONSTER 类型
            self.members = members
            if state and not self.matrixSetting:  # 没有阵法位置表
                self.autoPosition()
            else:
                self.matrixSetting = matrixSetting
    
    def autoPosition(self):
        '''自动更新阵法位置
        '''
        # rule 是 固定的？
        rule = [9,8,7,6,4,3,2,1,5]
        for index in range(len(self.members)):
            character = self.members[index]
            self.matrixSetting[character.baseInfo.id] = rule[index]
        
        
    def setMatrixPositionBatch(self,rule):
        '''批量设置阵法的位置'''
        for index in range(len(rule)):  # 根据 rule 设置阵法的位置
            pos = rule[index]
            character = self.members[index]
            self.matrixSetting[character.baseInfo.id] = pos
        
    def getCharacterEyeNo(self,characterId,characterType = 2):
        '''获取角色在阵法中的位置'''
        eyeNo = self.matrixSetting.get(characterId)
        return eyeNo
    
    def getMembers(self):
        '''获取战斗方成员信息'''
        fighters = []
        for member in self.members:
            data = member.getFightData()  # 获取怪物战斗数据
            fighters.append(data)
        return fighters
    
    def getLord(self):
        """获取主将的ID
        """
        return self.lord
        
        
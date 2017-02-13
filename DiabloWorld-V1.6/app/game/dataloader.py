#coding:utf8
'''
Created on 2013-8-14

@author: lan (www.9miao.com)
'''
# 导入数据库操作类
from app.share.dbopear import dbItems,dbMonster,dbExperience,dbSkill,\
dbProfession,dbZhanyi,dbDropout,dbShieldWord,dbCharacterPet
import memmode
from firefly.dbentrust.madminanager import MAdminManager

def load_config_data():
    """从数据库中读取配置信息
    """
    # 所有的物品模板信息
    dbItems.getAll_ItemTemplate()
    # 所有的套装信息
    dbItems.getAllsetInfo()
    # 所有怪物的信息
    dbMonster.getAllMonsterInfo()
    # 经验配置表
    dbExperience.getExperience_Config()

    dbSkill.getAllSkill()  # 初始化技能信息
    dbSkill.getBuffAddition()  # 获取buff对技能的加成
    dbSkill.getBuffOffsetInfo()  # 获取所有buff之间效果的信息配置
    dbSkill.getAllBuffInfo()  # 获取所有技能的信息

    # 获取职业配置表信息
    dbProfession.getProfession_Config()

    dbZhanyi.getAllZhangJieInfo()  # 获取章节的信息
    dbZhanyi.getAllZhanYiInfo()  # 获取所有战役的信息

    # 获取所有掉落信息
    dbDropout.getAll()
    # 获取所有屏蔽词
    dbShieldWord.getAll_ShieldWord()

    dbCharacterPet.getAllPetGrowthConfig()  # 获取宠物成长配置
    dbCharacterPet.getAllPetTemplate()  # 获取宠物的模板信息
    dbCharacterPet.getPetExp()  # 获取宠物的经验表
    dbCharacterPet.getPetTrainConfig()  # 获取宠物培养配置信息

    
def registe_madmin():
    """注册数据库与memcached对应
    """
    MAdminManager().registe( memmode.tb_character_admin)  # tb_character
    MAdminManager().registe( memmode.tb_zhanyi_record_admin)  # tb_zhanyi_record
    MAdminManager().registe( memmode.tbitemadmin)  # tb_item
    MAdminManager().registe(memmode.tb_matrix_amin)  # tb_character_matrix
    MAdminManager().registe(memmode.tbpetadmin)  # 少了 pet，补回来
    
    
    
    
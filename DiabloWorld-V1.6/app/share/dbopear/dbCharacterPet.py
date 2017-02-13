#coding:utf8
'''
Created on 2011-12-14

@author: lan (www.9miao.com)
'''

from firefly.dbentrust.dbpool import dbpool
from MySQLdb.cursors import DictCursor

PET_TRAIN_CONFIG = {}
PET_TEMPLATE = {}#宠物模板表
PET_TYPE = {}
PET_EXP = {}
PET_GROWTH = {}

#shopAll1=[]#灵兽商店50以下所有宠物
shopAll={1:[],2:[],3:[],4:[]}#    1高级宠物  2中级宠物  3低级宠物 根据宠物颜色来
shopXy=[]#50以内 幸运领取的宠物

##shopAll2=[]#幻兽商店50-70
#shopAll2={1:[],2:[],3:[]}#幻兽商店50-70   1高级宠物  2中级宠物  3低级宠物
#shopXy2=[]#50-70 幸运领取的宠物
#
##shopAll3=[]#圣兽商店70以上
#shopAll3={1:[],2:[],3:[]}#圣兽商店70以上      1高级宠物  2中级宠物  3低级宠物
#shopXy3=[]#70以上 幸运领取的宠物

def getPetExp():
    '''获取宠物的经验表
    '''
    global PET_EXP
    sql = "SELECT * FROM tb_pet_experience"
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    for exp in result:  # 经验表写入 PET_EXP
        PET_EXP[exp['level']] = exp['ExpRequired']
        
def getAllPetGrowthConfig():
    '''获取宠物成长配置
    '''
    global PET_GROWTH
    sql = "SELECT * FROM tb_pet_growth"
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    for growthconfig in result:  # 成长配置写入 PET_GROWTH
        attrType = growthconfig['pettype']  # 类型
        quality = growthconfig['quality']  # 品质
        if not PET_GROWTH.has_key(attrType):
            PET_GROWTH[attrType] = {}
        PET_GROWTH[attrType][quality] = growthconfig  # 这里是个二维字典，不同类型 不同品质 对应 不同配置

def getAllPetTemplate():
    '''获取宠物的模板信息'''
    global PET_TEMPLATE,shopAll,shopXy,PET_TYPE
    sql = "SELECT * FROM tb_pet_template ORDER BY `level` , `id`;"
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    for pet in result:  # 注意这里是数据是 固定 的！是由策划决定的
        attrType = pet['attrType']
        if not PET_TYPE.has_key(attrType):
            PET_TYPE[attrType] = []
        PET_TYPE[attrType].append(pet['id'])  # 将宠物id 放入对应 宠物类型的列表中
        PET_TEMPLATE[pet['id']] = pet  # 不同id对应的宠物模板

        if pet['coin']>0:
            zi=pet['baseQuality']  # 根据价格决定品级
            shopAll[zi].append(pet['id'])  # 将宠物id 放入对应 商店列表中 1高级宠物  2中级宠物  3低级宠物 根据宠物颜色来
        if pet['xy']>0:  # 幸运值
            shopXy.append(pet)  # 可以根据幸运领取的宠物
            

def getPetTrainConfig():
    '''获取宠物培养配置信息'''
    global PET_TRAIN_CONFIG
    sql = "SELECT * FROM tb_pet_training "
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    for train in result:  # 根据品级 将配置信息 放入 培养配置表 中
        PET_TRAIN_CONFIG[train['quality']] = train
        # train['quality'] = 1 2 3 4 5 ....
        # 配置表 quality品质     down_1培养下限1     up_1培养上限1       down_2下限2 up_2上限2 ...
    return result
    
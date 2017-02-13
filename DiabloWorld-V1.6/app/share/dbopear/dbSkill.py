#coding:utf8
'''
Created on 2013-8-14

@author: lan (www.9miao.com)
'''
from firefly.dbentrust.dbpool import dbpool
from MySQLdb.cursors import DictCursor

ALL_SKILL_INFO = {}  # 技能池 一维字典
SKILL_GROUP = {}  # 技能组  二维字典
ALL_BUFF_INFO = {}  # 所有buff技能池  二维字典
PROFESSION_SKILLGROUP = {}  # 职业技能组  一维字典，值为列表

#buff和buff直接的效果配置
BUFF_BUFF = {}
#buff对技能加成配置表
BUFF_SKILL = {}

def getBuffOffsetInfo():
    '''获取所有buff之间效果的信息配置
    buffId  buff
    tbuffId  能与之产生效果的buff
    nbuffId  新产生的buff
    nstack  新产生的buff的层叠数
    effect  产生效果后生产的效果
    '''
    global BUFF_BUFF
    sql = "SELECT * FROM tb_buff_buff"
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result=cursor.fetchall()
    cursor.close()
    conn.close()
    for offset in result:
        if not BUFF_BUFF.has_key(offset['buffId']):  # 还没有key
            BUFF_BUFF[offset['buffId']] = {}  # 初始化一个
        BUFF_BUFF[offset['buffId']][offset['tbuffId']] = offset  # 二维字典  value是 一个列表
    
def getBuffAddition():
    '''获取buff对技能的加成
    buffId  buff
    skillId  skill
    addition  效果的加成
    '''
    global BUFF_SKILL
    sql = "SELECT * FROM tb_buff_skill"
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result=cursor.fetchall()
    cursor.close()
    conn.close()
    for addition in result:
        if not BUFF_SKILL.has_key(addition['buffId']):  # 还没有key
            BUFF_SKILL[addition['buffId']] = {}  # 初始化一个
        BUFF_SKILL[addition['buffId']][addition['skillId']] = addition['addition']  # 二维字典  value是 一个值

def getSkillEffectByID(skillEffectID):
    '''获取技能效果ID'''
    sql = "SELECT * FROM tb_skill_effect where effectId=%d"%skillEffectID
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result=cursor.fetchone()
    cursor.close()
    conn.close()
    return result

def getAllSkill():
    '''初始化技能信息
    #技能池
    #技能组
    #职业技能组
    '''
    global  ALL_SKILL_INFO,SKILL_GROUP,PROFESSION_SKILLGROUP
    sql = "SELECT * FROM tb_skill_info"
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    for skill in result:
        effectInfo = getSkillEffectByID(skill['effect'])  # 调用上面的函数
        skill['effect'] = effectInfo
        ALL_SKILL_INFO[skill['skillId']] = skill  # 加入技能池
        if not SKILL_GROUP.has_key(skill['skillGroup']):  # 还没有技能组
            SKILL_GROUP[skill['skillGroup']] = {}
        SKILL_GROUP[skill['skillGroup']][skill['level']] = skill  # 加入技能组  二维字典（skillGroup技能组id，level技能等级）
    #初始化职业技能组ID
    for groupID in SKILL_GROUP:
        skillInfo = SKILL_GROUP[groupID].get(1)  # 获得等级为1的技能
        profession = skillInfo.get('profession',0)  # 获得该技能对应的profession职业，默认为0
        if not PROFESSION_SKILLGROUP.has_key(profession):  # 还没有职业技能组
            PROFESSION_SKILLGROUP[profession] = []
        PROFESSION_SKILLGROUP[profession].append(groupID)  # 加入职业技能组 一维字典，值为列表（根据profession 获得 技能列表）
        
def getBuffEffect(buffEffectID):
    '''获取buff效果'''
    sql = "SELECT * FROM tb_buff_effect where buffEffectID = %d"%buffEffectID
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result=cursor.fetchone()
    cursor.close()
    conn.close()
    return result
        
def getAllBuffInfo():
    '''获取所有技能的信息'''
    global ALL_BUFF_INFO
    sql = "SELECT * FROM tb_buff_info"
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result=cursor.fetchall()
    cursor.close()
    conn.close()
    for buff in result:
        ALL_BUFF_INFO[buff['buffId']] = buff  # 根据buff技能信息id 获得 buff技能信息
        effectInfo = getBuffEffect(buff['buffEffectID'])  # 根据buff效果id 获得 buff效果
        ALL_BUFF_INFO[buff['buffId']]['buffEffects'] = effectInfo # 二维字典为 buffId + 常量'buffEffects' 获得 buff效果


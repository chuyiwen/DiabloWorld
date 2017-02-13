#coding:utf8
'''
Created on 2012-3-1

@author: sean_lan
'''
from firefly.dbentrust.dbpool import dbpool
from MySQLdb.cursors import DictCursor
import datetime


def getUserInfo(uid):
    '''获取用户角色关系表所有信息
    @param id: int 用户的id
    '''
    sql = "select * from tb_user_character where id = %d"%(uid)
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    return result

def checkUserPassword(username,password):
    '''检测用户名户密码
    @param username: str 用户的用户名
    @param password: str 用户密码
    '''
    sql = "select id from `tb_register` where username = '%s' and password = '%s'" %( username, password)
    conn = dbpool.connection()
    cursor = conn.cursor()
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    pid = 0  # 默认未0，表示未查找到
    if result:
        pid = result[0]
    return pid

def getUserInfoByUsername(username,password):
    '''获取用户信息
    @param username: str 用户的用户名
    @param password: str 用户密码
    '''
    # id（用户id）, username, password, email, characterId（用户的角色id）, pid（邀请人的角色id）,
    # lastonline（最后在线时间）, logintimes（登陆次数）, enable（是否可以登录）
    sql = "select * from `tb_register` where username = '%s'\
     and password = '%s'" %( username, password)
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    return result

def creatUserCharacter(uid):
    '''为新用户建立空的用户角色关系记录
    @param id: int 用户id
    '''
    sql = "insert into `tb_user_character` (`id`) values(%d)" %uid
    conn = dbpool.connection()
    cursor = conn.cursor()
    count = cursor.execute(sql)
    conn.commit()
    cursor.close()
    conn.close()
    if count >= 1:
        return True
    else:
        return False

def updateUserCharacter(userId ,fieldname ,characterId):
    '''更新用户角色关系表
    @param userId: 用户的id
    @param fieldname: str 用户角色关系表中的字段名，表示用户的第几个角色
    @param characterId: int 角色的id
    '''
    # id, [character_1, character_2， character_3, character_4, character_5]五个字段，应该是最多可以创建5个角色
    # pid（邀请人id）, last_character（最后登录的角色）
    sql = "update `tb_user_character` set %s = %d where id = %d"%(fieldname ,characterId ,userId)
    conn = dbpool.connection()
    cursor = conn.cursor()
    count = cursor.execute(sql)
    conn.commit()
    cursor.close()
    conn.close()
    if count >= 1:
        return True
    else:
        return False
    
def InsertUserCharacter(userId,characterId):
    '''加入角色用户关系'''
    # userId 是账号唯一的， characterId 同一账号可以存在多个
    sql = "update tb_register set characterId = %d where `id` = %d"%( characterId ,userId)
    conn = dbpool.connection()
    cursor = conn.cursor()
    count = cursor.execute(sql)
    conn.commit()
    cursor.close()
    conn.close()
    if count >= 1:  # 成功加入
        return True
    else:
        return False

def checkCharacterName(nickname):
    '''检测角色名是否可用
    @param nickname: str 角色的名称
    '''
    sql = "SELECT `id` from tb_character where nickname = '%s'"%nickname
    conn = dbpool.connection()
    cursor = conn.cursor()
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    if result:  # 被使用过了
        return False
    return True

# 账号只有一个，但是同一账号下可以有多个角色
def creatNewCharacter(nickname ,profession ,userId,sex=1):
    '''创建新的角色
    @param nickname: str 角色的昵称
    @param profession: int 角色的职业编号
    @param userId: int 用户的id
    @param fieldname: str 用户角色关系表中的字段名，表示用户的第几个角色
    '''
    # userId 用户id 从 10000开始计起
    nowdatetime = str(datetime.datetime.today())
    sql = "insert into `tb_character`(nickName,profession,sex,createtime) \
    values('%s',%d,%d,'%s')"%(nickname ,profession,sex,nowdatetime)
    sql2 = "SELECT @@IDENTITY"  # 用select @@identity得到上一次插入记录时自动产生的ID（自增列）
    conn = dbpool.connection()
    cursor = conn.cursor()
    count = cursor.execute(sql)  # 插入，返回成功数量
    conn.commit()
    cursor.execute(sql2)
    result = cursor.fetchone()  # 获取上一次插入的那一条记录
    cursor.close()
    conn.close()
    if result and count:
        characterId = result[0]  # result[0] 是 id
        InsertUserCharacter(userId,characterId)
        return characterId
    else:
        return 0
    
    
def getUserCharacterInfo(characterId):
    '''获取用户角色列表的所需信息
    @param id: int 用户的id
    '''
    # town 角色所在的场景id
    sql = "select town from tb_character where id = %d"%(characterId)
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    return result

def CheckUserInfo(Uid):
    '''检测用户信息'''
    sql = "SELECT * from tb_register where username = '%s'"%Uid
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    return result

def creatUserInfo(username,password):
    '''创建用户信息
    '''
    sql = "insert into tb_register(username,`password`) values ('%s','%s')"%(username,password)
    conn = dbpool.connection()
    cursor = conn.cursor()
    count = cursor.execute(sql)
    conn.commit()
    cursor.close()
    conn.close()
    if(count >= 1):
        return True
    return False

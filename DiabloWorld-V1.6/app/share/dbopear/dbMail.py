#coding:utf8
'''
Created on 2011-8-8

@author: lan (www.9miao.com)
'''
from firefly.dbentrust.dbpool import dbpool
from MySQLdb.cursors import DictCursor
from twisted.python import log
from firefly.dbentrust import util

LEVEL_MAIL = {}#所有等级的邮件提示


def forEachQueryProps(sqlstr, props):
    '''遍历所要查询属性，以生成sql语句，在本类中使用'''
    if props == '*':  # 参数为 *
        sqlstr += ' *'
    elif type(props) == type([0]):  # 是个列表
        i = 0
        for prop in props:  # 组拼 sql 语句
            if(i == 0):
                sqlstr += ' ' + prop
            else:
                sqlstr += ', ' + prop
            i += 1
    else:
        log.msg('props to query must be list')
        return
    return sqlstr

def getAllLevelMail():
    '''获取所有的等级邮件提示
    '''
    global  LEVEL_MAIL
    sql="SELECT * FROM tb_levelmail"
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    scenesInfo = {}
    for scene in result:
        scenesInfo[scene['level']] = scene
    LEVEL_MAIL = scenesInfo  # 写入
    return scenesInfo

# 根据 类型码 返回对应类型的数量
def getPlayerMailCnd(characterId,mtype):
    '''获取角色邮件列表长度
    @param characterId: int 角色的ID
    @param type: int 邮件的分页类型
    '''
    cnd = 0
    if mtype ==0:  # 所有
        cnd = getPlayerAllMailCnd(characterId)
    elif mtype ==1:  # 系统
        cnd = getPlayerSysMailCnd(characterId)
    elif mtype ==2:  # 好友
        cnd = getPlayerFriMailCnd(characterId)
    elif mtype ==3:  # 保存
        cnd = getPlayerSavMailCnd(characterId)
    return cnd
    
def getPlayerAllMailCnd(characterId):
    '''获取玩家所有邮件的数量'''
    # isSaved = 0
    sql = "SELECT COUNT(`id`) FROM tb_mail WHERE receiverId = %d and isSaved = 0"%characterId
    conn = dbpool.connection()
    cursor = conn.cursor()
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    return result[0]

def getPlayerFriMailCnd(characterId):
    '''获取角色好友邮件的数量'''
    # type = 1 好友
    sql = "SELECT COUNT(id) FROM tb_mail WHERE receiverId = %d AND `type`=1  and isSaved = 0"%characterId
    conn = dbpool.connection()
    cursor = conn.cursor()
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    return result[0]

def getPlayerSysMailCnd(characterId):
    '''获取角色系统邮件数量'''
    # type = 0 系统
    sql = "SELECT COUNT(id) FROM tb_mail WHERE receiverId = %d AND `type`=0  and isSaved = 0"%characterId
    conn = dbpool.connection()
    cursor = conn.cursor()
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    return result[0]

def getPlayerSavMailCnd(characterId):
    '''获取已保存邮件的数量'''
    # isSaved = 1
    sql = "SELECT COUNT(id) FROM tb_mail WHERE receiverId = %d AND `isSaved`=1"%characterId
    conn = dbpool.connection()
    cursor = conn.cursor()
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    return result[0]

def getPlayerMailList(characterId):
    '''获取角色邮件列表'''
    data = getPlayerAllMailList(characterId)
    return data
    

def getPlayerAllMailList(characterId):
    '''获取角色所有邮件列表
    @param characterId: int 角色的id
    '''
    filedList = ['id','sender','title','type','isReaded','sendTime'] # 多了sender，少了content
    sqlstr = ''
    sqlstr = forEachQueryProps(sqlstr, filedList)  # 返回一个不完整 sql 语句
    # 按照降序（未读、最近时间）排序
    # isSaved = 0 表示未接收，可能是因为该玩家未上线
    sql = "select %s from `tb_mail` where receiverId = %d  and isSaved = 0\
     order by isReaded ,sendTime desc"%(sqlstr,characterId)
    conn = dbpool.connection()
    cursor = conn.cursor()
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    data = []  # 邮件列表
    for mail in result:  # 话说这里为什么不用 extend 呢？直接就可以全部放入了呀！
        mailInfo = {}
        for i in range(len(mail)):
            if filedList[i]=='sendTime':
                mailInfo['sendTime'] = str(mail[i])  # 为什么？？？这里 sendTime 有点不同，转换成string类型
            else:
                mailInfo[filedList[i]] = mail[i]  # 其他属性正常
        data.append(mailInfo)
    return data

def getPlayerSysMailList(characterId,page,limit):
    '''获取系统邮件列表
    @param characterId: int 角色的id
    '''
    filedList = ['id','title','type','isReaded','sendTime','content'] # 多了content，少了sender
    sqlstr = ''
    sqlstr = forEachQueryProps(sqlstr, filedList)
    # type = 0 表示 系统
    # isSaved = 0 表示 未接收
    sql = "select %s from `tb_mail` where receiverId = %d and `type`=0  and isSaved = 0\
     order by isReaded,sendTime desc LIMIT %d,%d "%(sqlstr,characterId,(page-1)*limit,limit)
    conn = dbpool.connection()
    cursor = conn.cursor()
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    data = []
    for mail in result:
        mailInfo = {}
        for i in range(len(mail)):
            mailInfo[filedList[i]] = mail[i]
        data.append(mailInfo)
    return data

def getPlayerFriMailList(characterId,page,limit):
    '''获取好友邮件列表
    @param characterId: int 角色的id
    '''
    filedList = ['id','title','type','isReaded','sendTime','content']
    sqlstr = ''
    sqlstr = forEachQueryProps(sqlstr, filedList)
    # type = 1 表示 好友
    # isSaved = 0 表示 未接收
    sql = "select %s from `tb_mail` where receiverId = %d and `type`=1  and isSaved = 0\
     order by isReaded LIMIT %d,%d "%(sqlstr,characterId,(page-1)*limit,limit)
    conn = dbpool.connection()
    cursor = conn.cursor()
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    data = []
    for mail in result:
        mailInfo = {}
        for i in range(len(mail)):
            mailInfo[filedList[i]] = mail[i]
        data.append(mailInfo)
    return data

def getPlayerSavMailList(characterId,page,limit):
    '''获取已保存的邮件列表
    @param characterId: int 角色的id
    '''
    filedList = ['id','title','type','isReaded','sendTime','content']
    sqlstr = ''
    sqlstr = forEachQueryProps(sqlstr, filedList)
    # isSaved = 1 表示 已接收
    # page 页数 根据页数获取已保存的邮件 offset = (page-1) * rows   rows = limit
    sql = "select %s from `tb_mail` where receiverId = %d and `isSaved`=1\
     order by isReaded LIMIT %d,%d "%(sqlstr,characterId,(page-1)*limit,limit)
    conn = dbpool.connection()
    cursor = conn.cursor()
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    data = []
    for mail in result:
        mailInfo = {}
        for i in range(len(mail)):
            mailInfo[filedList[i]] = mail[i]
        data.append(mailInfo)
    return data

def checkMail(mailId,characterId):
    '''检测邮件是否属于characterId
    @param characterId: int 角色的ID
    @param mailId: int 邮件的ID
    '''
    sql = "SELECT `id` FROM tb_mail WHERE id = %d AND receiverId=%d"%(mailId,characterId)
    conn = dbpool.connection()
    cursor = conn.cursor()
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    if result:  # 是
        return True
    return False  # 不是

def getMailInfo(mailId):
    '''获取邮件详细信息'''
    sql = "select * from `tb_mail` where id = %d"%(mailId)
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    return result
    
def updateMailInfo(mailId,prop):
    '''更新邮件信息'''
    # 传来了prop，prop包含需要更新的键值
    # 在 util.forEachUpdateProps 会格式化 sql 语句
    # {'id':mailId} 是 用于 where 判断
    sql = util.forEachUpdateProps('tb_mail',prop, {'id':mailId})
    conn = dbpool.connection()
    cursor = conn.cursor()
    count = cursor.execute(sql)
    conn.commit()
    cursor.close()
    conn.close()
    if(count >= 1):
        return True
    return False
    
def addMail(title,senderId,sender,receiverId,content,mailtype):
    '''添加邮件'''
    sql = "INSERT INTO tb_mail(title,senderId,sender,receiverId,\
    `type`,content,sendTime) VALUES ('%s',%d,'%s',%d,%d,'%s',\
    CURRENT_TIMESTAMP())"%(title,senderId,sender,receiverId,mailtype,content)
    conn = dbpool.connection()
    cursor = conn.cursor()
    count = cursor.execute(sql)
    conn.commit()
    cursor.close()
    conn.close()
    if(count >= 1):
        return True
    return False
    
def deleteMail(mailId):
    '''删除邮件'''
    sql = "DELETE FROM tb_mail WHERE id = %d"%mailId
    conn = dbpool.connection()
    cursor = conn.cursor()
    count = cursor.execute(sql)
    conn.commit()
    cursor.close()
    conn.close()
    if(count >= 1):
        return True
    return False
    

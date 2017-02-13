#coding:utf8
'''
Created on 2013-8-14

@author: lan (www.9miao.com)
'''
from firefly.dbentrust.dbpool import dbpool
from MySQLdb.cursors import DictCursor

# 这里和前面的几个类似，都是基本的数据库操作。

tb_Experience_config = {}
VIPEXP = {}

def getExperience_Config():
    '''获取经验配置表信息'''
    global tb_Experience_config
    sql = "select * from tb_experience"
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    for _item in result:
        tb_Experience_config[_item['level']] = _item
        
def getVIPExp():
    '''VIP升级配置表
    @param viplevel: int VIP等级  0 - 10
    @param maxexp: int 升级VIP最大经验  100 500 1000 2000 5000...
    '''
    global VIPEXP
    sql = "SELECT * FROM tb_vipexp"
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result=cursor.fetchall()
    cursor.close()
    conn.close()
    for vipp in result:
        VIPEXP[vipp['viplevel']] = vipp['maxexp']
        
        
        




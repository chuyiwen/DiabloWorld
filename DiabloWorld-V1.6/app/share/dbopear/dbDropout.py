#coding:utf8
'''
Created on 2013-8-15

@author: lan (www.9miao.com)
'''
from firefly.dbentrust.dbpool import dbpool
from MySQLdb.cursors import DictCursor
from app.game.core.Item import Item  # 这里导入了 Item，用于后面实例化物品
from twisted.python import log
import random

DROPOUT_CONFIG = {}
BASERATE=100000 #几率的基数

def getAll():
    '''获取所有掉落信息'''
    global DROPOUT_CONFIG
    sql="select * from tb_dropout"
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result=cursor.fetchall()
    cursor.close()
    conn.close()
    if not result:
        return None
    for item in result:
        # id 表示 怪物的id， itemid 表示对应怪物掉落物品的几种可能
        # id = 1 => itemid = [53000001,1,99999],[53000002,1,99999],[53000003,1,99999],[54000006,1,99999],[54000007,1,99999],[54000008,1,99999],[10150001,1,99999]
        # 上面为第一列itemid的数据，表示 [物品id， 数量， 概率（基数100000）]
        item['itemid']=eval("["+item['itemid']+"]")  # eval 可以执行一条语句，将上面几个列表合并
        DROPOUT_CONFIG[item['id']]=item
        
        
def getDropByid(did):
    '''根据怪物id获取掉落物品信息 (适用于 怪物掉落 返回一个掉落物品)
    @param did: int 怪物掉落表主键id
    '''
    data=DROPOUT_CONFIG.get(did,None)  # 获取did的值，不存在则返回None
    if not data:
        log.err(u'掉落表填写错误不存在掉落信息-掉落主键:%d'%did)
        return None
    for item in data.get('itemid'):  # FIXME 这里的 for 语句，意思是随机找到第一个合适的爆落物品
        # FIXME 我觉得应该一次可能爆落几个装备，所以可以将实例化的物品放到一个列表，再返回。
        abss=random.randint(1,BASERATE)  # 随机获取1 - 100000 之间的值
        if abss>=1 and abss<=item[2]:  # 如果随机出来此物品 item[2]是概率
            abss=random.randint(1,item[1])  # item[1]是物品数量
            item1=Item(item[0])  # item[0]是物品id，根据物品id实例化
            item1.pack.setStack(abss)  # 打包
            return item1
    return None


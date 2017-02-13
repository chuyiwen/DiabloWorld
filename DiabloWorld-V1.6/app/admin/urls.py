#-*-coding:utf8-*-

from MySQLdb.cursors import DictCursor
from firefly.dbentrust.dbpool import dbpool


def getIncomeByDate(onedate):
    """获取某天的缴费情况
    """
    # id, uid, rbm人民币, zuan充值钻, serviceid服务器id, lyid联运商id,
    # rtime充值时间, orderid订单id, boo充值是否成功
    # SUM(rbm)  人民币总数
    # COUNT(DISTINCT uid)  充值次数
    sql = "SELECT SUM(rbm) AS goal,COUNT(DISTINCT uid) AS cnt\
    FROM tb_recharge WHERE DATE(rtime)=DATE('%s') and boo=1;"%onedate
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchone()
    cursor.close()
    conn.close()
    result['goal'] = float(result.get('goal')) if result.get('goal') else 0
    return result


def getDayConsume(onedate):
    """获取某天的消费情况
    """
    # id, characterId角色id, spendType消费类型, speedGold消费钻的数量,
    # itemId关联的物品id, spendDetails消费明细, recordData消费时间
    sql = "SELECT * FROM tb_bill WHERE DATE(recordDate)=DATE('%s');"%onedate
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    info = {}
    info['cons_goal'] = sum([record['spendGold'] for record in result])  # 花费钻的总数
    info['user_cnt'] = len(set([record['characterId'] for record in result]))  # 去重的充值人数
    return info


def getDayRecordList(index,limit = 10):
    """获取每日的记录
    """
    # index为页数， limit为一页的数量，每一项为每一天的记录
    # id, recorddata记录日期, high当天最高在线人数, createrole当天创建的角色数量,
    # loginuser当天登陆过的账号数量, onlineaverage平均在线时间, onlinetotal总在线时间
    sql ="SELECT * FROM tb_statistics ORDER BY recorddate DESC LIMIT %s,%s;"%((index-1)*limit,index*limit)
    conn = dbpool.connection()
    cursor = conn.cursor(cursorclass=DictCursor)
    cursor.execute(sql)
    result = cursor.fetchall()
    cursor.close()
    conn.close()
    recordlist = []
    for daterecord in result:
        recorddate = daterecord['recorddate']  # 日期
        IncomeInfo = getIncomeByDate(recorddate)
        info = daterecord

        # 平均每次充值人民币数量
        info['f_arpu'] = 0 if not IncomeInfo['cnt'] else IncomeInfo['goal']/IncomeInfo['cnt']
        # 平均每个角色充值人民币数量
        info['z_arpu'] = 0 if not info['createrole'] else IncomeInfo['goal']/info['createrole']
        # 平均每个账号充值次数
        info['pay_rate'] = 0 if not info['loginuser'] else IncomeInfo['cnt']*100/info['loginuser']
        # 平均每个账号创建新角色数量
        info['r_rate'] = 0 if not info['createrole'] else info['loginuser']*100/info['createrole']
        # 记录日期
        info['recorddate'] = str(info['recorddate'])
        # cnt 充值次数
        info['pay_cnt'] = IncomeInfo['cnt']
        # goal 人民币总数
        info['pay_goal'] = IncomeInfo['goal']
        # 加上 cons_goal 花费钻的总数  user_cnt 去重的充值人数
        info.update(getDayConsume(recorddate))
        # 加入列表
        recordlist.append(info)
    return recordlist

def getStatistics():
    """
    """
    sql1 ="SELECT COUNT(id) FROM tb_register;"#总注册数
    sql2 ="SELECT COUNT(id) FROM tb_character;"#总创建数
    sql3 ="SELECT COUNT(DISTINCT uid) FROM tb_recharge WHERE boo=1;"#付费人数
    sql4 ="SELECT SUM(rbm) FROM tb_recharge WHERE boo=1;"#总付费人数
    conn = dbpool.connection()
    cursor = conn.cursor()
    cursor.execute(sql1)
    result1 = cursor.fetchone()[0]
    cursor.execute(sql2)
    result2 = cursor.fetchone()[0]
    cursor.execute(sql3)
    result3 = cursor.fetchone()[0]
    cursor.execute(sql4)
    result4 = cursor.fetchone()[0]
    cursor.close()
    conn.close()
    return {'reg_cnt':0 if not result1 else result1,#总注册数
            'role_cnt':0 if not result2 else result2,#总创建数
            'fu_cnt':0 if not result3 else result3,#付费人数
            'income':0.0 if not result4 else float(result4)}#总付费人数
#-*-coding:utf8-*-
'''
Created on 2013-6-5

@author: lan (www.9miao.com)
'''
from firefly.dbentrust.mmode import MAdmin


'''
mmode管理器，可以根据主键获取一个mmode的实例。
一个MAdmin管理器对应到的是数据库的某张表，继承与MemObject类。
它的实例化方式 MAdmin("表名",'id',fk = 'group 外键',incrkey='id 自增列')
insert 将所有数据写入到memcached。
load 将本管理器对应的数据库中的表的所有信息，写入到memcached中
'''

tbitemadmin = MAdmin('tb_item','id',fk ='characterId',incrkey='id')
tbitemadmin.insert()

tb_character_admin = MAdmin('tb_character','id',incrkey='id')
tb_character_admin.insert()

tb_zhanyi_record_admin = MAdmin('tb_zhanyi_record','id',fk ='characterId',incrkey='id')
tb_zhanyi_record_admin.insert()

tb_matrix_amin = MAdmin('tb_character_matrix','characterId',incrkey='id')
tb_matrix_amin.insert()

tb_equipment = MAdmin('tb_equipment','characterId',incrkey='id')
tb_equipment.insert()

tbpetadmin = MAdmin('tb_pet','id',fk ='ownerID',incrkey='id')
tbpetadmin.insert()


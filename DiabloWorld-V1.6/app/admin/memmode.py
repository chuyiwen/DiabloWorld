#coding:utf8
'''
Created on 2013年9月4日

@author: MSI
'''
from firefly.dbentrust.mmode import MAdmin

# 从数据库中读取tb_register表中的信息，key为第二个参数，即 username，value为对应的信息
register_admin = MAdmin('tb_register','username')
# 将数据写入到memcached中
register_admin.insert()

#coding:utf8
'''
Created on 2013-8-6

@author: lan (www.9miao.com)
'''
import json,sys
from firefly.server.server import FFServer

if __name__=="__main__":
    args = sys.argv
    servername = None
    config = None
    if len(args)>2:
        servername = args[1]  # servername 服务名
        config = json.load(open(args[2],'r'))  # 加载 config
    else:
        raise ValueError
    dbconf = config.get('db')
    memconf = config.get('memcached')
    sersconf = config.get('servers',{})  # server
    masterconf = config.get('master',{})  # master
    serconfig = sersconf.get(servername)
    ser = FFServer()
    ser.config(serconfig, dbconfig=dbconf, memconfig=memconf,masterconf=masterconf)
    ser.start()
    
    
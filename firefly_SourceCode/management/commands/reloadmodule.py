#coding:utf8
'''
Created on 2013-8-12

@author: lan (www.9miao.com)
'''
import urllib,sys

def execute(*args):
    """
    """
    if not args:  # 没参数 默认master port 为9998
        masterport =9998
    else:
        masterport = int(args[0])
    url = "http://localhost:%s/reloadmodule"%masterport  # 利用网页预加载模块
    try:
        response = urllib.urlopen(url)
    except:
        response = None
    if response:
        sys.stdout.write("reload module success \n")
    else:
        sys.stdout.write("reload module failed \n")
#-*-coding:utf8-*-
'''
Created on 2013-4-27

@author: lan
'''
from firefly.dbentrust.memobject import MemObject

class Mcharacter(MemObject):
    
    def __init__(self,pid,name,mc):
        '''初始化对象
        '''
        MemObject.__init__(self, name, mc)
        self.id = pid
        self.level = 0
        self.profession = 0
        self.nickname = u''
        self.guanqia = 1000
        
    def initData(self,data):
        '''初始化数据
        '''
        for keyname in self.__dict__.keys():
            if not keyname.startswith('_'):
                setattr(self,keyname,data.get(keyname))  # 设置 keyname = data.get(keyname)

    # 意思是可以把这个 方法 当成 属性 用
    @property
    def mcharacterinfo(self):
        keys = [ key for key in self.__dict__.keys() if not key.startswith('_')]
        info = self.get_multi(keys)  # 一次获取多个key的值
        return info  # 返回角色信息
        
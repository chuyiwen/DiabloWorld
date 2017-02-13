#coding:utf8
'''
Created on 2013-8-14

@author: lan (www.9miao.com)
'''
from firefly.utils.services import CommandService
from twisted.python import log
from twisted.internet import defer


# CommandService 服务类
class LocalService(CommandService):
    
    def callTargetSingle(self,targetKey,*args,**kw):
        '''call Target by Single
        @param conn: client connection
        @param targetKey: target ID
        @param data: client data
        '''
        
        self._lock.acquire()  # 上锁
        try:
            target = self.getTarget(targetKey)
            if not target:  # 没有获取目标
                log.err('the command '+str(targetKey)+' not Found on service')
                return None
            if targetKey not in self.unDisplay:  # 可以获取目标
                log.msg("call method %s on service[single]"%target.__name__)
            defer_data = target(targetKey,*args,**kw)  # 从字典获取值
            if not defer_data:  # 没数据
                return None
            if isinstance(defer_data,defer.Deferred):  # 同类型
                return defer_data
            '''
            Deferred:
            提供了让程序查找非同步任务完成的一种方式，而在这时还可以做其他事情。
            当函数返回一个Deferred对象时，说明获得结果之前还需要一定时间。
            为了在任务完成时获得结果，可以为Deferred指定一个事件处理器
            '''
            d = defer.Deferred()
            d.callback(defer_data)  # 在结果出来后调用一个方法。
        finally:
            self._lock.release()  # 解锁
        return d

localservice = LocalService('localservice')

def localserviceHandle(target):
    '''服务处理
    @param target: func Object
    '''
    localservice.mapTarget(target)

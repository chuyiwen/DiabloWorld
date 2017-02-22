#coding:utf8
'''
Created on 2011-1-3
服务类
@author: sean_lan
'''
import threading
from twisted.internet import defer,threads
from twisted.python import log


class Service(object):
    """A remoting service 
    
    attributes:
    ============
     * name - string, service name.
     * runstyle 
    """
    SINGLE_STYLE = 1  # 单行
    PARALLEL_STYLE = 2  # 并行

    def __init__(self, name,runstyle = SINGLE_STYLE):
        self._name = name
        self._runstyle = runstyle
        self.unDisplay = set()
        self._lock = threading.RLock()
        self._targets = {} # Keeps track of targets internally

    def __iter__(self):
        return self._targets.itervalues()
    
    def addUnDisplayTarget(self,command):
        '''Add a target unDisplay when client call it.'''
        self.unDisplay.add(command)

    def mapTarget(self, target):
        """Add a target to the service."""
        self._lock.acquire()
        try:
            key = target.__name__  # 函数名
            if self._targets.has_key(key):  # 重复映射
                exist_target = self._targets.get(key)
                raise "target [%d] Already exists,\
                Conflict between the %s and %s"%(key,exist_target.__name__,target.__name__)
            self._targets[key] = target  # 函数名 和 函数 的映射
        finally:
            self._lock.release()

    def unMapTarget(self, target):
        """Remove a target from the service."""
        self._lock.acquire()
        try:
            key = target.__name__
            if key in self._targets:  # 移除
                del self._targets[key]
        finally:
            self._lock.release()
            
    def unMapTargetByKey(self,targetKey):
        """Remove a target from the service."""
        self._lock.acquire()
        try:
            del self._targets[targetKey]  # 移除
        finally:
            self._lock.release()
            
    def getTarget(self, targetKey):
        """Get a target from the service by name."""
        self._lock.acquire()
        try:
            target = self._targets.get(targetKey, None)  # 根据 key 获取 函数
        finally:
            self._lock.release()
        return target
    
    def callTarget(self,targetKey,*args,**kw):
        '''call Target
        @param conn: client connection
        @param targetKey: target ID
        @param data: client data
        '''
        if self._runstyle == self.SINGLE_STYLE:
            result = self.callTargetSingle(targetKey,*args,**kw)  # 单行
        else:
            result = self.callTargetParallel(targetKey,*args,**kw)  # 并行
        return result

    # 单行
    def callTargetSingle(self,targetKey,*args,**kw):
        '''call Target by Single
        @param conn: client connection
        @param targetKey: target ID
        @param data: client data
        '''
        target = self.getTarget(targetKey)  # 取得函数
        
        self._lock.acquire()
        try:
            if not target:
                log.err('the command '+str(targetKey)+' not Found on service')
                return None
            if targetKey not in self.unDisplay:  # 不在不显示集合（可以使用）
                log.msg("call method %s on service[single]"%target.__name__)
            defer_data = target(*args,**kw)  # 执行
            if not defer_data:  # 没数据返回
                return None
            if isinstance(defer_data,defer.Deferred):  # 是 defer.Deferred 类数据
                return defer_data
            d = defer.Deferred()  # 无阻塞地等待数据，直到数据准备就绪
            d.callback(defer_data)  # 数据到达就执行回调
        finally:
            self._lock.release()
        return d

    # 并行
    def callTargetParallel(self,targetKey,*args,**kw):
        '''call Target by Single
        @param conn: client connection
        @param targetKey: target ID
        @param data: client data
        '''
        self._lock.acquire()
        try:
            target = self.getTarget(targetKey)
            if not target:
                log.err('the command '+str(targetKey)+' not Found on service')
                return None
            log.msg("call method %s on service[parallel]"%target.__name__)
            d = threads.deferToThread(target,*args,**kw)  # 使同步函数实现非阻塞 开启线程
        finally:
            self._lock.release()
        return d
    

# 通过 含commandId的名字 添加到 服务类字典
class CommandService(Service):
    """A remoting service 
    According to Command ID search target
    """
    def mapTarget(self, target):  # 加入服务类字典
        """Add a target to the service."""
        self._lock.acquire()
        try:
            key = int((target.__name__).split('_')[-1])  # 分割下划线，取最后一个
            if self._targets.has_key(key):  # 已存在
                exist_target = self._targets.get(key)
                raise "target [%d] Already exists,\
                Conflict between the %s and %s"%(key,exist_target.__name__,target.__name__)
            self._targets[key] = target  # 加入
        finally:
            self._lock.release()
            
    def unMapTarget(self, target):  # 移除
        """Remove a target from the service."""
        self._lock.acquire()
        try:
            key = int((target.__name__).split('_')[-1])  # 分割
            if key in self._targets:
                del self._targets[key]
        finally:
            self._lock.release()
    

    
    
    
            
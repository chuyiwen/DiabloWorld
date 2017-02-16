#coding:utf8
'''
Created on 2013-8-7

@author: lan (www.9miao.com)
'''
from twisted.web import resource
from twisted.internet import reactor
from firefly.server.globalobject import GlobalObject
root = GlobalObject().webroot
reactor = reactor
def ErrorBack(reason):
    pass

def masterwebHandle(cls):
    '''
    '''
    root.putChild(cls.__name__, cls())

'''
用到了装饰器，masterwebHandle 中的 cls是使用了装饰器的函数。
这里意思是 向root 添加 stop reloadmodule 作为子类。
注意这里不需要调用函数就被传入了。
'''

@masterwebHandle
class stop(resource.Resource):
    '''stop service'''
    
    def render(self, request):
        '''
        遍历子节点，通知服务停止
        '''
        for child in GlobalObject().root.childsmanager._childs.values():
            d = child.callbackChild('serverStop')
            d.addCallback(ErrorBack)
        reactor.callLater(0.5,reactor.stop)  # reactor 将在0.5秒后停止
        return "stop"

@masterwebHandle
class reloadmodule(resource.Resource):
    '''reload module'''
    
    def render(self, request):
        '''
        遍历子节点，通知重新加载模块
        '''
        for child in GlobalObject().root.childsmanager._childs.values():
            d = child.callbackChild('sreload')
            d.addCallback(ErrorBack)
        return "reload"





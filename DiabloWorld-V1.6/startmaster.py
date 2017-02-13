#coding:utf8

import os
# nt 是 windows， posix 是 *nix
if os.name!='nt' and os.name!='posix':
    from twisted.internet import epollreactor
    epollreactor.install()

if __name__=="__main__":
    from firefly.master.master import Master
    master = Master()
    master.config('config.json','appmain.py')
    master.start()

'''
1、master:管理模块，通过subprocess.Popen()来启动其它模块，该模块启动一个webserver，简单的通过监听本机9998端口，
用get方法来获取用户管理命令，目前默认的是2条命令，stop和reload，负责其它模块的stop，reload功能。
只要在本机浏览器输入：htt空格p://localhost:9998/stop 或者htt空格p://localhost:9998/reload 即可。
2、dbfront：数据库前端模块，负责管理DB和Memcache。比如load用户信息到memcache中，定期（系统写死了1800秒）刷新并同步memcache.

3、gate：这个其实是真正的center，其它模块（除了dbfront）都会和这个模块挂接（通过twisted.pb  后面会抽空详细说明）。
4、net：网络模块，负责监听客户端tcp/ip连接，转发相应的命令数据包给gate。  =>  port 11009 客户端需要连接的端口
5、game1：暗黑世界的游戏模块，这个模块里面会处理几乎所有的游戏逻辑，存储所有的游戏数据：
比如角色升级的经验等级，各种npc信息，各种掉落信息，各种战斗阵型。
这些数据在系统启动前都是保存在mysql里面，game1模块负责load到自己的内存里面（注意，不是memcache里面，而是直接内存）

6、admin：系统管理员模块，其实这个模块对于游戏本身来说，可有可无，主要作用就是导出游戏统计数据，
比如在线人数，每天充值数量等等。。。。无非就是简单的load数据库内容在简单做些计算而已，我们不做重点。
'''

'''
1：client send() #发送数据包
2: net recv() #接收，解析数据包
3: gate forward()#分发数据包到各个服务节点
4：[game]service doserv()# 处理数据包逻辑计算
5：[master]dbserv CRUD #数据持久化
6：response # 按相反顺序返回。

1至2走sock的tcp协议 ;
2-3-4走：使用RPC ;
4-5走的dbpool或memcached连接。
'''
    
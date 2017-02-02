# Firefly服务器框架安装和使用

---

## Firefly在CentOS下的安装
> * 安装Python
`下载Python-2.7.11`  
wget https://www.python.org/ftp/python/2.7.11/Python-2.7.11.tgz  
`解压并进入目录`  
tar zxvf Python-2.7.11.tgz  
cd Python-2.7.11  
`编译安装`  
./configure  
make all  
make install  
make clean  
`建立软连接，使系统默认的python指向python2.7`  
mv /usr/bin/python /usr/bin/python2.6.6  
ln -s /usr/bin/python2.7 /usr/bin/python  
`解决系统 Python 软链接指向 Python2.7 版本后，因为yum是不兼容Python2.7的，所以yum不能正常工作，我们需要指定yum的Python版本`  
\#vi /usr/bin/yum  
将文件头部的  
\#!/usr/bin/python  
改成  
\#!/usr/bin/python2.6.6
> * 安装MySQL
> * 安装memcached
`启动命令`  
memcached -u root -d
> * 安装pip：python `get-pip.py`
> * 将register.py放到D盘然后：python `register.py`
> * pip install `twisted`
> * pip install `python-memcached`
> * pip install `DBUtils`
> * pip install `affinity`
> * pip install `MySQL-python`
`如果发生错误`  
yum install python-devel  
yum install mysql-devel
> * 安装Firefly，可以pip install `firefly`

## Firefly在CentOS下的使用
> * firefly-admin.py createproject Test
> * cd Test
> * python startmaster.py

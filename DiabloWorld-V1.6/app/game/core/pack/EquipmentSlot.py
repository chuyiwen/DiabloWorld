#coding:utf8
'''
Created on 2011-3-29

@author: sean_lan
'''
from app.share.dbopear import dbItems
from app.game import util
from app.game.memmode import tb_equipment

BODYTYPE = ['body','trousers','header','bracer',
            'shoes','belt','necklace','ring','weapon','subweapon']
    
class EquipmentSlot:
    '''角色装备栏'''
    #装备栏中装备位置编号（item的bodytype，装备在身体的部位）
    #0=衣服
    #1=裤子
    #2=头盔
    #3=手套
    #4=靴子
    #5=腰带
    #6=项链
    #7=戒指
    #8=主武器
    #9=副武器
    
    def __init__(self,size = 10):
        '''
        @param size: int 包裹的大小
        '''
        self._items = {}
        
    def putEquipmentInEquipmentSlot(self,parts,equipmentid):
        '''根据数据库获取的信息设置物品
        @param part: str 部位名称
        @param equipment:  Item object 装备实例
        '''
        if parts in BODYTYPE:
            self._items[BODYTYPE.index(parts)] = equipmentid
        
    def updateEquipment(self,partsId,equipmentid):
        '''更换装备
        @param partsId: int 角色的部位的id
        @param equipment: Item object 装备
        '''
        self._items[partsId] = equipmentid
        return True
    
    def getItemByPosition(self, position):
        '''根据坐标得到物品
        @param position: int 物品的位置
        '''
        return self._items.get(position)
    
    def updateEquipments(self,characterid):
        """
        """
        prop = {}
        for pos,itemid in self._items.items():  # 遍历装备槽
            parts = BODYTYPE[pos]
            prop[parts] = itemid
        equipmentsInfo = tb_equipment.getObj(characterid)  # 从当前装备获取信息
        print characterid,equipmentsInfo
        equipmentsInfo.update_multi(prop)  # mmode 更新装备
    
    def getAllEquipttributes(self):
        '''得到玩家装备附加属性列表'''
        EXTATTRIBUTE = {}
        for item in [item['itemComponent'] for item in self._items]:
            info = item.getItemAttributes()
            equipsetattr = self.getEquipmentSetAttr()
            # 两个字典相加两个字典类型相加，有相同key的value值相加，不同的key合并,只支持value值为数值类型
            EXTATTRIBUTE = util.addDict(EXTATTRIBUTE, info)
        EXTATTRIBUTE = util.addDict(EXTATTRIBUTE, equipsetattr)
        return EXTATTRIBUTE
    
    def getEquipmentSetCont(self):
        '''获取装备中的装备的套装件数
        其实不需要这么麻烦的吧...直接一个 for循环 就可以记录完毕
        '''
        itemsetlist = [item['itemComponent'].baseInfo.itemtemplateInfo['suiteId'] \
                        for item in self._items \
                        if item['itemComponent'].baseInfo.itemtemplateInfo['suiteId']]
        nowsets = set(itemsetlist)  # 相同套装同 suiteId，所以只会加入 set集合 一次
        setcontdict = {}
        for setid in nowsets:
            setcount = itemsetlist.count(setid)  # 相同套装的件数
            setcontdict[setid] = setcount
        return setcontdict
    
    def getEquipmentSetAttr(self):
        '''获取套装属性加成
        '''
        itemsetlist = [item['itemComponent'].baseInfo.itemtemplateInfo['suiteId'] \
                        for item in self._items \
                        if item['itemComponent'].baseInfo.itemtemplateInfo['suiteId']]
        nowsets = set(itemsetlist)
        info = {}  # 效果字典
        for setid in nowsets:
            setinfo = dbItems.ALL_SETINFO[setid]  # 这里导入 dbItems，取得套装效果
            setcount = itemsetlist.count(setid)  # 相同套装的件数
            allsetattr = eval(setinfo['effect'])  # eval？执行什么...不清楚
            for key,value in allsetattr.items():
                if key <= setcount:  # 超过件数，产生效果
                    effect = eval(value.get('effect'))  # 又是 eval？？？
                    info = util.addDict(info, effect)
        return info
    
    def getItemPositionById(self,itemId):
        '''根据物品的id获取物品的位置'''
        for pos,itemid in self._items.items():
            if itemid==itemId:
                return pos
        return -1
    

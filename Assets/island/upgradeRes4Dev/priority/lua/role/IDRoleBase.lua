--[[
//                    ooOoo
//                   8888888
//                  88" . "88
//                  (| -_- |)
//                  O\  =  /O
//               ____/`---'\____
//             .'  \\|     |//  `.
//            /  \\|||  :  |||//  \
//           /  _||||| -:- |||||-  \
//           |   | \\\  -  /// |   |
//           | \_|  ''\---/''  |_/ |
//            \ .-\__  `-`  ___/-. /
//         ___ `. .' /--.--\  `. . ___
//      ."" '<  `.___\_<|>_/___.'  >' "".
//     | | : ` - \`.`\ _ /`.`/- ` : | |
//     \ \ `-.    \_ __\ /__ _/   .-` / /
//======`-.____`-.___\_____/___.-`____.-'======
//                   `=---='
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//           佛祖保佑       永无BUG
//           游戏大卖       公司腾飞
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
--]]
require("public.class")
-- 角色基础相关
---@class IDRoleBase:IDLUnitBase
IDRoleBase = class("IDRoleBase", IDLUnitBase)

---@param csSelf Coolape.CLUnit
function IDRoleBase:ctor(csSelf)
    self:getBase(IDRoleBase).ctor(self, csSelf)
    ---@type Coolape.CLUnit
    self.csSelf = csSelf -- cs对象
    ---@type UnityEngine.Transform
    self.transform = nil
    ---@type UnityEngine.GameObject
    self.gameObject = nil
    self.isOffense = false -- 是进攻方
    self.id = 0

    ---@type WrapBattleUnitData
    self.serverData = nil -- 服务器数据
    self.attr = nil -- 属性
    self.isFinishInited = false
    self.isDead = false
end

function IDRoleBase:init(selfObj, id, star, lev, _isOffense, other)
    self:getBase(IDRoleBase).init(self, selfObj, id, star, lev, _isOffense, other)
    self.csSelf.isOffense = _isOffense
    self.isOffense = _isOffense
    self.id = id
    self.csSelf.id = id
    self.isDead = false
    self.csSelf.isDead = false
    self.instanceID = self.gameObject:GetInstanceID()
    self.csSelf.instanceID = self.instanceID

    -- 初始化
    self.csSelf.RandomFactor = self.csSelf:initRandomFactor()
    self.csSelf.RandomFactor2 = self.csSelf:initRandomFactor2()
    self.csSelf.RandomFactor3 = self.csSelf:initRandomFactor3()
    -- 取得属性配置
    if other then
        self.serverData = other.serverData
    end
    ---@type DBCFRoleData
    self.attr = DBCfg.getRoleByID(id)

    if GameMode.battle == MyCfg.mode then
        -- 初始化数据
        ---@type UnitData4Battle
        self.data = {}
        self.data.HP =
            DBCfg.getGrowingVal(
            bio2number(self.attr.HPMin),
            bio2number(self.attr.HPMax),
            bio2number(self.attr.HPCurve),
            bio2number(self.serverData.lev) / bio2number(self.attr.MaxLev)
        )
        self.data.curHP = number2bio(self.data.HP)
        self.data.HP = number2bio(self.data.HP)
    end

    self:loadShadow()
end
function IDRoleBase:__init(selfObj, other)
    if self.isFinishInited then
        return
    end
    self:getBase(IDRoleBase).__init(self, selfObj, other)
    self.csSelf = selfObj
    self.transform = selfObj.transform
    self.gameObject = selfObj.gameObject
    self.body = selfObj.mbody
    if self.body and (not self.body:IsNull()) then
        self.action = self.body:GetComponent("CLRoleAction")
    end
end

---@public 加载影子
function IDRoleBase:loadShadow()
    if self.attr == nil then
        return
    end
    if self.shadow == nil then
        local shadowType = bio2number(self.attr.ShadowType)
        if shadowType < 1 then
            return
        end
        local shadowName = joinStr("shadow", shadowType)
        CLUIOtherObjPool.borrowObjAsyn(
            shadowName,
            function(name, obj, orgs)
                if (not self.gameObject.activeInHierarchy) or self.shadow ~= nil then
                    CLUIOtherObjPool.returnObj(obj)
                    SetActive(obj, false)
                    return
                end
                self.shadow = obj.transform
                self.shadow.parent = MyCfg.self.shadowRoot
                self.shadow.localEulerAngles = Vector3.zero
                self.shadow.localScale = Vector3.one * bio2number(self.attr.ShadowSize) / 10
                self.shadow.position = self.transform.position + Vector3.up * 0.01
                SetActive(self.shadow.gameObject, true)
            end
        )
    end
end

---@public 播放动作
function IDRoleBase:playAction(actionName, onCompleteMotion)
    if self.action == nil then
        printe("self.action is nil")
        return
    end
    if onCompleteMotion == nil then
        onCompleteMotion = ActCBtoList(100, self:wrapFunc(self.onActionFinish))
    end
    self.action:setAction(getAction(actionName), onCompleteMotion)
end

---@public 当动作播放完的通用回调
function IDRoleBase:onActionFinish(act)
    local actVal = act.currActionValue
    if getAction("idel") == actVal then
    elseif getAction("idel2") == actVal then
    elseif getAction("idel3") == actVal then
    elseif getAction("dead") == actVal then
    elseif getAction("dizzy") == actVal then
    elseif getAction("run") == actVal then
    elseif getAction("walk") == actVal then
        return
    end
    self:idel()
end

function IDRoleBase:idel()
    self:playAction("idel")
end

---@public 被击中
---@param damage number 伤害值
---@param attacker IDLUnitBase 攻击方
function IDRoleBase:onHurt(damage, attacker)
    -- self:getBase(IDRoleBase).onHurt(self, damage, attacker)
    local curHP = bio2number(self.data.curHP)
    curHP = curHP - damage
    if curHP < 0 then
        curHP = 0
    end
    self.data.curHP = number2bio(curHP)
    -- //TODO:显示扣血效果
    if curHP <= 0 then
        self:onDead()
    end
end

function IDRoleBase:iamDie()
    CLEffect.play(self.attr.DeadEffect, self.transform.position)
    if self.attr.DeadSound then
        SoundEx.playSound(self.attr.DeadSound, 1, 1)
        SetActive(self.gameObject, false)
        IDLBattle.someOneDead(self)
    else
        self:playDeadSund(0)
    end
end

---@public 播放死亡音效，播完后再通知战场
function IDRoleBase:playDeadSund(i)
    if i == 0 then
        SoundEx.playSound("heavy_die_01", 1, 1)
        self.csSelf:invoke4Lua(self.playDeadSund, i + 1, 0.2)
    elseif i == 1 then
        Utl.playSound("heavy_die_03", 1, 1)
        self.csSelf:invoke4Lua(self.playDeadSund, i + 1, 0.2)
    elseif i == 2 then
        Utl.playSound("heavy_die_04", 1, 1)
        self.csSelf:invoke4Lua(self.playDeadSund, i + 1, 0.2)
    elseif i == 3 then
        Utl.playSound("heavy_die_02", 1, 1)
        SetActive(self.gameObject, false)
        IDLBattle.someOneDead(self)
    end
end

function IDRoleBase:clean()
    self:getBase(IDRoleBase).clean(self)
    if self.shadow then
        CLUIOtherObjPool.returnObj(self.shadow.gameObject)
        SetActive(self.shadow.gameObject, false)
        self.shadow = nil
    end
end

--------------------------------------------
return IDRoleBase

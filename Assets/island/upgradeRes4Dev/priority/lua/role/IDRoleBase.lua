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
--//TODO:角色在地表行走时，还要考虑那那个是假地块的情况

require("public.class")
-- 角色基础相关
---@class IDRoleBase:IDLUnitBase
IDRoleBase = class("IDRoleBase", IDLUnitBase)

---@param csSelf Coolape.CLUnit
function IDRoleBase:ctor(csSelf)
    self:getBase(IDRoleBase).ctor(self, csSelf)

    ---@type WrapBattleUnitData
    self.serverData = nil -- 服务器数据
    ---@type DBCFRoleData
    self.attr = nil -- 属性
    -- 冰冻时间
    self.frozenTime = 0
end

---@param selfObj MyUnit
function IDRoleBase:__init(selfObj, other)
    if self.isFinishInited then
        return
    end
    self:getBase(IDRoleBase).__init(self, selfObj, other)
    self.body = selfObj.mbody
    if self.body and (not self.body:IsNull()) then
        ---@type Coolape.CLRoleAction
        self.action = self.body:GetComponent("CLRoleAction")
    end
    ---@type Coolape.CLRoleAvata
    self.avata = selfObj:GetComponent("CLRoleAvata")
    ---@type Coolape.CLSharedAssets
    self.assets = selfObj:GetComponent("CLSharedAssets")

    ---@type Coolape.CLSeeker
    self.seeker = self.csSelf:GetComponent("CLSeekerByRay")
    if self.seeker == nil then
        self.seeker = self.csSelf:GetComponent("CLSeeker")
    end
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
    self.isRole = true
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

        self.data.damage =
            DBCfg.getGrowingVal(
            bio2number(self.attr.DamageMin),
            bio2number(self.attr.DamageMax),
            bio2number(self.attr.DamageCurve),
            bio2number(self.serverData.lev) / bio2number(self.attr.MaxLev)
        )
        self.data.damage = number2bio(self.data.damage)
    end
    self:regain()
    self:loadShadow()
    SetActive(self.body.gameObject, false)
    self.assets:init(self:wrapFunc(self.dress), IDConst.dressMode.normal)
end

-- function IDRoleBase:uiEventDelegate(go)
--     -- 进入这个方法，说明资源加载好了
--     self:dress(IDConst.dressMode.normal)
-- end

---@public 换装
---@param mode  IDConst.dressMode
function IDRoleBase:dress(mode)
    if self.avata == nil then
        return
    end
    if mode == IDConst.dressMode.ice then
        self.avata:switch2xx("body", "2", self:wrapFunc(self.onFinishDress))
    else
        self.avata:switch2xx("body", "1", self:wrapFunc(self.onFinishDress))
    end
end

function IDRoleBase:onFinishDress()
    SetActive(self.body.gameObject, true)
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
                local pos = self.transform.position
                pos.y = 0
                self.shadow.position = pos + Vector3.up * 0.01
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
    self:getBase(IDRoleBase).onHurt(self, damage, attacker)
end

function IDRoleBase:iamDie()
    -- //TODO:可以考虑在同一网格中只能有1到2个死亡特效，因为有可能很多舰船士兵都在同一时间段死掉
    CLEffect.play(self.attr.DeadEffect, self.transform.position)
    if isNilOrEmpty(self.attr.DeadSound) then
        self:playDeadSund(0)
    else
        SoundEx.playSound(self.attr.DeadSound, 1, 1)
        SetActive(self.gameObject, false)
        IDLBattle.someOneDead(self)
    end
end

---@public 播放死亡音效，播完后再通知战场
function IDRoleBase:playDeadSund(i)
    if i == 0 then
        SoundEx.playSound("heavy_die_01", 1, 1)
        self.csSelf:invoke4Lua(self.playDeadSund, i + 1, 0.2)
    elseif i == 1 then
        SoundEx.playSound("heavy_die_03", 1, 1)
        self.csSelf:invoke4Lua(self.playDeadSund, i + 1, 0.2)
    elseif i == 2 then
        SoundEx.playSound("heavy_die_04", 1, 1)
        self.csSelf:invoke4Lua(self.playDeadSund, i + 1, 0.2)
    elseif i == 3 then
        SoundEx.playSound("heavy_die_02", 1, 1)
        SetActive(self.gameObject, false)
        IDLBattle.someOneDead(self)
    end
end

---@public 冰冻
function IDRoleBase:frozen(sec)
    --//TODO:注意冰冻会不会影响其它的处理
    self.state = IDConst.RoleState.frozen
    local leftTime = self.frozenTime - DateEx.nowMS
    if leftTime >= sec * 1000 then
        return
    end
    self.frozenTime = DateEx.nowMS + sec * 1000
    self:dress(IDConst.dressMode.ice)
    self:pause()
    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.unFrozen))
    InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.unFrozen), sec)
end

---@public 解除冰冻
function IDRoleBase:unFrozen()
    self.state = IDConst.RoleState.idel
    self:dress(IDConst.dressMode.normal)
    self:regain()
end

function IDRoleBase:pause()
    if self.action then
        self.action.enabled = false
    end
    if self.seeker then
        self.seeker.enabled = false
    end
end

function IDRoleBase:regain()
    if self.action then
        self.action.enabled = true
    end
    if self.seeker then
        self.seeker.enabled = true
    end
end

function IDRoleBase:clean()
    self:getBase(IDRoleBase).clean(self)
    if self.shadow then
        CLUIOtherObjPool.returnObj(self.shadow.gameObject)
        SetActive(self.shadow.gameObject, false)
        self.shadow = nil
    end

    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.unFrozen))
end

--------------------------------------------
return IDRoleBase

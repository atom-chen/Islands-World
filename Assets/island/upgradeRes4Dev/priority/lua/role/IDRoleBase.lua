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
    if self:getBase(IDRoleBase).__init(self, selfObj, other) then
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
        return true
    end
    return false
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
    if bio2number(self.attr.Bullets) > 0 then
        ---@type DBCFBulletData 子弹
        self.bulletAttr = DBCfg.getBulletByID(bio2number(self.attr.Bullets))
    end

    self.MaxAttackRange = bio2number(self.attr.AttackRange)
    self.MinAttackRange = bio2number(self.attr.MinAttackRange)
    -- 高度
    self.flyHeigh = bio2number(self.attr.FlyHeigh) / 10
    -- 寻路相关设置
    if self.attr.IsFlying then
        if self.seeker then
            self.seeker.rayHeight = self.flyHeigh
        end
    else
        if self.seeker then
            self.seeker.mAStarPathSearch = IDMainCity.astar4Ocean
            self.seeker.mAStarPathSearch:addGridStateChgCallback(self:wrapFunction4CS(self.onAstarChgCallback))
        end
    end
    if self.seeker then
        self.seeker:init(
            self:wrapFunction4CS(self.onSearchPath),
            self:wrapFunction4CS(self.onMoving),
            self:wrapFunction4CS(self.onArrived)
        )
        local speed = bio2number(self.attr.MoveSpeed) / 100
        speed = speed * (1 + self.csSelf:fakeRandom(-100, 100) / 1000)
        self.seeker.speed = speed
        local AttackRange = bio2number(self.attr.AttackRange) / 100
        AttackRange = AttackRange + 1 * self.csSelf:fakeRandom2(-100, 0) / 500
        self.seeker.endReachedDistance = AttackRange
    end

    self:regain()
    self:loadShadow()
    SetActive(self.body.gameObject, false)
    self.assets:init(self:wrapFunc(self.dress), IDConst.dressMode.normal)

    if GameMode.battle == MyCfg.mode then
        -- 战斗时，初始化数据
        ---@type UnitData4Battle
        self.data = {}
        -- 最大血量
        self.data.HP =
            DBCfg.getGrowingVal(
            bio2number(self.attr.HPMin),
            bio2number(self.attr.HPMax),
            bio2number(self.attr.HPCurve),
            bio2number(self.serverData.lev) / bio2number(self.attr.MaxLev)
        )
        -- 当前血量
        self.data.curHP = number2bio(self.data.HP)
        self.data.HP = number2bio(self.data.HP)
        -- 伤害值
        self.data.damage =
            DBCfg.getGrowingVal(
            bio2number(self.attr.DamageMin),
            bio2number(self.attr.DamageMax),
            bio2number(self.attr.DamageCurve),
            bio2number(self.serverData.lev) / bio2number(self.attr.MaxLev)
        )
        self.data.damage = number2bio(self.data.damage)
    end
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
    self:getBase(IDRoleBase).pause(self)
    if self.action then
        self.action.enabled = false
    end
    if self.seeker then
        self.seeker.enabled = false
    end
end

function IDRoleBase:regain()
    self:getBase(IDRoleBase).regain(self)
    if self.action then
        self.action.enabled = true
    end
    if self.seeker then
        self.seeker.enabled = true
    end
end

---@public 当Astar 网格刷新时回调
function IDRoleBase:onAstarChgCallback()
end

---@public 当寻路完成
function IDRoleBase:onSearchPath(pathList, canReach)
    if MyCfg.mode == GameMode.battle then
        InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.refresh4Searcher))
        InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.refresh4Searcher), 0.1)
    end
end

---@public 战斗时，要定时刷新寻敌器的位置
function IDRoleBase:refresh4Searcher()
    if MyCfg.mode == GameMode.battle then
        IDLBattle.searcher.refreshUnit(self)
        InvokeEx.invokeByFixedUpdate(self:wrapFunc(self.refresh4Searcher), 0.1)
    end
end

---@public 当移动过程中的回调
function IDRoleBase:onMoving()
    if self.shadow then
        self.tmpPos = self.transform.position
        self.tmpPos.y = 0
        self.shadow.position = self.tmpPos
    end
    local pos = self.transform.position
    pos.y = self.flyHeigh
    self.transform.position = pos
end

---@public 当到达目标时的回调
function IDRoleBase:onArrived()
    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.refresh4Searcher))
    self:refresh4Searcher()
end

---@public 攻击
function IDRoleBase:doAttack()
    self:doSearchTarget()
    if self.target then
        if self.attr.IsFlying then
            self.seeker:seek(self.target.transform.position)
        else
            self.seeker:seekAsyn(self.target.transform.position)
        end
    end
end

function IDRoleBase:doSearchTarget()
    local target = self.target
    if target == nil or target.isDead then
        -- 重新寻敌
        target = IDLBattle.searchTarget(self)
    else
        local pos1 = self.transform.position
        pos1.y = 0
        local pos2 = target.transform.position
        pos2.y = 0
        local dis = Vector3.Distance(pos1, pos2)
        -- dis减掉0.6，是因为寻敌时用的网格的index来计算的距离，因为可有可以对象在网格边上的情况
        if (dis - 0.6) > self.MaxAttackRange or dis < self.MinAttackRange then
            -- 重新寻敌
            target = IDLBattle.searchTarget(self)
        end
    end
    -- 设置目标
    self:setTarget(target)
end

function IDRoleBase:clean()
    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.unFrozen))
    InvokeEx.cancelInvokeByFixedUpdate(self:wrapFunc(self.refresh4Searcher))
    
    self:setTarget(nil)
    if self.seeker then
        self.seeker:stopMove()
        if self.seeker.mAStarPathSearch then
            self.seeker.mAStarPathSearch:removeGridStateChgCallback(self.onAstarChgCallback)
        end
    end
    self:getBase(IDRoleBase).clean(self)
    if self.shadow then
        CLUIOtherObjPool.returnObj(self.shadow.gameObject)
        SetActive(self.shadow.gameObject, false)
        self.shadow = nil
    end
end

--------------------------------------------
return IDRoleBase

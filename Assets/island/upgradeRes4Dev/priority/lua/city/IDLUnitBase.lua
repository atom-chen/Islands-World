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
-- 建筑基础相关
---@class IDLUnitBase:ClassBase
IDLUnitBase = class("IDLUnitBase")

---@class UnitData4Battle 战斗时数据
---@field public HP number 总血量
---@field public curHP number 当前血量
---@field public damage number 伤害值

---@param csSelf MyUnit
function IDLUnitBase:ctor(csSelf)
    ---@type MyUnit
    self.csSelf = csSelf -- cs对象
    ---@type UnityEngine.Transform
    self.transform = nil
    ---@type UnityEngine.GameObject
    self.gameObject = nil
    ---@type UnitData4Battle
    self.data = nil
    ---@type Coolape.CLCellLua
    self.lifebar = nil
    self.isOffense = false -- 是进攻方
    self.id = 0
    self.isFinishInited = false
    self.isDead = false
    -- 暂停状态
    self.isPause = false
    -- 状态
    self.state = IDConst.RoleState.idel
end

---@return boolean 返回true表示可以处理init,否则不需要处理（保证__init只调用一次）
function IDLUnitBase:__init(selfObj, other)
    if self.isFinishInited then
        return false
    end
    self.isFinishInited = true
    self.csSelf = selfObj
    self.transform = selfObj.transform
    self.gameObject = selfObj.gameObject
    return true
end

function IDLUnitBase:init(selfObj, id, star, lev, _isOffense, other)
    self:__init(selfObj, other)
    self.state = IDConst.RoleState.idel
    self.csSelf.isOffense = _isOffense
    self.isOffense = _isOffense
    self.id = id
    self.csSelf.id = id
    self.isDead = false
    self.csSelf.isDead = false
    self.instanceID = self.gameObject:GetInstanceID()
    self.csSelf.instanceID = self.instanceID
    self.isPause = false
end

function IDLUnitBase:setCollider(val)
end

function IDLUnitBase:SetActive(active)
    SetActive(self.gameObject, active)
end

---@public 被击中
---@param damage number 伤害值
---@param attacker IDLUnitBase 攻击方
function IDLUnitBase:onHurt(damage, attacker)
    local curHP = bio2number(self.data.curHP)
    curHP = curHP - damage
    if curHP < 0 then
        curHP = 0
    elseif curHP > bio2number(self.data.HP) then
        curHP = bio2number(self.data.HP)
    end
    self.data.curHP = number2bio(curHP)
    -- 显示扣血效果
    self:showLifebar(damage)
    if curHP <= 0 then
        self:onDead()
    end
end

---@public 显示扣血效果
function IDLUnitBase:showLifebar(damage)
    local data = {damage = damage, unit = self, offset = Vector3.up * 0.2}
    if self.lifebar == nil then
        CLUIOtherObjPool.borrowObjAsyn(
            "LifebarHud",
            function(name, obj, orgs)
                ---@type UnityEngine.GameObject
                local go = obj
                if self.lifebar then
                    CLUIOtherObjPool.returnObj(obj)
                    SetActive(obj, false)
                else
                    self.lifebar = go:GetComponent("CLCellLua")
                    self.lifebar.transform.parent = MyCfg.self.hud3dRoot
                    self.lifebar.transform.localScale = Vector3.one
                    self.lifebar.transform.localEulerAngles = Vector3.zero
                    SetActive(self.lifebar.gameObject, true)
                end
                self.lifebar:init(data, nil)
            end
        )
    else
        self.lifebar:init(data, nil)
    end
    self.csSelf:cancelInvoke4Lua(self.hideLifebar)
    self.csSelf:invoke4Lua(self.hideLifebar, 2)
end

function IDLUnitBase:hideLifebar()
    if self.lifebar then
        CLUIOtherObjPool.returnObj(self.lifebar.gameObject)
        SetActive(self.lifebar.gameObject, false)
        self.lifebar = nil
    end
end

---@public 寻敌
function IDLUnitBase:doSearchTarget()
    printe("must override [doSearchTarget] function!")
end

---@public 取得伤害值
---@param target 目标
function IDLUnitBase:getDamage(target)
    printe("must override [getDamage] function!")
end

---@public 死掉了
function IDLUnitBase:onDead()
    if self.isDead then
        return
    end
    self.csSelf:cancelInvoke4Lua()
    self.isDead = true
    self.csSelf.isDead = true
    self:iamDie()
end

---@public 冰冻
function IDLUnitBase:frozen(sec)
    printe("must override [frozen] function!")
end

---@public 我死掉了，处理死掉的效果
function IDLUnitBase:iamDie()
    printe("must override [iamDie] function!")
end

function IDLUnitBase:pause()
    self.isPause = true
end

function IDLUnitBase:regain()
    self.isPause = false
end

function IDLUnitBase:clean()
    self:_clean()
end

function IDLUnitBase:_clean()
    self.csSelf:cancelInvoke4Lua()
    self:hideLifebar()
end

return IDLUnitBase

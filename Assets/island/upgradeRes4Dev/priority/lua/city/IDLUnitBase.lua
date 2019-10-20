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
end

function IDLUnitBase:__init(selfObj, other)
    if self.isFinishInited then
        return
    end
    self.isFinishInited = true
    self.csSelf = selfObj
    self.transform = selfObj.transform
    self.gameObject = selfObj.gameObject
end

function IDLUnitBase:init(selfObj, id, star, lev, _isOffense, other)
    self:__init(selfObj, other)
    self.csSelf.isOffense = _isOffense
    self.isOffense = _isOffense
    self.id = id
    self.csSelf.id = id
    self.isDead = false
    self.csSelf.isDead = false
    self.instanceID = self.gameObject:GetInstanceID()
    self.csSelf.instanceID = self.instanceID
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
    printe("must override [onHurt] function!")
end

---@public 取得伤害值
function IDLUnitBase:getDamage()
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

---@public 我死掉了，处理死掉的效果
function IDLUnitBase:iamDie()
    printe("must override [iamDie] function!")
end

function IDLUnitBase:clean()
    self:_clean()
end

function IDLUnitBase:_clean()
    self.csSelf:cancelInvoke4Lua()
end

return IDLUnitBase

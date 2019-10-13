---@class MyUnit : Coolape.CLUnit
local m = { }
---public MyUnit .ctor()
---@return MyUnit
function m.New() end
---public Void initGetLuaFunc()
function m:initGetLuaFunc() end
---public Void init(Int32 id, Int32 star, Int32 lev, Boolean isOffense, Object other)
---@param optional Int32 id
---@param optional Int32 star
---@param optional Int32 lev
---@param optional Boolean isOffense
---@param optional Object other
function m:init(id, star, lev, isOffense, other) end
---public Void doAttack()
function m:doAttack() end
---public CLUnit doSearchTarget()
---@return CLUnit
function m:doSearchTarget() end
---public Void moveTo(Vector3 toPos)
---@param optional Vector3 toPos
function m:moveTo(toPos) end
---public Void moveToTarget(Transform target)
---@param optional Transform target
function m:moveToTarget(target) end
---public Void onBeTarget(CLUnit attacker)
---@param optional CLUnit attacker
function m:onBeTarget(attacker) end
---public Boolean onHurt(Int32 hurt, Object skillAttr, CLUnit attacker)
---@return bool
---@param optional Int32 hurt
---@param optional Object skillAttr
---@param optional CLUnit attacker
function m:onHurt(hurt, skillAttr, attacker) end
---public Void onHurtFinish(Object skillAttr, CLUnit attacker)
---@param optional Object skillAttr
---@param optional CLUnit attacker
function m:onHurtFinish(skillAttr, attacker) end
---public Void onHurtHP(Int32 hurt, Object skillAttr)
---@param optional Int32 hurt
---@param optional Object skillAttr
function m:onHurtHP(hurt, skillAttr) end
---public Void onRelaseTarget(CLUnit attacker)
---@param optional CLUnit attacker
function m:onRelaseTarget(attacker) end
---public Void onDead()
function m:onDead() end
MyUnit = m
return m

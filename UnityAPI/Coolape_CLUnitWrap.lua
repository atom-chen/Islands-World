---@class Coolape.CLUnit : Coolape.CLBehaviour4Lua
---@field public SCANRange System.Single
---@field public instanceID System.Int32
---@field public type System.Int32
---@field public id System.Int32
---@field public mTarget Coolape.CLUnit
---@field public mAttacker Coolape.CLUnit
---@field public isDead System.Boolean
---@field public hudAnchor UnityEngine.Transform
---@field public state Coolape.CLRoleState
---@field public isOffense System.Boolean
---@field public isCopyBody System.Boolean
---@field public size UnityEngine.Vector3
---@field public mbody UnityEngine.Transform
---@field public matMap System.Collections.Hashtable
---@field public RandomFactor System.Single
---@field public RandomFactor2 System.Single
---@field public RandomFactor3 System.Single
---@field public lev System.Int32
---@field public isDefense System.Boolean
---@field public collider UnityEngine.Collider
---@field public minSize System.Single
---@field public materials UnityEngine.Material
local m = { }
---public Void clean()
function m:clean() end
---public Void OnDrawGizmos()
function m:OnDrawGizmos() end
---public Void setMatOutLine()
function m:setMatOutLine() end
---public Void setMatIceEffect()
function m:setMatIceEffect() end
---public Void setMatViolent()
function m:setMatViolent() end
---public Void setMatOutLineWithColor(Color mainColor, Color outLineColor)
---@param optional Color mainColor
---@param optional Color outLineColor
function m:setMatOutLineWithColor(mainColor, outLineColor) end
---public Void setMatToonWithColor(Color mainColor)
---@param optional Color mainColor
function m:setMatToonWithColor(mainColor) end
---public Material getBodyMat(Transform tr)
---@return Material
---@param optional Transform tr
function m:getBodyMat(tr) end
---public Void setBodyMat(Transform tr, Material mat)
---@param optional Transform tr
---@param optional Material mat
function m:setBodyMat(tr, mat) end
---public Void setMatToon()
function m:setMatToon() end
---public Void pause()
function m:pause() end
---public Void regain()
function m:regain() end
---public Void Start()
function m:Start() end
---public Single initRandomFactor()
---@return number
function m:initRandomFactor() end
---public Int32 fakeRandom(Int32 min, Int32 max)
---@return number
---@param optional Int32 min
---@param optional Int32 max
function m:fakeRandom(min, max) end
---public Single initRandomFactor2()
---@return number
function m:initRandomFactor2() end
---public Int32 fakeRandom2(Int32 min, Int32 max)
---@return number
---@param optional Int32 min
---@param optional Int32 max
function m:fakeRandom2(min, max) end
---public Single initRandomFactor3()
---@return number
function m:initRandomFactor3() end
---public Int32 fakeRandom3(Int32 min, Int32 max)
---@return number
---@param optional Int32 min
---@param optional Int32 max
function m:fakeRandom3(min, max) end
---public Void init(Int32 id, Int32 star, Int32 lev, Boolean isOffense, Object other)
---@param optional Int32 id
---@param optional Int32 star
---@param optional Int32 lev
---@param optional Boolean isOffense
---@param optional Object other
function m:init(id, star, lev, isOffense, other) end
---public CLUnit doSearchTarget()
---@return CLUnit
function m:doSearchTarget() end
---public Void onBeTarget(CLUnit attacker)
---@param optional CLUnit attacker
function m:onBeTarget(attacker) end
---public Void onRelaseTarget(CLUnit attacker)
---@param optional CLUnit attacker
function m:onRelaseTarget(attacker) end
---public Void doAttack()
function m:doAttack() end
---public Void onHurtHP(Int32 hurt, Object skillAttr)
---@param optional Int32 hurt
---@param optional Object skillAttr
function m:onHurtHP(hurt, skillAttr) end
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
---public Void onDead()
function m:onDead() end
---public Void moveTo(Vector3 toPos)
---@param optional Vector3 toPos
function m:moveTo(toPos) end
---public Void moveToTarget(Transform target)
---@param optional Transform target
function m:moveToTarget(target) end
Coolape.CLUnit = m
return m

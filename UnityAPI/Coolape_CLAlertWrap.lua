---@class Coolape.CLAlert : UnityEngine.MonoBehaviour
---@field public self Coolape.CLAlert
---@field public hudBackgroundSpriteName System.String
---@field public hudBackgroundSpriteType UIBasicSprite.Type
---@field public bgAnchorLeft System.Int32
---@field public bgAnchorBottom System.Int32
---@field public bgAnchorTop System.Int32
---@field public bgAnchorRight System.Int32
---@field public hudBackgroundColor UnityEngine.Color
---@field public pool Coolape.SpriteHudPool

local m = { }
---public CLAlert .ctor()
---@return CLAlert
function m.New() end
---public Void add(Object msg)
---public Void add(Object msg, Color color, Single delayTime)
---public Void add(Object msg, Color color, Single scaleOffset, Vector3 posOffset)
---public Void add(Object msg, Color color, Single delayTime, Single scaleOffset)
---public Void add(Object msg, Color color, Single delayTime, Single scaleOffset, Boolean needBackGround, Vector3 posOffset)
---@param Object msg
---@param Color color
---@param Single delayTime
---@param Single scaleOffset
---@param Boolean needBackGround
---@param optional Vector3 posOffset
function m.add(msg, color, delayTime, scaleOffset, needBackGround, posOffset) end
Coolape.CLAlert = m
return m

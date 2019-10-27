---@class Coolape.CLAStarNode
---@field public left Coolape.CLAStarNode
---@field public right Coolape.CLAStarNode
---@field public up Coolape.CLAStarNode
---@field public down Coolape.CLAStarNode
---@field public leftUp Coolape.CLAStarNode
---@field public leftDown Coolape.CLAStarNode
---@field public rightUp Coolape.CLAStarNode
---@field public rightDown Coolape.CLAStarNode
---@field public aroundList System.Collections.Generic.List1Coolape.CLAStarNode
---@field public index System.Int32
---@field public position UnityEngine.Vector3
---@field public isObstruct System.Boolean
local m = { }
---public CLAStarNode .ctor(Int32 index, Vector3 pos)
---@return CLAStarNode
---@param optional Int32 index
---@param optional Vector3 pos
function m.New(index, pos) end
---public Void init(CLAStarNode left, CLAStarNode right, CLAStarNode up, CLAStarNode down, CLAStarNode leftUp, CLAStarNode leftDown, CLAStarNode rightUp, CLAStarNode rightDown)
---@param optional CLAStarNode left
---@param optional CLAStarNode right
---@param optional CLAStarNode up
---@param optional CLAStarNode down
---@param optional CLAStarNode leftUp
---@param optional CLAStarNode leftDown
---@param optional CLAStarNode rightUp
---@param optional CLAStarNode rightDown
function m:init(left, right, up, down, leftUp, leftDown, rightUp, rightDown) end
---public Void setParentNode(CLAStarNode preNode, String key)
---@param optional CLAStarNode preNode
---@param optional String key
function m:setParentNode(preNode, key) end
---public CLAStarNode getParentNode(String key)
---@return CLAStarNode
---@param optional String key
function m:getParentNode(key) end
Coolape.CLAStarNode = m
return m

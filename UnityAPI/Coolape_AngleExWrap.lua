---@class Coolape.AngleEx

local m = { }
---public Vector2 getEndVector2(Vector2 directionalFrom, Vector2 directionalTo, Vector2 from, Single toDistance)
---@return Vector2
---@param optional Vector2 directionalFrom
---@param optional Vector2 directionalTo
---@param optional Vector2 from
---@param optional Single toDistance
function m.getEndVector2(directionalFrom, directionalTo, from, toDistance) end
---public Vector2 getCirclePointV2(Vector2 p, Single r, Single angle)
---@return Vector2
---@param optional Vector2 p
---@param optional Single r
---@param optional Single angle
function m.getCirclePointV2(p, r, angle) end
---public Vector3 getCirclePointV3(Vector3 p, Single r, Single angle)
---@return Vector3
---@param optional Vector3 p
---@param optional Single r
---@param optional Single angle
function m.getCirclePointV3(p, r, angle) end
---public Vector3 getCirclePointStartWithXV3(Vector3 p, Single r, Single angle)
---@return Vector3
---@param optional Vector3 p
---@param optional Single r
---@param optional Single angle
function m.getCirclePointStartWithXV3(p, r, angle) end
---public Vector3 getCirclePointStartWithYV3(Vector3 p, Single r, Single angle)
---@return Vector3
---@param optional Vector3 p
---@param optional Single r
---@param optional Single angle
function m.getCirclePointStartWithYV3(p, r, angle) end
Coolape.AngleEx = m
return m

---@class UnityEngine.LayerMask : System.ValueType
---@field public value System.Int32

local m = { }
---public Int32 op_Implicit(LayerMask mask)
---public LayerMask op_Implicit(Int32 intVal)
---@return number
---@param optional Int32 intVal
function m.op_Implicit(intVal) end
---public String LayerToName(Int32 layer)
---@return String
---@param optional Int32 layer
function m.LayerToName(layer) end
---public Int32 NameToLayer(String layerName)
---@return number
---@param optional String layerName
function m.NameToLayer(layerName) end
---public Int32 GetMask(String[] layerNames)
---@return number
---@param optional String[] layerNames
function m.GetMask(layerNames) end
UnityEngine.LayerMask = m
return m

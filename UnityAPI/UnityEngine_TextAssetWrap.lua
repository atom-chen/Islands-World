---@class UnityEngine.TextAsset : UnityEngine.Object
---@field public text System.String
---@field public bytes System.Byte
local m = { }
---public TextAsset .ctor()
---public TextAsset .ctor(String text)
---@return TextAsset
---@param String text
function m.New(text) end
---public String ToString()
---@return String
function m:ToString() end
UnityEngine.TextAsset = m
return m

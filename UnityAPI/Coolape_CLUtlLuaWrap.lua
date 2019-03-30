---@class Coolape.CLUtlLua : UnityEngine.MonoBehaviour
---@field public isFinishAddLoader System.Boolean
---@field public FileBytesCacheMap System.Collections.Hashtable

local m = { }
---public CLUtlLua .ctor()
---@return CLUtlLua
function m.New() end
---public Void addLuaLoader(LuaEnv lua)
---@param optional LuaEnv lua
function m.addLuaLoader(lua) end
---public Void cleanFileBytesCacheMap()
function m.cleanFileBytesCacheMap() end
---public Byte[] myLuaLoader(String& filepath)
---@return table
---@param optional String& filepath
function m.myLuaLoader(filepath) end
---public Byte[] deCodeLua(Byte[] buff)
---@return table
---@param optional Byte[] buff
function m.deCodeLua(buff) end
---public String getLua(String fn)
---@return String
---@param optional String fn
function m.getLua(fn) end
---public Object[] doLua(LuaEnv lua, String _path)
---@return table
---@param optional LuaEnv lua
---@param optional String _path
function m.doLua(lua, _path) end
---public ArrayList luaTableKeys2List(LuaTable tb)
---@return ArrayList
---@param optional LuaTable tb
function m.luaTableKeys2List(table) end
---public ArrayList luaTableVals2List(LuaTable tb)
---@return ArrayList
---@param optional LuaTable tb
function m.luaTableVals2List(table) end
Coolape.CLUtlLua = m
return m

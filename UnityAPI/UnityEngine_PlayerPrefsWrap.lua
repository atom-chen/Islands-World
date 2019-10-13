---@class UnityEngine.PlayerPrefs
local m = { }
---public PlayerPrefs .ctor()
---@return PlayerPrefs
function m.New() end
---public Void SetInt(String key, Int32 value)
---@param optional String key
---@param optional Int32 value
function m.SetInt(key, value) end
---public Int32 GetInt(String key)
---public Int32 GetInt(String key, Int32 defaultValue)
---@return number
---@param String key
---@param optional Int32 defaultValue
function m.GetInt(key, defaultValue) end
---public Void SetFloat(String key, Single value)
---@param optional String key
---@param optional Single value
function m.SetFloat(key, value) end
---public Single GetFloat(String key)
---public Single GetFloat(String key, Single defaultValue)
---@return number
---@param String key
---@param optional Single defaultValue
function m.GetFloat(key, defaultValue) end
---public Void SetString(String key, String value)
---@param optional String key
---@param optional String value
function m.SetString(key, value) end
---public String GetString(String key)
---public String GetString(String key, String defaultValue)
---@return String
---@param String key
---@param optional String defaultValue
function m.GetString(key, defaultValue) end
---public Boolean HasKey(String key)
---@return bool
---@param optional String key
function m.HasKey(key) end
---public Void DeleteKey(String key)
---@param optional String key
function m.DeleteKey(key) end
---public Void DeleteAll()
function m.DeleteAll() end
---public Void Save()
function m.Save() end
UnityEngine.PlayerPrefs = m
return m

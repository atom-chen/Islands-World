---@class Localization
---@field public loadFunction Localization.LoadFunction
---@field public onLocalize Localization.OnLocalizeNotification
---@field public localizationHasBeenSet System.Boolean
---@field public dictionary System.Collections.Generic.Dictionary2System.StringSystem.String
---@field public knownLanguages System.String
---@field public language System.String

local m = { }
---public Void Load(TextAsset asset)
---public Void Load(String name, Byte[] asset)
---@param String name
---@param optional Byte[] asset
function m.Load(name, asset) end
---public Void Set(String languageName, Byte[] bytes)
---public Void Set(String languageName, Dictionary`2 dictionary)
---public Void Set(String key, String value)
---@param optional String key
---@param optional String value
function m.Set(key, value) end
---public Boolean LoadCSV(TextAsset asset, Boolean merge)
---public Boolean LoadCSV(Byte[] bytes, Boolean merge)
---@return bool
---@param optional Byte[] bytes
---@param optional Boolean merge
function m.LoadCSV(bytes, merge) end
---public Void append(String languageName, Byte[] asset)
---@param optional String languageName
---@param optional Byte[] asset
function m.append(languageName, asset) end
---public String Get(String key)
---@return String
---@param optional String key
function m.Get(key) end
---public String Format(String key, Object[] parameters)
---@return String
---@param optional String key
---@param optional Object[] parameters
function m.Format(key, parameters) end
---public Boolean Exists(String key)
---@return bool
---@param optional String key
function m.Exists(key) end
Localization = m
return m

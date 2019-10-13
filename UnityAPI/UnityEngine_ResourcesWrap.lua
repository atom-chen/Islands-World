---@class UnityEngine.Resources
local m = { }
---public Resources .ctor()
---@return Resources
function m.New() end
---public Object[] FindObjectsOfTypeAll(Type t)
---@return table
---@param optional Type t
function m.FindObjectsOfTypeAll(type) end
---public Object Load(String path)
---public Object Load(String path, Type systemTypeInstance)
---@return Object
---@param String path
---@param optional Type systemTypeInstance
function m.Load(path, systemTypeInstance) end
---public ResourceRequest LoadAsync(String path)
---public ResourceRequest LoadAsync(String path, Type t)
---@return ResourceRequest
---@param String path
---@param optional Type t
function m.LoadAsync(path, type) end
---public Object[] LoadAll(String path)
---public Object[] LoadAll(String path, Type systemTypeInstance)
---@return table
---@param String path
---@param optional Type systemTypeInstance
function m.LoadAll(path, systemTypeInstance) end
---public Object GetBuiltinResource(Type t, String path)
---@return Object
---@param optional Type t
---@param optional String path
function m.GetBuiltinResource(type, path) end
---public Void UnloadAsset(Object assetToUnload)
---@param optional Object assetToUnload
function m.UnloadAsset(assetToUnload) end
---public AsyncOperation UnloadUnusedAssets()
---@return AsyncOperation
function m.UnloadUnusedAssets() end
UnityEngine.Resources = m
return m

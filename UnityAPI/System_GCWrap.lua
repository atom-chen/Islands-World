---@class System.GC
---@field public MaxGeneration System.Int32

local m = { }
---public Void Collect()
---public Void Collect(Int32 generation)
---public Void Collect(Int32 generation, GCCollectionMode mode)
---@param Int32 generation
---@param GCCollectionMode mode
function m.Collect(generation, mode) end
---public Int32 GetGeneration(WeakReference wo)
---public Int32 GetGeneration(Object obj)
---@return number
---@param optional Object obj
function m.GetGeneration(obj) end
---public Int64 GetTotalMemory(Boolean forceFullCollection)
---@return long
---@param optional Boolean forceFullCollection
function m.GetTotalMemory(forceFullCollection) end
---public Void KeepAlive(Object obj)
---@param optional Object obj
function m.KeepAlive(obj) end
---public Void ReRegisterForFinalize(Object obj)
---@param optional Object obj
function m.ReRegisterForFinalize(obj) end
---public Void SuppressFinalize(Object obj)
---@param optional Object obj
function m.SuppressFinalize(obj) end
---public Void WaitForPendingFinalizers()
function m.WaitForPendingFinalizers() end
---public Int32 CollectionCount(Int32 generation)
---@return number
---@param optional Int32 generation
function m.CollectionCount(generation) end
---public Void AddMemoryPressure(Int64 bytesAllocated)
---@param optional Int64 bytesAllocated
function m.AddMemoryPressure(bytesAllocated) end
---public Void RemoveMemoryPressure(Int64 bytesAllocated)
---@param optional Int64 bytesAllocated
function m.RemoveMemoryPressure(bytesAllocated) end
System.GC = m
return m

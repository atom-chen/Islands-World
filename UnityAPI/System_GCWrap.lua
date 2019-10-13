---@class System.GC
---@field public MaxGeneration System.Int32
local m = { }
---public Void AddMemoryPressure(Int64 bytesAllocated)
---@param optional Int64 bytesAllocated
function m.AddMemoryPressure(bytesAllocated) end
---public Void RemoveMemoryPressure(Int64 bytesAllocated)
---@param optional Int64 bytesAllocated
function m.RemoveMemoryPressure(bytesAllocated) end
---public Int32 GetGeneration(Object obj)
---public Int32 GetGeneration(WeakReference wo)
---@return number
---@param optional WeakReference wo
function m.GetGeneration(wo) end
---public Void Collect()
---public Void Collect(Int32 generation)
---public Void Collect(Int32 generation, GCCollectionMode mode)
---public Void Collect(Int32 generation, GCCollectionMode mode, Boolean blocking)
---public Void Collect(Int32 generation, GCCollectionMode mode, Boolean blocking, Boolean compacting)
---@param Int32 generation
---@param GCCollectionMode mode
---@param Boolean blocking
---@param Boolean compacting
function m.Collect(generation, mode, blocking, compacting) end
---public Int32 CollectionCount(Int32 generation)
---@return number
---@param optional Int32 generation
function m.CollectionCount(generation) end
---public Void KeepAlive(Object obj)
---@param optional Object obj
function m.KeepAlive(obj) end
---public Void WaitForPendingFinalizers()
function m.WaitForPendingFinalizers() end
---public Void SuppressFinalize(Object obj)
---@param optional Object obj
function m.SuppressFinalize(obj) end
---public Void ReRegisterForFinalize(Object obj)
---@param optional Object obj
function m.ReRegisterForFinalize(obj) end
---public Int64 GetTotalMemory(Boolean forceFullCollection)
---@return long
---@param optional Boolean forceFullCollection
function m.GetTotalMemory(forceFullCollection) end
---public Void RegisterForFullGCNotification(Int32 maxGenerationThreshold, Int32 largeObjectHeapThreshold)
---@param optional Int32 maxGenerationThreshold
---@param optional Int32 largeObjectHeapThreshold
function m.RegisterForFullGCNotification(maxGenerationThreshold, largeObjectHeapThreshold) end
---public Void CancelFullGCNotification()
function m.CancelFullGCNotification() end
---public GCNotificationStatus WaitForFullGCApproach()
---public GCNotificationStatus WaitForFullGCApproach(Int32 millisecondsTimeout)
---@return number
---@param Int32 millisecondsTimeout
function m.WaitForFullGCApproach(millisecondsTimeout) end
---public GCNotificationStatus WaitForFullGCComplete()
---public GCNotificationStatus WaitForFullGCComplete(Int32 millisecondsTimeout)
---@return number
---@param Int32 millisecondsTimeout
function m.WaitForFullGCComplete(millisecondsTimeout) end
---public Boolean TryStartNoGCRegion(Int64 totalSize)
---public Boolean TryStartNoGCRegion(Int64 totalSize, Int64 lohSize)
---public Boolean TryStartNoGCRegion(Int64 totalSize, Boolean disallowFullBlockingGC)
---public Boolean TryStartNoGCRegion(Int64 totalSize, Int64 lohSize, Boolean disallowFullBlockingGC)
---@return bool
---@param Int64 totalSize
---@param Int64 lohSize
---@param optional Boolean disallowFullBlockingGC
function m.TryStartNoGCRegion(totalSize, lohSize, disallowFullBlockingGC) end
---public Void EndNoGCRegion()
function m.EndNoGCRegion() end
System.GC = m
return m

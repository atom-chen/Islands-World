---@class UICenterOnChild : UnityEngine.MonoBehaviour
---@field public springStrength System.Single
---@field public nextPageThreshold System.Single
---@field public onFinished SpringPanel.OnFinished
---@field public onCenter UICenterOnChild.OnCenterCallback
---@field public centeredObject UnityEngine.GameObject
local m = { }
---public UICenterOnChild .ctor()
---@return UICenterOnChild
function m.New() end
---public Void Recenter()
function m:Recenter() end
---public Void CenterOn(Transform target)
---@param optional Transform target
function m:CenterOn(target) end
UICenterOnChild = m
return m

---@class UIProgressBar : UIWidgetContainer
---@field public current UIProgressBar
---@field public onDragFinished UIProgressBar.OnDragFinished
---@field public thumb UnityEngine.Transform
---@field public numberOfSteps System.Int32
---@field public onChange System.Collections.Generic.List1EventDelegate
---@field public cachedTransform UnityEngine.Transform
---@field public cachedCamera UnityEngine.Camera
---@field public foregroundWidget UIWidget
---@field public backgroundWidget UIWidget
---@field public fillDirection UIProgressBar.FillDirection
---@field public value System.Single
---@field public alpha System.Single
local m = { }
---public UIProgressBar .ctor()
---@return UIProgressBar
function m.New() end
---public Void ForceUpdate()
function m:ForceUpdate() end
UIProgressBar = m
return m

---@class Coolape.TweenProgress : UITweener
---@field public from System.Single
---@field public to System.Single
---@field public finishCallback System.Object
---@field public slider UISlider
---@field public value System.Single

local m = { }
---public TweenProgress .ctor()
---@return TweenProgress
function m.New() end
---public Void Play(Boolean forward, Object finishCallback)
---public Void Play(Single from, Single to, Object finishCallback)
---@param Single from
---@param optional Single to
---@param optional Object finishCallback
function m:Play(from, to, finishCallback) end
Coolape.TweenProgress = m
return m

---@class UIPlayTween : UnityEngine.MonoBehaviour
---@field public current UIPlayTween
---@field public tweenTarget UnityEngine.GameObject
---@field public tweenGroup System.Int32
---@field public trigger AnimationOrTween.Trigger
---@field public playDirection AnimationOrTween.Direction
---@field public resetOnPlay System.Boolean
---@field public resetIfDisabled System.Boolean
---@field public ifDisabledOnPlay AnimationOrTween.EnableCondition
---@field public disableWhenFinished AnimationOrTween.DisableCondition
---@field public includeChildren System.Boolean
---@field public onFinished System.Collections.Generic.List1EventDelegate

local m = { }
---public UIPlayTween .ctor()
---@return UIPlayTween
function m.New() end
---public Void Play(Boolean forward)
---@param optional Boolean forward
function m:Play(forward) end
UIPlayTween = m
return m

---@class UITweener : UnityEngine.MonoBehaviour
---@field public exeOrder System.Int32
---@field public current UITweener
---@field public method UITweener.Method
---@field public style UITweener.Style
---@field public animationCurve UnityEngine.AnimationCurve
---@field public ignoreTimeScale System.Boolean
---@field public delay System.Single
---@field public duration System.Single
---@field public steeperCurves System.Boolean
---@field public tweenGroup System.Int32
---@field public onFinished System.Collections.Generic.List1EventDelegate
---@field public eventReceiver UnityEngine.GameObject
---@field public callWhenFinished System.String
---@field public onFinishCallback System.Object
---@field public amountPerDelta System.Single
---@field public tweenFactor System.Single
---@field public direction AnimationOrTween.Direction

local m = { }
---public Void Reset()
function m:Reset() end
---public Void SetOnFinished(EventDelegate del)
---public Void SetOnFinished(Callback del)
---@param optional Callback del
function m:SetOnFinished(del) end
---public Void AddOnFinished(EventDelegate del)
---public Void AddOnFinished(Callback del)
---@param optional Callback del
function m:AddOnFinished(del) end
---public Void RemoveOnFinished(EventDelegate del)
---@param optional EventDelegate del
function m:RemoveOnFinished(del) end
---public Void Sample(Single factor, Boolean isFinished)
---@param optional Single factor
---@param optional Boolean isFinished
function m:Sample(factor, isFinished) end
---public Void PlayForward()
function m:PlayForward() end
---public Void PlayReverse()
function m:PlayReverse() end
---public Void Play(Boolean forward)
---@param optional Boolean forward
function m:Play(forward) end
---public Void ResetToBeginning()
function m:ResetToBeginning() end
---public Void Toggle()
function m:Toggle() end
---public Void SetStartToCurrentValue()
function m:SetStartToCurrentValue() end
---public Void SetEndToCurrentValue()
function m:SetEndToCurrentValue() end
UITweener = m
return m

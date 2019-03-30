---@class Coolape.SScreenShakes : UnityEngine.MonoBehaviour
---@field public self Coolape.SScreenShakes
---@field public twPos TweenPosition
---@field public offset UnityEngine.Vector3

local m = { }
---public SScreenShakes .ctor()
---@return SScreenShakes
function m.New() end
---public Void play(Object finishCallback, Single delay)
---public Void play(Object finishCallback, Single delay, Single strength)
---public Void play(Object finishCallback, Single delay, Single strength, Boolean loop)
---@param Object finishCallback
---@param Single delay
---@param optional Single strength
---@param optional Boolean loop
function m.play(finishCallback, delay, strength, loop) end
---public Void stop()
function m.stop() end
---public Void _play(Object finishCallback, Single delay, Single strength, Boolean loop)
---@param optional Object finishCallback
---@param optional Single delay
---@param optional Single strength
---@param optional Boolean loop
function m:_play(finishCallback, delay, strength, loop) end
---public IEnumerator doShakes(Object finishCallback, Single delay, Single strength, Boolean loop)
---@return IEnumerator
---@param optional Object finishCallback
---@param optional Single delay
---@param optional Single strength
---@param optional Boolean loop
function m:doShakes(finishCallback, delay, strength, loop) end
---public Void doFinishCallback()
function m:doFinishCallback() end
---public Void onFinish()
function m:onFinish() end
Coolape.SScreenShakes = m
return m

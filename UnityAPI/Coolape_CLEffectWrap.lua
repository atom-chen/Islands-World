---@class Coolape.CLEffect : UnityEngine.MonoBehaviour
---@field public willFinishCallback System.Object
---@field public willFinishCallbackPara System.Object
---@field public finishCallback System.Object
---@field public finishCallbackPara System.Object
---@field public returnAuto System.Boolean
---@field public willFinishTimePercent System.Single
---@field public animationProc Coolape.AnimationProc
---@field public transform UnityEngine.Transform
---@field public particleSys UnityEngine.ParticleSystem
---@field public animators UnityEngine.Animator
---@field public animations UnityEngine.Animator

local m = { }
---public CLEffect .ctor()
---@return CLEffect
function m.New() end
---public IEnumerator playDelay(Vector3 pos, Transform parent, Single willFinishTimePercent, Object willFinishCallback, Object willFinishCallbackPara, Object finishCallback, Object finishCallbackPara, Single delaySec, Boolean returnAuto)
---public CLEffect playDelay(String name, Vector3 pos, Transform parent, Single willFinishTimePercent, Object willFinishCallback, Object willFinishCallbackPara, Object finishCallback, Object finishCallbackPara, Single delaySec, Boolean returnAuto)
---@return IEnumerator
---@param String name
---@param optional Vector3 pos
---@param optional Transform parent
---@param optional Single willFinishTimePercent
---@param optional Object willFinishCallback
---@param optional Object willFinishCallbackPara
---@param optional Object finishCallback
---@param optional Object finishCallbackPara
---@param optional Single delaySec
---@param optional Boolean returnAuto
function m:playDelay(name, pos, parent, willFinishTimePercent, willFinishCallback, willFinishCallbackPara, finishCallback, finishCallbackPara, delaySec, returnAuto) end
---public CLEffect play(String name, Vector3 pos)
---public CLEffect play(String name, Vector3 pos, Transform parent)
---public CLEffect play(String name, Vector3 pos, Object finishCallback, Object finishCallbackPara)
---public CLEffect play(String name, Vector3 pos, Transform parent, Object finishCallback, Object finishCallbackPara)
---public CLEffect play(String name, Vector3 pos, Transform parent, Single willFinishTimePercent, Object willFinishCallback, Object willFinishCallbackPara, Object finishCallback, Object finishCallbackPara, Boolean returnAuto)
---@return CLEffect
---@param String name
---@param Vector3 pos
---@param Transform parent
---@param Single willFinishTimePercent
---@param Object willFinishCallback
---@param Object willFinishCallbackPara
---@param Object finishCallback
---@param optional Object finishCallbackPara
---@param optional Boolean returnAuto
function m.play(name, pos, parent, willFinishTimePercent, willFinishCallback, willFinishCallbackPara, finishCallback, finishCallbackPara, returnAuto) end
---public Void onFinishSetPrefab(Object[] args)
---@param optional Object[] args
function m.onFinishSetPrefab(args) end
---public Void onFinishSetPrefab2(Object[] args)
---@param optional Object[] args
function m.onFinishSetPrefab2(args) end
---public Void show(Vector3 pos, Transform parent, Single willFinishTimePercent, Object willFinishCallback, Object willFinishCallbackPara, Object finishCallback, Object finishCallbackPara, Boolean returnAuto)
---@param optional Vector3 pos
---@param optional Transform parent
---@param optional Single willFinishTimePercent
---@param optional Object willFinishCallback
---@param optional Object willFinishCallbackPara
---@param optional Object finishCallback
---@param optional Object finishCallbackPara
---@param optional Boolean returnAuto
function m:show(pos, parent, willFinishTimePercent, willFinishCallback, willFinishCallbackPara, finishCallback, finishCallbackPara, returnAuto) end
---public Void onFinish(Object[] obj)
---@param optional Object[] obj
function m:onFinish(obj) end
---public Void Start()
function m:Start() end
---public Void pause()
function m:pause() end
---public Void regain()
function m:regain() end
---public Void playSC(Vector3 pos, Transform parent, Single willFinishTimePercent, Object willFinishCallback, Object willFinishCallbackPara, Object finishCallback, Object finishCallbackPara, Single delaySec, Boolean returnAuto)
---@param optional Vector3 pos
---@param optional Transform parent
---@param optional Single willFinishTimePercent
---@param optional Object willFinishCallback
---@param optional Object willFinishCallbackPara
---@param optional Object finishCallback
---@param optional Object finishCallbackPara
---@param optional Single delaySec
---@param optional Boolean returnAuto
function m:playSC(pos, parent, willFinishTimePercent, willFinishCallback, willFinishCallbackPara, finishCallback, finishCallbackPara, delaySec, returnAuto) end
Coolape.CLEffect = m
return m

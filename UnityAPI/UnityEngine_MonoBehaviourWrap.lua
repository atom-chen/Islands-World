---@class UnityEngine.MonoBehaviour : UnityEngine.Behaviour
---@field public useGUILayout System.Boolean
---@field public runInEditMode System.Boolean

local m = { }
---public MonoBehaviour .ctor()
---@return MonoBehaviour
function m.New() end
---public Boolean IsInvoking()
---public Boolean IsInvoking(String methodName)
---@return bool
---@param String methodName
function m:IsInvoking(methodName) end
---public Void CancelInvoke()
---public Void CancelInvoke(String methodName)
---@param String methodName
function m:CancelInvoke(methodName) end
---public Void Invoke(String methodName, Single time)
---@param optional String methodName
---@param optional Single time
function m:Invoke(methodName, time) end
---public Void InvokeRepeating(String methodName, Single time, Single repeatRate)
---@param optional String methodName
---@param optional Single time
---@param optional Single repeatRate
function m:InvokeRepeating(methodName, time, repeatRate) end
---public Coroutine StartCoroutine(String methodName)
---public Coroutine StartCoroutine(IEnumerator routine)
---public Coroutine StartCoroutine(String methodName, Object value)
---@return Coroutine
---@param String methodName
---@param optional Object value
function m:StartCoroutine(methodName, value) end
---public Void StopCoroutine(IEnumerator routine)
---public Void StopCoroutine(Coroutine routine)
---public Void StopCoroutine(String methodName)
---@param optional String methodName
function m:StopCoroutine(methodName) end
---public Void StopAllCoroutines()
function m:StopAllCoroutines() end
---public Void print(Object message)
---@param optional Object message
function m.print(message) end
UnityEngine.MonoBehaviour = m
return m

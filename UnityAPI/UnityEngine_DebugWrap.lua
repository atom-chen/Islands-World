---@class UnityEngine.Debug
---@field public unityLogger UnityEngine.ILogger
---@field public developerConsoleVisible System.Boolean
---@field public isDebugBuild System.Boolean

local m = { }
---public Debug .ctor()
---@return Debug
function m.New() end
---public Void DrawLine(Vector3 start, Vector3 ed)
---public Void DrawLine(Vector3 start, Vector3 ed, Color color)
---public Void DrawLine(Vector3 start, Vector3 ed, Color color, Single duration)
---public Void DrawLine(Vector3 start, Vector3 ed, Color color, Single duration, Boolean depthTest)
---@param Vector3 start
---@param Vector3 ed
---@param Color color
---@param optional Single duration
---@param optional Boolean depthTest
function m.DrawLine(start, end, color, duration, depthTest) end
---public Void DrawRay(Vector3 start, Vector3 dir)
---public Void DrawRay(Vector3 start, Vector3 dir, Color color)
---public Void DrawRay(Vector3 start, Vector3 dir, Color color, Single duration)
---public Void DrawRay(Vector3 start, Vector3 dir, Color color, Single duration, Boolean depthTest)
---@param Vector3 start
---@param Vector3 dir
---@param Color color
---@param optional Single duration
---@param optional Boolean depthTest
function m.DrawRay(start, dir, color, duration, depthTest) end
---public Void Break()
function m.Break() end
---public Void DebugBreak()
function m.DebugBreak() end
---public Void Log(Object message)
---public Void Log(Object message, Object context)
---@param Object message
---@param optional Object context
function m.Log(message, context) end
---public Void LogFormat(String format, Object[] args)
---public Void LogFormat(Object context, String format, Object[] args)
---@param Object context
---@param optional String format
---@param optional Object[] args
function m.LogFormat(context, format, args) end
---public Void LogError(Object message)
---public Void LogError(Object message, Object context)
---@param Object message
---@param optional Object context
function m.LogError(message, context) end
---public Void LogErrorFormat(String format, Object[] args)
---public Void LogErrorFormat(Object context, String format, Object[] args)
---@param Object context
---@param optional String format
---@param optional Object[] args
function m.LogErrorFormat(context, format, args) end
---public Void ClearDeveloperConsole()
function m.ClearDeveloperConsole() end
---public Void LogException(Exception exception)
---public Void LogException(Exception exception, Object context)
---@param Exception exception
---@param optional Object context
function m.LogException(exception, context) end
---public Void LogWarning(Object message)
---public Void LogWarning(Object message, Object context)
---@param Object message
---@param optional Object context
function m.LogWarning(message, context) end
---public Void LogWarningFormat(String format, Object[] args)
---public Void LogWarningFormat(Object context, String format, Object[] args)
---@param Object context
---@param optional String format
---@param optional Object[] args
function m.LogWarningFormat(context, format, args) end
---public Void Assert(Boolean condition)
---public Void Assert(Boolean condition, String message)
---public Void Assert(Boolean condition, Object message)
---public Void Assert(Boolean condition, Object context)
---public Void Assert(Boolean condition, String message, Object context)
---public Void Assert(Boolean condition, Object message, Object context)
---@param Boolean condition
---@param Object message
---@param optional Object context
function m.Assert(condition, message, context) end
---public Void AssertFormat(Boolean condition, String format, Object[] args)
---public Void AssertFormat(Boolean condition, Object context, String format, Object[] args)
---@param Boolean condition
---@param optional Object context
---@param optional String format
---@param optional Object[] args
function m.AssertFormat(condition, context, format, args) end
---public Void LogAssertion(Object message)
---public Void LogAssertion(Object message, Object context)
---@param Object message
---@param optional Object context
function m.LogAssertion(message, context) end
---public Void LogAssertionFormat(String format, Object[] args)
---public Void LogAssertionFormat(Object context, String format, Object[] args)
---@param Object context
---@param optional String format
---@param optional Object[] args
function m.LogAssertionFormat(context, format, args) end
UnityEngine.Debug = m
return m

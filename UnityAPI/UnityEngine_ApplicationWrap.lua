---@class UnityEngine.Application
---@field public isPlaying System.Boolean
---@field public isFocused System.Boolean
---@field public platform UnityEngine.RuntimePlatform
---@field public buildGUID System.String
---@field public isMobilePlatform System.Boolean
---@field public isConsolePlatform System.Boolean
---@field public runInBackground System.Boolean
---@field public isBatchMode System.Boolean
---@field public dataPath System.String
---@field public streamingAssetsPath System.String
---@field public persistentDataPath System.String
---@field public temporaryCachePath System.String
---@field public absoluteURL System.String
---@field public unityVersion System.String
---@field public version System.String
---@field public installerName System.String
---@field public identifier System.String
---@field public installMode UnityEngine.ApplicationInstallMode
---@field public sandboxType UnityEngine.ApplicationSandboxType
---@field public productName System.String
---@field public companyName System.String
---@field public cloudProjectId System.String
---@field public targetFrameRate System.Int32
---@field public systemLanguage UnityEngine.SystemLanguage
---@field public consoleLogPath System.String
---@field public backgroundLoadingPriority UnityEngine.ThreadPriority
---@field public internetReachability UnityEngine.NetworkReachability
---@field public genuine System.Boolean
---@field public genuineCheckAvailable System.Boolean
---@field public isEditor System.Boolean
local m = { }
---public Application .ctor()
---@return Application
function m.New() end
---public Void Quit()
---public Void Quit(Int32 exitCode)
---@param Int32 exitCode
function m.Quit(exitCode) end
---public Void Unload()
function m.Unload() end
---public Boolean CanStreamedLevelBeLoaded(Int32 levelIndex)
---public Boolean CanStreamedLevelBeLoaded(String levelName)
---@return bool
---@param optional String levelName
function m.CanStreamedLevelBeLoaded(levelName) end
---public Boolean IsPlaying(Object obj)
---@return bool
---@param optional Object obj
function m.IsPlaying(obj) end
---public String[] GetBuildTags()
---@return table
function m.GetBuildTags() end
---public Void SetBuildTags(String[] buildTags)
---@param optional String[] buildTags
function m.SetBuildTags(buildTags) end
---public Boolean HasProLicense()
---@return bool
function m.HasProLicense() end
---public Boolean RequestAdvertisingIdentifierAsync(AdvertisingIdentifierCallback delegateMethod)
---@return bool
---@param optional AdvertisingIdentifierCallback delegateMethod
function m.RequestAdvertisingIdentifierAsync(delegateMethod) end
---public Void OpenURL(String url)
---@param optional String url
function m.OpenURL(url) end
---public StackTraceLogType GetStackTraceLogType(LogType logType)
---@return number
---@param optional LogType logType
function m.GetStackTraceLogType(logType) end
---public Void SetStackTraceLogType(LogType logType, StackTraceLogType stackTraceType)
---@param optional LogType logType
---@param optional StackTraceLogType stackTraceType
function m.SetStackTraceLogType(logType, stackTraceType) end
---public AsyncOperation RequestUserAuthorization(UserAuthorization mode)
---@return AsyncOperation
---@param optional UserAuthorization mode
function m.RequestUserAuthorization(mode) end
---public Boolean HasUserAuthorization(UserAuthorization mode)
---@return bool
---@param optional UserAuthorization mode
function m.HasUserAuthorization(mode) end
---public Void add_lowMemory(LowMemoryCallback value)
---@param optional LowMemoryCallback value
function m.add_lowMemory(value) end
---public Void remove_lowMemory(LowMemoryCallback value)
---@param optional LowMemoryCallback value
function m.remove_lowMemory(value) end
---public Void add_logMessageReceived(LogCallback value)
---@param optional LogCallback value
function m.add_logMessageReceived(value) end
---public Void remove_logMessageReceived(LogCallback value)
---@param optional LogCallback value
function m.remove_logMessageReceived(value) end
---public Void add_logMessageReceivedThreaded(LogCallback value)
---@param optional LogCallback value
function m.add_logMessageReceivedThreaded(value) end
---public Void remove_logMessageReceivedThreaded(LogCallback value)
---@param optional LogCallback value
function m.remove_logMessageReceivedThreaded(value) end
---public Void add_onBeforeRender(UnityAction value)
---@param optional UnityAction value
function m.add_onBeforeRender(value) end
---public Void remove_onBeforeRender(UnityAction value)
---@param optional UnityAction value
function m.remove_onBeforeRender(value) end
---public Void add_focusChanged(Action`1 value)
---@param optional Action`1 value
function m.add_focusChanged(value) end
---public Void remove_focusChanged(Action`1 value)
---@param optional Action`1 value
function m.remove_focusChanged(value) end
---public Void add_deepLinkActivated(Action`1 value)
---@param optional Action`1 value
function m.add_deepLinkActivated(value) end
---public Void remove_deepLinkActivated(Action`1 value)
---@param optional Action`1 value
function m.remove_deepLinkActivated(value) end
---public Void add_wantsToQuit(Func`1 value)
---@param optional Func`1 value
function m.add_wantsToQuit(value) end
---public Void remove_wantsToQuit(Func`1 value)
---@param optional Func`1 value
function m.remove_wantsToQuit(value) end
---public Void add_quitting(Action value)
---@param optional Action value
function m.add_quitting(value) end
---public Void remove_quitting(Action value)
---@param optional Action value
function m.remove_quitting(value) end
UnityEngine.Application = m
return m

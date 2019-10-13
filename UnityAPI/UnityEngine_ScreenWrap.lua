---@class UnityEngine.Screen
---@field public width System.Int32
---@field public height System.Int32
---@field public dpi System.Single
---@field public orientation UnityEngine.ScreenOrientation
---@field public sleepTimeout System.Int32
---@field public autorotateToPortrait System.Boolean
---@field public autorotateToPortraitUpsideDown System.Boolean
---@field public autorotateToLandscapeLeft System.Boolean
---@field public autorotateToLandscapeRight System.Boolean
---@field public currentResolution UnityEngine.Resolution
---@field public fullScreen System.Boolean
---@field public fullScreenMode UnityEngine.FullScreenMode
---@field public safeArea UnityEngine.Rect
---@field public cutouts UnityEngine.Rect
---@field public resolutions UnityEngine.Resolution
---@field public brightness System.Single
local m = { }
---public Screen .ctor()
---@return Screen
function m.New() end
---public Void SetResolution(Int32 width, Int32 height, FullScreenMode fullscreenMode)
---public Void SetResolution(Int32 width, Int32 height, Boolean fullscreen)
---public Void SetResolution(Int32 width, Int32 height, FullScreenMode fullscreenMode, Int32 preferredRefreshRate)
---public Void SetResolution(Int32 width, Int32 height, Boolean fullscreen, Int32 preferredRefreshRate)
---@param Int32 width
---@param optional Int32 height
---@param optional Boolean fullscreen
---@param optional Int32 preferredRefreshRate
function m.SetResolution(width, height, fullscreen, preferredRefreshRate) end
UnityEngine.Screen = m
return m

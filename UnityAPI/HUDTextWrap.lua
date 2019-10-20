---@class HUDText : UnityEngine.MonoBehaviour
---@field public fontName System.String
---@field public font UIFont
---@field public effect UILabel.Effect
---@field public inactiveWhenFinish System.Boolean
---@field public gradient System.Boolean
---@field public fontStyle UnityEngine.FontStyle
---@field public spacingX System.Int32
---@field public needAddValue System.Boolean
---@field public needQueue System.Boolean
---@field public scaleOffset System.Single
---@field public speed System.Single
---@field public hightOffset System.Single
---@field public scaleCurve UnityEngine.AnimationCurve
---@field public offsetCurve UnityEngine.AnimationCurve
---@field public alphaCurve UnityEngine.AnimationCurve
---@field public ignoreTimeScale System.Boolean
---@field public isVisible System.Boolean
local m = { }
---public HUDText .ctor()
---@return HUDText
function m.New() end
---public Void init(Int32 num)
---@param optional Int32 num
function m:init(num) end
---public UILabel Add(Object obj, Color c, Single stayDuration, Single scaleOffset)
---@return UILabel
---@param optional Object obj
---@param optional Color c
---@param optional Single stayDuration
---@param optional Single scaleOffset
function m:Add(obj, c, stayDuration, scaleOffset) end
HUDText = m
return m

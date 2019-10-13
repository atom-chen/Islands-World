---@class UITexture : UIBasicSprite
---@field public mainTexture UnityEngine.Texture
---@field public material UnityEngine.Material
---@field public shader UnityEngine.Shader
---@field public premultipliedAlpha System.Boolean
---@field public border UnityEngine.Vector4
---@field public uvRect UnityEngine.Rect
---@field public drawingDimensions UnityEngine.Vector4
---@field public fixedAspect System.Boolean
local m = { }
---public UITexture .ctor()
---@return UITexture
function m.New() end
---public Void MakePixelPerfect()
function m:MakePixelPerfect() end
---public Void OnFill(BetterList`1 verts, BetterList`1 uvs, BetterList`1 cols)
---@param optional BetterList`1 verts
---@param optional BetterList`1 uvs
---@param optional BetterList`1 cols
function m:OnFill(verts, uvs, cols) end
UITexture = m
return m

---@class UISprite : UIBasicSprite
---@field public atlasName System.String
---@field public grayMatMap System.Collections.Hashtable
---@field public material UnityEngine.Material
---@field public grayMaterial UnityEngine.Material
---@field public atlas UIAtlas
---@field public spriteName System.String
---@field public isValid System.Boolean
---@field public border UnityEngine.Vector4
---@field public pixelSize System.Single
---@field public minWidth System.Int32
---@field public minHeight System.Int32
---@field public drawingDimensions UnityEngine.Vector4
---@field public premultipliedAlpha System.Boolean
local m = { }
---public UISprite .ctor()
---@return UISprite
function m.New() end
---public Void refresh()
function m:refresh() end
---public Void setGray()
function m:setGray() end
---public Void unSetGray()
function m:unSetGray() end
---public UISpriteData GetAtlasSprite1()
---@return UISpriteData
function m:GetAtlasSprite1() end
---public UISpriteData GetAtlasSprite2()
---@return UISpriteData
function m:GetAtlasSprite2() end
---public UISpriteData GetAtlasSprite()
---@return UISpriteData
function m:GetAtlasSprite() end
---public Void MakePixelPerfect()
function m:MakePixelPerfect() end
---public Void OnFill(BetterList`1 verts, BetterList`1 uvs, BetterList`1 cols)
---@param optional BetterList`1 verts
---@param optional BetterList`1 uvs
---@param optional BetterList`1 cols
function m:OnFill(verts, uvs, cols) end
UISprite = m
return m

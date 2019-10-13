---@class UIAtlas : UnityEngine.MonoBehaviour
---@field public retainCounter System.Collections.Hashtable
---@field public assetBundleMap System.Collections.Hashtable
---@field public materailPool System.Collections.Hashtable
---@field public onBorrowSpriteCallback System.Object
---@field public releaseSpriteTime System.Int32
---@field public isBorrowSpriteMode System.Boolean
---@field public spriteMap System.Collections.Hashtable
---@field public spriteMaterial UnityEngine.Material
---@field public premultipliedAlpha System.Boolean
---@field public spriteList System.Collections.Generic.List1UISpriteData
---@field public texture UnityEngine.Texture
---@field public pixelSize System.Single
---@field public replacement UIAtlas
local m = { }
---public UIAtlas .ctor()
---@return UIAtlas
function m.New() end
---public Void init()
function m:init() end
---public Void returnSpriteByname(String name)
---@param optional String name
function m:returnSpriteByname(name) end
---public Void releaseAllTexturesImm()
function m:releaseAllTexturesImm() end
---public Void doReleaseTexture(String name)
---public Void doReleaseTexture(String name, Boolean force)
---@param String name
---@param optional Boolean force
function m:doReleaseTexture(name, force) end
---public Boolean hadGetSpriteTexture(String spriteName)
---@return bool
---@param optional String spriteName
function m:hadGetSpriteTexture(spriteName) end
---public UISpriteData getSpriteBorrowMode(String name)
---@return UISpriteData
---@param optional String name
function m:getSpriteBorrowMode(name) end
---public UISpriteData borrowSpriteByname(String name, UISprite uisp)
---public UISpriteData borrowSpriteByname(String name, UISprite uisp, Object callback)
---public UISpriteData borrowSpriteByname(String name, UISprite uisp, Object callback, Object args)
---@return UISpriteData
---@param String name
---@param UISprite uisp
---@param optional Object callback
---@param optional Object args
function m:borrowSpriteByname(name, uisp, callback, args) end
---public Material getMaterail(Texture tt, String texturePath)
---@return Material
---@param optional Texture tt
---@param optional String texturePath
function m:getMaterail(tt, texturePath) end
---public UISpriteData GetSprite(String name)
---@return UISpriteData
---@param optional String name
function m:GetSprite(name) end
---public String GetRandomSprite(String startsWith)
---@return String
---@param optional String startsWith
function m:GetRandomSprite(startsWith) end
---public Void MarkSpriteListAsChanged()
function m:MarkSpriteListAsChanged() end
---public Void SortAlphabetically()
function m:SortAlphabetically() end
---public BetterList`1 GetListOfSprites()
---public BetterList`1 GetListOfSprites(String match)
---@return BetterList`1
---@param String match
function m:GetListOfSprites(match) end
---public Boolean CheckIfRelated(UIAtlas a, UIAtlas b)
---@return bool
---@param optional UIAtlas a
---@param optional UIAtlas b
function m.CheckIfRelated(a, b) end
---public Void MarkAsChanged()
function m:MarkAsChanged() end
UIAtlas = m
return m

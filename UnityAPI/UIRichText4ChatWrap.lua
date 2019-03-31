---@class UIRichText4Chat : UnityEngine.MonoBehaviour
---@field public _label UILabel
---@field public faceAtlas UIAtlas
---@field public atlasName System.String
---@field public faceSize System.Int32
---@field public faceScaleOffset System.Single
---@field public faceHead System.String
---@field public isFullSpace System.Boolean
---@field public spaceSize System.Single
---@field public _FaceChar_ System.String
---@field public spaceNumber System.Int32
---@field public faceStr System.String
---@field public pool UIRichText4Chat.SpritePool
---@field public spList System.Collections.Generic.List1UISprite
---@field public label UILabel
---@field public value System.String

local m = { }
---public UIRichText4Chat .ctor()
---@return UIRichText4Chat
function m.New() end
---public Void init()
function m:init() end
---public Void calculateSpaceSize()
function m:calculateSpaceSize() end
---public String wrapFaceName(String faceName)
---@return String
---@param optional String faceName
function m:wrapFaceName(faceName) end
---public Void onInputChanged(GameObject go)
---@param optional GameObject go
function m:onInputChanged(go) end
---public Void onTextChanged(GameObject go)
---@param optional GameObject go
function m:onTextChanged(go) end
---public Vector3 calculatePos(Vector3 pos1, Vector3 pos2)
---@return Vector3
---@param optional Vector3 pos1
---@param optional Vector3 pos2
function m:calculatePos(pos1, pos2) end
---public Void showFace(String faceName, Vector3 pos)
---@param optional String faceName
---@param optional Vector3 pos
function m:showFace(faceName, pos) end
---public String findFace()
---@return String
function m:findFace() end
---public Void clean()
function m:clean() end
---public Void OnDisable()
function m:OnDisable() end
---public Void OnEnable()
function m:OnEnable() end
UIRichText4Chat = m
return m

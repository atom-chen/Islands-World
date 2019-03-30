---@class Coolape.CLUIDrag4World : UnityEngine.MonoBehaviour
---@field public self Coolape.CLUIDrag4World
---@field public onDragMoveDelegate System.Object
---@field public onDragScaleDelegate System.Object
---@field public onEndDragMoveDelegate System.Object
---@field public main3DCamera Coolape.MyMainCamera
---@field public target UnityEngine.Transform
---@field public scaleTarget Coolape.CLSmoothFollow
---@field public canMove System.Boolean
---@field public canRotation System.Boolean
---@field public canRotationOneTouch System.Boolean
---@field public canScale System.Boolean
---@field public canDoInertance System.Boolean
---@field public isLimitCheckStrict System.Boolean
---@field public scrollMomentum UnityEngine.Vector3
---@field public dragEffect Coolape.CLUIDrag4World.DragEffect
---@field public momentumAmount System.Single
---@field public groundMask UnityEngine.LayerMask
---@field public viewCenter UnityEngine.Vector3
---@field public viewRadius System.Single
---@field public rotationMini System.Single
---@field public rotationMax System.Single
---@field public scaleMini System.Single
---@field public scaleMax System.Single
---@field public scaleHeightMini System.Single
---@field public scaleHeightMax System.Single
---@field public rotateSpeed System.Single
---@field public scaleSpeed System.Single
---@field public inertanceSpeed System.Single
---@field public dragMovement UnityEngine.Vector3
---@field public _scaleValue System.Single

local m = { }
---public CLUIDrag4World .ctor()
---@return CLUIDrag4World
function m.New() end
---public Void setCanClickPanel(String pName)
---@param optional String pName
function m.setCanClickPanel(pName) end
---public Void removeCanClickPanel(String pName)
---@param optional String pName
function m.removeCanClickPanel(pName) end
---public Void OnDrag(Vector2 delta)
---@param optional Vector2 delta
function m:OnDrag(delta) end
---public Void doInertance()
function m:doInertance() end
---public Void unDoInertance()
function m:unDoInertance() end
---public Single getAngle(Vector3 dir)
---@return number
---@param optional Vector3 dir
function m:getAngle(dir) end
---public Void procScaler(Single delta)
---@param optional Single delta
function m:procScaler(delta) end
---public Boolean limitDisplayView()
---public Boolean limitDisplayView(Vector3& offset)
---@return bool
---@param Vector3& offset
function m:limitDisplayView(offset) end
---public Void CancelMovement()
function m:CancelMovement() end
---public Void CancelSpring()
function m:CancelSpring() end
Coolape.CLUIDrag4World = m
return m

---@class UIWidget : UIRect
---@field public onChange UIWidget.OnDimensionsChanged
---@field public onPostFill UIWidget.OnPostFillCallback
---@field public mOnRender UIDrawCall.OnRenderCallback
---@field public autoResizeBoxCollider System.Boolean
---@field public hideIfOffScreen System.Boolean
---@field public keepAspectRatio UIWidget.AspectRatioSource
---@field public aspectRatio System.Single
---@field public hitCheck UIWidget.HitCheck
---@field public panel UIPanel
---@field public geometry UIGeometry
---@field public fillGeometry System.Boolean
---@field public drawCall UIDrawCall
---@field public onRender UIDrawCall.OnRenderCallback
---@field public drawRegion UnityEngine.Vector4
---@field public pivotOffset UnityEngine.Vector2
---@field public width System.Int32
---@field public height System.Int32
---@field public color UnityEngine.Color
---@field public alpha System.Single
---@field public isVisible System.Boolean
---@field public hasVertices System.Boolean
---@field public rawPivot UIWidget.Pivot
---@field public pivot UIWidget.Pivot
---@field public depth System.Int32
---@field public raycastDepth System.Int32
---@field public localCorners UnityEngine.Vector3
---@field public localSize UnityEngine.Vector2
---@field public localCenter UnityEngine.Vector3
---@field public worldCorners UnityEngine.Vector3
---@field public worldCenter UnityEngine.Vector3
---@field public drawingDimensions UnityEngine.Vector4
---@field public material UnityEngine.Material
---@field public mainTexture UnityEngine.Texture
---@field public shader UnityEngine.Shader
---@field public hasBoxCollider System.Boolean
---@field public showHandlesWithMoveTool System.Boolean
---@field public showHandles System.Boolean
---@field public minWidth System.Int32
---@field public minHeight System.Int32
---@field public border UnityEngine.Vector4

local m = { }
---public UIWidget .ctor()
---@return UIWidget
function m.New() end
---public Void SetDimensions(Int32 w, Int32 h)
---@param optional Int32 w
---@param optional Int32 h
function m:SetDimensions(w, h) end
---public Vector3[] GetSides(Transform relativeTo)
---@return table
---@param optional Transform relativeTo
function m:GetSides(relativeTo) end
---public Single CalculateFinalAlpha(Int32 frameID)
---@return number
---@param optional Int32 frameID
function m:CalculateFinalAlpha(frameID) end
---public Void Invalidate(Boolean includeChildren)
---@param optional Boolean includeChildren
function m:Invalidate(includeChildren) end
---public Single CalculateCumulativeAlpha(Int32 frameID)
---@return number
---@param optional Int32 frameID
function m:CalculateCumulativeAlpha(frameID) end
---public Void SetRect(Single x, Single y, Single width, Single height)
---@param optional Single x
---@param optional Single y
---@param optional Single width
---@param optional Single height
function m:SetRect(x, y, width, height) end
---public Void ResizeCollider()
function m:ResizeCollider() end
---public Int32 FullCompareFunc(UIWidget left, UIWidget right)
---@return number
---@param optional UIWidget left
---@param optional UIWidget right
function m.FullCompareFunc(left, right) end
---public Int32 PanelCompareFunc(UIWidget left, UIWidget right)
---@return number
---@param optional UIWidget left
---@param optional UIWidget right
function m.PanelCompareFunc(left, right) end
---public Bounds CalculateBounds()
---public Bounds CalculateBounds(Transform relativeParent)
---@return Bounds
---@param Transform relativeParent
function m:CalculateBounds(relativeParent) end
---public Void SetDirty()
function m:SetDirty() end
---public Void RemoveFromPanel()
function m:RemoveFromPanel() end
---public Void MarkAsChanged()
function m:MarkAsChanged() end
---public UIPanel CreatePanel()
---@return UIPanel
function m:CreatePanel() end
---public Void CheckLayer()
function m:CheckLayer() end
---public Void ParentHasChanged()
function m:ParentHasChanged() end
---public Boolean UpdateVisibility(Boolean visibleByAlpha, Boolean visibleByPanel)
---@return bool
---@param optional Boolean visibleByAlpha
---@param optional Boolean visibleByPanel
function m:UpdateVisibility(visibleByAlpha, visibleByPanel) end
---public Boolean UpdateTransform(Int32 frame)
---@return bool
---@param optional Int32 frame
function m:UpdateTransform(frame) end
---public Boolean UpdateGeometry(Int32 frame)
---@return bool
---@param optional Int32 frame
function m:UpdateGeometry(frame) end
---public Void WriteToBuffers(BetterList`1 v, BetterList`1 u, BetterList`1 c, BetterList`1 n, BetterList`1 t)
---@param optional BetterList`1 v
---@param optional BetterList`1 u
---@param optional BetterList`1 c
---@param optional BetterList`1 n
---@param optional BetterList`1 t
function m:WriteToBuffers(v, u, c, n, t) end
---public Void MakePixelPerfect()
function m:MakePixelPerfect() end
---public Void OnFill(BetterList`1 verts, BetterList`1 uvs, BetterList`1 cols)
---@param optional BetterList`1 verts
---@param optional BetterList`1 uvs
---@param optional BetterList`1 cols
function m:OnFill(verts, uvs, cols) end
UIWidget = m
return m

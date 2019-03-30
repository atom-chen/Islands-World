---@class NGUITools
---@field public soundVolume System.Single
---@field public fileAccess System.Boolean
---@field public clipboard System.String
---@field public screenSize UnityEngine.Vector2
---@field public isFringe System.Boolean
---@field public rateFringe System.Single

local m = { }
---public AudioSource PlaySound(AudioClip clip)
---public AudioSource PlaySound(AudioClip clip, Single volume)
---public AudioSource PlaySound(AudioClip clip, Single volume, Single pitch)
---@return AudioSource
---@param AudioClip clip
---@param Single volume
---@param optional Single pitch
function m.PlaySound(clip, volume, pitch) end
---public Int32 RandomRange(Int32 min, Int32 max)
---@return number
---@param optional Int32 min
---@param optional Int32 max
function m.RandomRange(min, max) end
---public String GetHierarchy(GameObject obj)
---@return String
---@param optional GameObject obj
function m.GetHierarchy(obj) end
---public Camera FindCameraForLayer(Int32 layer)
---@return Camera
---@param optional Int32 layer
function m.FindCameraForLayer(layer) end
---public Void AddWidgetCollider(GameObject go)
---public Void AddWidgetCollider(GameObject go, Boolean considerInactive)
---@param GameObject go
---@param optional Boolean considerInactive
function m.AddWidgetCollider(go, considerInactive) end
---public Void UpdateWidgetCollider(GameObject go)
---public Void UpdateWidgetCollider(BoxCollider2D box, Boolean considerInactive)
---public Void UpdateWidgetCollider(BoxCollider box, Boolean considerInactive)
---public Void UpdateWidgetCollider(GameObject go, Boolean considerInactive)
---@param GameObject go
---@param optional Boolean considerInactive
function m.UpdateWidgetCollider(go, considerInactive) end
---public String GetTypeName(Object obj)
---@return String
---@param optional Object obj
function m.GetTypeName(obj) end
---public Void RegisterUndo(Object obj, String name)
---@param optional Object obj
---@param optional String name
function m.RegisterUndo(obj, name) end
---public Void SetDirty(Object obj)
---@param optional Object obj
function m.SetDirty(obj) end
---public GameObject AddChild(GameObject parent)
---public GameObject AddChild(GameObject parent, GameObject prefab)
---public GameObject AddChild(GameObject parent, Boolean undo)
---@return GameObject
---@param GameObject parent
---@param optional Boolean undo
function m.AddChild(parent, undo) end
---public Int32 CalculateRaycastDepth(GameObject go)
---@return number
---@param optional GameObject go
function m.CalculateRaycastDepth(go) end
---public Int32 CalculateNextDepth(GameObject go)
---public Int32 CalculateNextDepth(GameObject go, Boolean ignoreChildrenWithColliders)
---@return number
---@param GameObject go
---@param optional Boolean ignoreChildrenWithColliders
function m.CalculateNextDepth(go, ignoreChildrenWithColliders) end
---public Int32 AdjustDepth(GameObject go, Int32 adjustment)
---@return number
---@param optional GameObject go
---@param optional Int32 adjustment
function m.AdjustDepth(go, adjustment) end
---public Void BringForward(GameObject go)
---@param optional GameObject go
function m.BringForward(go) end
---public Void PushBack(GameObject go)
---@param optional GameObject go
function m.PushBack(go) end
---public Void NormalizeDepths()
function m.NormalizeDepths() end
---public Void NormalizeWidgetDepths()
---public Void NormalizeWidgetDepths(UIWidget[] list)
---public Void NormalizeWidgetDepths(GameObject go)
---@param GameObject go
function m.NormalizeWidgetDepths(go) end
---public Void NormalizePanelDepths()
function m.NormalizePanelDepths() end
---public UIPanel CreateUI(Boolean advanced3D)
---public UIPanel CreateUI(Boolean advanced3D, Int32 layer)
---public UIPanel CreateUI(Transform trans, Boolean advanced3D, Int32 layer)
---@return UIPanel
---@param Transform trans
---@param Boolean advanced3D
---@param optional Int32 layer
function m.CreateUI(trans, advanced3D, layer) end
---public Void SetChildLayer(Transform t, Int32 layer)
---@param optional Transform t
---@param optional Int32 layer
function m.SetChildLayer(t, layer) end
---public UISprite AddSprite(GameObject go, UIAtlas atlas, String spriteName)
---@return UISprite
---@param optional GameObject go
---@param optional UIAtlas atlas
---@param optional String spriteName
function m.AddSprite(go, atlas, spriteName) end
---public GameObject GetRoot(GameObject go)
---@return GameObject
---@param optional GameObject go
function m.GetRoot(go) end
---public Void Destroy(Object obj)
---@param optional Object obj
function m.Destroy(obj) end
---public Void DestroyImmediate(Object obj)
---@param optional Object obj
function m.DestroyImmediate(obj) end
---public Void Broadcast(String funcName)
---public Void Broadcast(String funcName, Object param)
---@param String funcName
---@param optional Object param
function m.Broadcast(funcName, param) end
---public Boolean IsChild(Transform parent, Transform child)
---@return bool
---@param optional Transform parent
---@param optional Transform child
function m.IsChild(parent, child) end
---public Void SetActive(GameObject go, Boolean state)
---public Void SetActive(GameObject go, Boolean state, Boolean compatibilityMode)
---@param GameObject go
---@param optional Boolean state
---@param optional Boolean compatibilityMode
function m.SetActive(go, state, compatibilityMode) end
---public Void SetActiveChildren(GameObject go, Boolean state)
---@param optional GameObject go
---@param optional Boolean state
function m.SetActiveChildren(go, state) end
---public Boolean GetActive(GameObject go)
---public Boolean GetActive(Behaviour mb)
---@return bool
---@param optional Behaviour mb
function m.GetActive(mb) end
---public Void SetActiveSelf(GameObject go, Boolean state)
---@param optional GameObject go
---@param optional Boolean state
function m.SetActiveSelf(go, state) end
---public Void SetLayer(GameObject go, Int32 layer)
---@param optional GameObject go
---@param optional Int32 layer
function m.SetLayer(go, layer) end
---public Vector3 Round(Vector3 v)
---@return Vector3
---@param optional Vector3 v
function m.Round(v) end
---public Void MakePixelPerfect(Transform t)
---@param optional Transform t
function m.MakePixelPerfect(t) end
---public Boolean Save(String fileName, Byte[] bytes)
---@return bool
---@param optional String fileName
---@param optional Byte[] bytes
function m.Save(fileName, bytes) end
---public Byte[] Load(String fileName)
---@return table
---@param optional String fileName
function m.Load(fileName) end
---public Color ApplyPMA(Color c)
---@return Color
---@param optional Color c
function m.ApplyPMA(c) end
---public Void MarkParentAsChanged(GameObject go)
---@param optional GameObject go
function m.MarkParentAsChanged(go) end
---public Vector3[] GetSides(Camera cam)
---public Vector3[] GetSides(Camera cam, Transform relativeTo)
---public Vector3[] GetSides(Camera cam, Single depth)
---public Vector3[] GetSides(Camera cam, Single depth, Transform relativeTo)
---@return table
---@param Camera cam
---@param Single depth
---@param optional Transform relativeTo
function m.GetSides(cam, depth, relativeTo) end
---public Vector3[] GetWorldCorners(Camera cam)
---public Vector3[] GetWorldCorners(Camera cam, Transform relativeTo)
---public Vector3[] GetWorldCorners(Camera cam, Single depth)
---public Vector3[] GetWorldCorners(Camera cam, Single depth, Transform relativeTo)
---@return table
---@param Camera cam
---@param Single depth
---@param optional Transform relativeTo
function m.GetWorldCorners(cam, depth, relativeTo) end
---public String GetFuncName(Object obj, String method)
---@return String
---@param optional Object obj
---@param optional String method
function m.GetFuncName(obj, method) end
---public Void ImmediatelyCreateDrawCalls(GameObject root)
---@param optional GameObject root
function m.ImmediatelyCreateDrawCalls(root) end
---public Void updateAll(Transform tr)
---@param optional Transform tr
function m.updateAll(tr) end
---public Boolean isIphonex()
---@return bool
function m.isIphonex() end
---public Rect wrapRect4IphoneX(Rect rect)
---@return Rect
---@param optional Rect rect
function m.wrapRect4IphoneX(rect) end
---public Rect wrapRect4Fringe(Rect rect)
---@return Rect
---@param optional Rect rect
function m.wrapRect4Fringe(rect) end
NGUITools = m
return m

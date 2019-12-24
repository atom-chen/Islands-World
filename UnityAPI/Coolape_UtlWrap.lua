---@class Coolape.Utl
---@field public kXAxis UnityEngine.Vector3
---@field public kZAxis UnityEngine.Vector3
---@field public uuid System.String
local m = { }
---public String GetMacAddress()
---@return String
function m.GetMacAddress() end
---public Hashtable vector2ToMap(Vector2 v2)
---@return Hashtable
---@param optional Vector2 v2
function m.vector2ToMap(v2) end
---public Hashtable vector3ToMap(Vector3 v3)
---@return Hashtable
---@param optional Vector3 v3
function m.vector3ToMap(v3) end
---public Hashtable vector4ToMap(Vector4 v4)
---@return Hashtable
---@param optional Vector4 v4
function m.vector4ToMap(v4) end
---public Vector2 mapToVector2(Hashtable map)
---@return Vector2
---@param optional Hashtable map
function m.mapToVector2(map) end
---public Vector3 mapToVector3(Hashtable map)
---@return Vector3
---@param optional Hashtable map
function m.mapToVector3(map) end
---public Hashtable colorToMap(Color color)
---@return Hashtable
---@param optional Color color
function m.colorToMap(color) end
---public Color mapToColor(Hashtable map)
---@return Color
---@param optional Hashtable map
function m.mapToColor(map) end
---public String filterPath(String path)
---@return String
---@param optional String path
function m.filterPath(path) end
---public AnimationCurve getAnimationCurve(ArrayList list, WrapMode postWrapMode, WrapMode preWrapMode)
---@return AnimationCurve
---@param optional ArrayList list
---@param optional WrapMode postWrapMode
---@param optional WrapMode preWrapMode
function m.getAnimationCurve(list, postWrapMode, preWrapMode) end
---public Void rotateTowardsForecast(Transform trsf, Transform target, Single forecastDis)
---@param optional Transform trsf
---@param optional Transform target
---@param optional Single forecastDis
function m.rotateTowardsForecast(trsf, target, forecastDis) end
---public Void RotateTowards(Transform trsf, Vector3 dir)
---public Void RotateTowards(Transform trsf, Vector3 from, Vector3 to)
---public Void RotateTowards(Transform transform, Vector3 dir, Single turningSpeed)
---@param Transform transform
---@param optional Vector3 dir
---@param optional Single turningSpeed
function m.RotateTowards(transform, dir, turningSpeed) end
---public Vector3 getAngle(Vector3 dir)
---public Vector3 getAngle(Vector3 pos1, Vector3 pos2)
---public Vector3 getAngle(Transform tr, Vector3 pos2)
---@return Vector3
---@param Transform tr
---@param optional Vector3 pos2
function m.getAngle(tr, pos2) end
---public Void setBodyMatEdit(Transform tr)
---public Void setBodyMatEdit(Transform tr, Shader defaultShader)
---@param Transform tr
---@param optional Shader defaultShader
function m.setBodyMatEdit(tr, defaultShader) end
---public Single distance(Transform tr1, Transform tr2)
---public Single distance(Vector2 v1, Vector2 v2)
---public Single distance(Vector3 v1, Vector3 v2)
---@return number
---@param optional Vector3 v1
---@param optional Vector3 v2
function m.distance(v1, v2) end
---public Single distance4Loc(Transform tr1, Transform tr2)
---@return number
---@param optional Transform tr1
---@param optional Transform tr2
function m.distance4Loc(tr1, tr2) end
---public String LuaTableToString(LuaTable map)
---public Void LuaTableToString(LuaTable map, StringBuilder outstr, Int32 spacecount)
---@return String
---@param LuaTable map
---@param StringBuilder outstr
---@param optional Int32 spacecount
function m.LuaTableToString(map, outstr, spacecount) end
---public String MapToString(Hashtable map)
---public Void MapToString(Hashtable map, StringBuilder outstr, Int32 spacecount)
---@return String
---@param Hashtable map
---@param StringBuilder outstr
---@param optional Int32 spacecount
function m.MapToString(map, outstr, spacecount) end
---public String ArrayListToString2(ArrayList list)
---@return String
---@param optional ArrayList list
function m.ArrayListToString2(list) end
---public Void ArrayListToString(ArrayList list, StringBuilder outstr, Int32 spacecount)
---@param optional ArrayList list
---@param optional StringBuilder outstr
---@param optional Int32 spacecount
function m.ArrayListToString(list, outstr, spacecount) end
---public ArrayList drawGrid(LineRenderer prefab, Vector3 origin, Int32 numRows, Int32 numCols, Single cellSize, Color color, Transform gridRoot, Single h)
---@return ArrayList
---@param optional LineRenderer prefab
---@param optional Vector3 origin
---@param optional Int32 numRows
---@param optional Int32 numCols
---@param optional Single cellSize
---@param optional Color color
---@param optional Transform gridRoot
---@param optional Single h
function m.drawGrid(prefab, origin, numRows, numCols, cellSize, color, gridRoot, h) end
---public LineRenderer drawLine(LineRenderer prefab, Vector3 startPos, Vector3 endPos, Color color)
---@return LineRenderer
---@param optional LineRenderer prefab
---@param optional Vector3 startPos
---@param optional Vector3 endPos
---@param optional Color color
function m.drawLine(prefab, startPos, endPos, color) end
---public GameObject cloneRes(String path)
---public GameObject cloneRes(GameObject prefab)
---@return GameObject
---@param optional GameObject prefab
function m.cloneRes(prefab) end
---public Object loadRes(String path)
---@return Object
---@param optional String path
function m.loadRes(path) end
---public GameObject loadGobj(String path)
---@return GameObject
---@param optional String path
function m.loadGobj(path) end
---public Vector2 addVector2(Vector2 v1, Vector2 v2)
---@return Vector2
---@param optional Vector2 v1
---@param optional Vector2 v2
function m.addVector2(v1, v2) end
---public Vector3 addVector3(Vector3 v1, Vector3 v2)
---@return Vector3
---@param optional Vector3 v1
---@param optional Vector3 v2
function m.addVector3(v1, v2) end
---public Vector2 cutVector2(Vector2 v1, Vector2 v2)
---@return Vector2
---@param optional Vector2 v1
---@param optional Vector2 v2
function m.cutVector2(v1, v2) end
---public Vector3 cutVector3(Vector3 v1, Vector3 v2)
---@return Vector3
---@param optional Vector3 v1
---@param optional Vector3 v2
function m.cutVector3(v1, v2) end
---public Transform getChild(Transform root, Object[] args)
---@return Transform
---@param optional Transform root
---@param optional Object[] args
function m.getChild(root, args) end
---public String getSDCard()
---@return String
function m.getSDCard() end
---public String chgToSDCard(String path)
---@return String
---@param optional String path
function m.chgToSDCard(path) end
---public String MD5Encrypt(String strText)
---public String MD5Encrypt(Byte[] bytes)
---@return String
---@param optional Byte[] bytes
function m.MD5Encrypt(bytes) end
---public Byte[] getUtf8bytes(String str)
---@return table
---@param optional String str
function m.getUtf8bytes(str) end
---public Boolean netIsActived()
---@return bool
function m.netIsActived() end
---public String getNetState()
---@return String
function m.getNetState() end
---public String urlAddTimes(String url)
---@return String
---@param optional String url
function m.urlAddTimes(url) end
---public String getSingInCodeAndroid()
---@return String
function m.getSingInCodeAndroid() end
---public LayerMask getLayer(String layerName)
---@return LayerMask
---@param optional String layerName
function m.getLayer(layerName) end
---public RaycastHit getRaycastHitInfor(Camera camera, Vector3 inPos, LayerMask layer)
---@return RaycastHit
---@param optional Camera camera
---@param optional Vector3 inPos
---@param optional LayerMask layer
function m.getRaycastHitInfor(camera, inPos, layer) end
---public Object[] doCallback(Object callback, Object[] args)
---@return table
---@param optional Object callback
---@param optional Object[] args
function m.doCallback(callback, args) end
---public Hashtable fileToMap(String path)
---@return Hashtable
---@param optional String path
function m.fileToMap(path) end
---public Object fileToObj(String path)
---@return Object
---@param optional String path
function m.fileToObj(path) end
---public Void printe(Object msg)
---@param optional Object msg
function m.printe(msg) end
---public Void printw(Object msg)
---@param optional Object msg
function m.printw(msg) end
---public Boolean IntersectRay(Bounds bounds, Ray ray)
---public Boolean IntersectRay(Bounds bounds, Ray ray, Single minDis, Single maxDis)
---@return bool
---@param Bounds bounds
---@param Ray ray
---@param optional Single minDis
---@param optional Single maxDis
function m.IntersectRay(bounds, ray, minDis, maxDis) end
---public Vector3 RotateAround(Vector3 currPoint, Vector3 point, Vector3 axis, Single angle)
---@return Vector3
---@param optional Vector3 currPoint
---@param optional Vector3 point
---@param optional Vector3 axis
---@param optional Single angle
function m.RotateAround(currPoint, point, axis, angle) end
---public Byte[] read4MemoryStream(MemoryStream ms, Int32 offset, Int32 len)
---@return table
---@param optional MemoryStream ms
---@param optional Int32 offset
---@param optional Int32 len
function m.read4MemoryStream(ms, offset, len) end
Coolape.Utl = m
return m

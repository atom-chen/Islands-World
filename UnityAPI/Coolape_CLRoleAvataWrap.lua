---@class Coolape.CLRoleAvata : UnityEngine.MonoBehaviour
---@field public bonesNames System.Collections.Generic.List1System.String
---@field public bonesList System.Collections.Generic.List1UnityEngine.Transform
---@field public animator UnityEngine.Animator
---@field public bodyPartNames System.Collections.Generic.List1System.String
---@field public bodyParts System.Collections.Generic.List1Coolape.CLBodyPart
---@field public bonesMap System.Collections.Hashtable
local m = { }
---public CLRoleAvata .ctor()
---@return CLRoleAvata
function m.New() end
---public Transform getBoneByName(String bname)
---@return Transform
---@param optional String bname
function m:getBoneByName(bname) end
---public Void setMapindex()
function m:setMapindex() end
---public Void OnApplicationQuit()
function m:OnApplicationQuit() end
---public Void OnDestroy()
function m:OnDestroy() end
---public Void cleanMaterial()
function m:cleanMaterial() end
---public Void setDefaultMaterial()
function m:setDefaultMaterial() end
---public Void switch2xx(String partName, String cellName)
---public Void switch2xx(String partName, String cellName, Object callback)
---@param String partName
---@param optional String cellName
---@param optional Object callback
function m:switch2xx(partName, cellName, callback) end
Coolape.CLRoleAvata = m
return m

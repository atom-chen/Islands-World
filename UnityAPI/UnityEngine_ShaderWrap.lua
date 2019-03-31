---@class UnityEngine.Shader : UnityEngine.Object
---@field public maximumLOD System.Int32
---@field public globalMaximumLOD System.Int32
---@field public isSupported System.Boolean
---@field public globalRenderPipeline System.String
---@field public renderQueue System.Int32

local m = { }
---public Shader Find(String name)
---@return Shader
---@param optional String name
function m.Find(name) end
---public Void EnableKeyword(String keyword)
---@param optional String keyword
function m.EnableKeyword(keyword) end
---public Void DisableKeyword(String keyword)
---@param optional String keyword
function m.DisableKeyword(keyword) end
---public Boolean IsKeywordEnabled(String keyword)
---@return bool
---@param optional String keyword
function m.IsKeywordEnabled(keyword) end
---public Void WarmupAllShaders()
function m.WarmupAllShaders() end
---public Int32 PropertyToID(String name)
---@return number
---@param optional String name
function m.PropertyToID(name) end
---public Void SetGlobalFloat(String name, Single value)
---public Void SetGlobalFloat(Int32 nameID, Single value)
---@param optional Int32 nameID
---@param optional Single value
function m.SetGlobalFloat(nameID, value) end
---public Void SetGlobalInt(String name, Int32 value)
---public Void SetGlobalInt(Int32 nameID, Int32 value)
---@param optional Int32 nameID
---@param optional Int32 value
function m.SetGlobalInt(nameID, value) end
---public Void SetGlobalVector(String name, Vector4 value)
---public Void SetGlobalVector(Int32 nameID, Vector4 value)
---@param optional Int32 nameID
---@param optional Vector4 value
function m.SetGlobalVector(nameID, value) end
---public Void SetGlobalColor(String name, Color value)
---public Void SetGlobalColor(Int32 nameID, Color value)
---@param optional Int32 nameID
---@param optional Color value
function m.SetGlobalColor(nameID, value) end
---public Void SetGlobalMatrix(String name, Matrix4x4 value)
---public Void SetGlobalMatrix(Int32 nameID, Matrix4x4 value)
---@param optional Int32 nameID
---@param optional Matrix4x4 value
function m.SetGlobalMatrix(nameID, value) end
---public Void SetGlobalTexture(String name, Texture value)
---public Void SetGlobalTexture(Int32 nameID, Texture value)
---@param optional Int32 nameID
---@param optional Texture value
function m.SetGlobalTexture(nameID, value) end
---public Void SetGlobalBuffer(String name, ComputeBuffer value)
---public Void SetGlobalBuffer(Int32 nameID, ComputeBuffer value)
---@param optional Int32 nameID
---@param optional ComputeBuffer value
function m.SetGlobalBuffer(nameID, value) end
---public Void SetGlobalFloatArray(String name, List`1 values)
---public Void SetGlobalFloatArray(Int32 nameID, List`1 values)
---public Void SetGlobalFloatArray(String name, Single[] values)
---public Void SetGlobalFloatArray(Int32 nameID, Single[] values)
---@param optional Int32 nameID
---@param optional Single[] values
function m.SetGlobalFloatArray(nameID, values) end
---public Void SetGlobalVectorArray(String name, List`1 values)
---public Void SetGlobalVectorArray(Int32 nameID, List`1 values)
---public Void SetGlobalVectorArray(String name, Vector4[] values)
---public Void SetGlobalVectorArray(Int32 nameID, Vector4[] values)
---@param optional Int32 nameID
---@param optional Vector4[] values
function m.SetGlobalVectorArray(nameID, values) end
---public Void SetGlobalMatrixArray(String name, List`1 values)
---public Void SetGlobalMatrixArray(Int32 nameID, List`1 values)
---public Void SetGlobalMatrixArray(String name, Matrix4x4[] values)
---public Void SetGlobalMatrixArray(Int32 nameID, Matrix4x4[] values)
---@param optional Int32 nameID
---@param optional Matrix4x4[] values
function m.SetGlobalMatrixArray(nameID, values) end
---public Single GetGlobalFloat(String name)
---public Single GetGlobalFloat(Int32 nameID)
---@return number
---@param optional Int32 nameID
function m.GetGlobalFloat(nameID) end
---public Int32 GetGlobalInt(String name)
---public Int32 GetGlobalInt(Int32 nameID)
---@return number
---@param optional Int32 nameID
function m.GetGlobalInt(nameID) end
---public Vector4 GetGlobalVector(String name)
---public Vector4 GetGlobalVector(Int32 nameID)
---@return Vector4
---@param optional Int32 nameID
function m.GetGlobalVector(nameID) end
---public Color GetGlobalColor(String name)
---public Color GetGlobalColor(Int32 nameID)
---@return Color
---@param optional Int32 nameID
function m.GetGlobalColor(nameID) end
---public Matrix4x4 GetGlobalMatrix(String name)
---public Matrix4x4 GetGlobalMatrix(Int32 nameID)
---@return Matrix4x4
---@param optional Int32 nameID
function m.GetGlobalMatrix(nameID) end
---public Texture GetGlobalTexture(String name)
---public Texture GetGlobalTexture(Int32 nameID)
---@return Texture
---@param optional Int32 nameID
function m.GetGlobalTexture(nameID) end
---public Single[] GetGlobalFloatArray(String name)
---public Single[] GetGlobalFloatArray(Int32 nameID)
---public Void GetGlobalFloatArray(String name, List`1 values)
---public Void GetGlobalFloatArray(Int32 nameID, List`1 values)
---@return table
---@param Int32 nameID
---@param optional List`1 values
function m.GetGlobalFloatArray(nameID, values) end
---public Vector4[] GetGlobalVectorArray(String name)
---public Vector4[] GetGlobalVectorArray(Int32 nameID)
---public Void GetGlobalVectorArray(String name, List`1 values)
---public Void GetGlobalVectorArray(Int32 nameID, List`1 values)
---@return table
---@param Int32 nameID
---@param optional List`1 values
function m.GetGlobalVectorArray(nameID, values) end
---public Matrix4x4[] GetGlobalMatrixArray(String name)
---public Matrix4x4[] GetGlobalMatrixArray(Int32 nameID)
---public Void GetGlobalMatrixArray(String name, List`1 values)
---public Void GetGlobalMatrixArray(Int32 nameID, List`1 values)
---@return table
---@param Int32 nameID
---@param optional List`1 values
function m.GetGlobalMatrixArray(nameID, values) end
UnityEngine.Shader = m
return m

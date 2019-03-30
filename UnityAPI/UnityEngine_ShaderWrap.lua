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
---public Void SetGlobalFloat(Int32 nameID, Single value)
---public Void SetGlobalFloat(String name, Single value)
---@param optional String name
---@param optional Single value
function m.SetGlobalFloat(name, value) end
---public Void SetGlobalInt(Int32 nameID, Int32 value)
---public Void SetGlobalInt(String name, Int32 value)
---@param optional String name
---@param optional Int32 value
function m.SetGlobalInt(name, value) end
---public Void SetGlobalVector(Int32 nameID, Vector4 value)
---public Void SetGlobalVector(String name, Vector4 value)
---@param optional String name
---@param optional Vector4 value
function m.SetGlobalVector(name, value) end
---public Void SetGlobalColor(Int32 nameID, Color value)
---public Void SetGlobalColor(String name, Color value)
---@param optional String name
---@param optional Color value
function m.SetGlobalColor(name, value) end
---public Void SetGlobalMatrix(Int32 nameID, Matrix4x4 value)
---public Void SetGlobalMatrix(String name, Matrix4x4 value)
---@param optional String name
---@param optional Matrix4x4 value
function m.SetGlobalMatrix(name, value) end
---public Void SetGlobalTexture(Int32 nameID, Texture value)
---public Void SetGlobalTexture(String name, Texture value)
---@param optional String name
---@param optional Texture value
function m.SetGlobalTexture(name, value) end
---public Void SetGlobalBuffer(Int32 nameID, ComputeBuffer value)
---public Void SetGlobalBuffer(String name, ComputeBuffer value)
---@param optional String name
---@param optional ComputeBuffer value
function m.SetGlobalBuffer(name, value) end
---public Void SetGlobalFloatArray(String name, Single[] values)
---public Void SetGlobalFloatArray(Int32 nameID, Single[] values)
---public Void SetGlobalFloatArray(String name, List`1 values)
---public Void SetGlobalFloatArray(Int32 nameID, List`1 values)
---@param optional Int32 nameID
---@param optional List`1 values
function m.SetGlobalFloatArray(nameID, values) end
---public Void SetGlobalVectorArray(String name, Vector4[] values)
---public Void SetGlobalVectorArray(Int32 nameID, Vector4[] values)
---public Void SetGlobalVectorArray(String name, List`1 values)
---public Void SetGlobalVectorArray(Int32 nameID, List`1 values)
---@param optional Int32 nameID
---@param optional List`1 values
function m.SetGlobalVectorArray(nameID, values) end
---public Void SetGlobalMatrixArray(String name, Matrix4x4[] values)
---public Void SetGlobalMatrixArray(Int32 nameID, Matrix4x4[] values)
---public Void SetGlobalMatrixArray(String name, List`1 values)
---public Void SetGlobalMatrixArray(Int32 nameID, List`1 values)
---@param optional Int32 nameID
---@param optional List`1 values
function m.SetGlobalMatrixArray(nameID, values) end
---public Single GetGlobalFloat(Int32 nameID)
---public Single GetGlobalFloat(String name)
---@return number
---@param optional String name
function m.GetGlobalFloat(name) end
---public Int32 GetGlobalInt(Int32 nameID)
---public Int32 GetGlobalInt(String name)
---@return number
---@param optional String name
function m.GetGlobalInt(name) end
---public Vector4 GetGlobalVector(Int32 nameID)
---public Vector4 GetGlobalVector(String name)
---@return Vector4
---@param optional String name
function m.GetGlobalVector(name) end
---public Color GetGlobalColor(Int32 nameID)
---public Color GetGlobalColor(String name)
---@return Color
---@param optional String name
function m.GetGlobalColor(name) end
---public Matrix4x4 GetGlobalMatrix(Int32 nameID)
---public Matrix4x4 GetGlobalMatrix(String name)
---@return Matrix4x4
---@param optional String name
function m.GetGlobalMatrix(name) end
---public Texture GetGlobalTexture(Int32 nameID)
---public Texture GetGlobalTexture(String name)
---@return Texture
---@param optional String name
function m.GetGlobalTexture(name) end
---public Single[] GetGlobalFloatArray(Int32 nameID)
---public Single[] GetGlobalFloatArray(String name)
---public Void GetGlobalFloatArray(Int32 nameID, List`1 values)
---public Void GetGlobalFloatArray(String name, List`1 values)
---@return table
---@param String name
---@param optional List`1 values
function m.GetGlobalFloatArray(name, values) end
---public Vector4[] GetGlobalVectorArray(Int32 nameID)
---public Vector4[] GetGlobalVectorArray(String name)
---public Void GetGlobalVectorArray(Int32 nameID, List`1 values)
---public Void GetGlobalVectorArray(String name, List`1 values)
---@return table
---@param String name
---@param optional List`1 values
function m.GetGlobalVectorArray(name, values) end
---public Matrix4x4[] GetGlobalMatrixArray(Int32 nameID)
---public Matrix4x4[] GetGlobalMatrixArray(String name)
---public Void GetGlobalMatrixArray(Int32 nameID, List`1 values)
---public Void GetGlobalMatrixArray(String name, List`1 values)
---@return table
---@param String name
---@param optional List`1 values
function m.GetGlobalMatrixArray(name, values) end
UnityEngine.Shader = m
return m

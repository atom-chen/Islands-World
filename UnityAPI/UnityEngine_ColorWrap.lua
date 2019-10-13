---@class UnityEngine.Color : System.ValueType
---@field public r System.Single
---@field public g System.Single
---@field public b System.Single
---@field public a System.Single
---@field public red UnityEngine.Color
---@field public green UnityEngine.Color
---@field public blue UnityEngine.Color
---@field public white UnityEngine.Color
---@field public black UnityEngine.Color
---@field public yellow UnityEngine.Color
---@field public cyan UnityEngine.Color
---@field public magenta UnityEngine.Color
---@field public gray UnityEngine.Color
---@field public grey UnityEngine.Color
---@field public clear UnityEngine.Color
---@field public grayscale System.Single
---@field public linear UnityEngine.Color
---@field public gamma UnityEngine.Color
---@field public maxColorComponent System.Single
---@field public Item System.Single
local m = { }
---public Color .ctor(Single r, Single g, Single b)
---public Color .ctor(Single r, Single g, Single b, Single a)
---@return Color
---@param Single r
---@param optional Single g
---@param optional Single b
---@param optional Single a
function m.New(r, g, b, a) end
---public String ToString()
---public String ToString(String format)
---@return String
---@param String format
function m:ToString(format) end
---public Int32 GetHashCode()
---@return number
function m:GetHashCode() end
---public Boolean Equals(Object other)
---public Boolean Equals(Color other)
---@return bool
---@param optional Color other
function m:Equals(other) end
---public Color op_Addition(Color a, Color b)
---@return Color
---@param optional Color a
---@param optional Color b
function m.op_Addition(a, b) end
---public Color op_Subtraction(Color a, Color b)
---@return Color
---@param optional Color a
---@param optional Color b
function m.op_Subtraction(a, b) end
---public Color op_Multiply(Color a, Color b)
---public Color op_Multiply(Color a, Single b)
---public Color op_Multiply(Single b, Color a)
---@return Color
---@param optional Single b
---@param optional Color a
function m.op_Multiply(b, a) end
---public Color op_Division(Color a, Single b)
---@return Color
---@param optional Color a
---@param optional Single b
function m.op_Division(a, b) end
---public Boolean op_Equality(Color lhs, Color rhs)
---@return bool
---@param optional Color lhs
---@param optional Color rhs
function m.op_Equality(lhs, rhs) end
---public Boolean op_Inequality(Color lhs, Color rhs)
---@return bool
---@param optional Color lhs
---@param optional Color rhs
function m.op_Inequality(lhs, rhs) end
---public Color Lerp(Color a, Color b, Single t)
---@return Color
---@param optional Color a
---@param optional Color b
---@param optional Single t
function m.Lerp(a, b, t) end
---public Color LerpUnclamped(Color a, Color b, Single t)
---@return Color
---@param optional Color a
---@param optional Color b
---@param optional Single t
function m.LerpUnclamped(a, b, t) end
---public Vector4 op_Implicit(Color c)
---public Color op_Implicit(Vector4 v)
---@return Vector4
---@param optional Vector4 v
function m.op_Implicit(v) end
---public Void RGBToHSV(Color rgbColor, Single& H, Single& S, Single& V)
---@param optional Color rgbColor
---@param optional Single& H
---@param optional Single& S
---@param optional Single& V
function m.RGBToHSV(rgbColor, H, S, V) end
---public Color HSVToRGB(Single H, Single S, Single V)
---public Color HSVToRGB(Single H, Single S, Single V, Boolean hdr)
---@return Color
---@param Single H
---@param optional Single S
---@param optional Single V
---@param optional Boolean hdr
function m.HSVToRGB(H, S, V, hdr) end
UnityEngine.Color = m
return m

---@class Coolape.ColorEx

local m = { }
---public Color GetJetColor(Single val)
---@return Color
---@param optional Single val
function m.GetJetColor(val) end
---public Color getGrayColor()
---@return Color
function m.getGrayColor() end
---public Color getGrayColor2()
---@return Color
function m.getGrayColor2() end
---public Color getColor(Int32 r, Int32 g, Int32 b)
---public Color getColor(Int32 r, Int32 g, Int32 b, Int32 a)
---@return Color
---@param Int32 r
---@param optional Int32 g
---@param optional Int32 b
---@param optional Int32 a
function m.getColor(r, g, b, a) end
Coolape.ColorEx = m
return m

---@class UILabel : UIWidget
---@field public keepCrispWhenShrunk UILabel.Crispness
---@field public isAppendEndingString System.Boolean
---@field public AppendString System.String
---@field public fontName System.String
---@field public isAnchoredHorizontally System.Boolean
---@field public isAnchoredVertically System.Boolean
---@field public material UnityEngine.Material
---@field public bitmapFont UIFont
---@field public trueTypeFont UnityEngine.Font
---@field public ambigiousFont UnityEngine.Object
---@field public text System.String
---@field public defaultFontSize System.Int32
---@field public fontSize System.Int32
---@field public fontStyle UnityEngine.FontStyle
---@field public alignment NGUIText.Alignment
---@field public applyGradient System.Boolean
---@field public gradientTop UnityEngine.Color
---@field public gradientBottom UnityEngine.Color
---@field public spacingX System.Int32
---@field public spacingY System.Int32
---@field public useFloatSpacing System.Boolean
---@field public floatSpacingX System.Single
---@field public floatSpacingY System.Single
---@field public effectiveSpacingY System.Single
---@field public effectiveSpacingX System.Single
---@field public supportEncoding System.Boolean
---@field public symbolStyle NGUIText.SymbolStyle
---@field public overflowMethod UILabel.Overflow
---@field public multiLine System.Boolean
---@field public localCorners UnityEngine.Vector3
---@field public worldCorners UnityEngine.Vector3
---@field public drawingDimensions UnityEngine.Vector4
---@field public maxLineCount System.Int32
---@field public effectStyle UILabel.Effect
---@field public effectColor UnityEngine.Color
---@field public effectDistance UnityEngine.Vector2
---@field public processedText System.String
---@field public printedSize UnityEngine.Vector2
---@field public localSize UnityEngine.Vector2

local m = { }
---public UILabel .ctor()
---@return UILabel
function m.New() end
---public Vector3[] GetSides(Transform relativeTo)
---@return table
---@param optional Transform relativeTo
function m:GetSides(relativeTo) end
---public Void MarkAsChanged()
function m:MarkAsChanged() end
---public Void ProcessText()
function m:ProcessText() end
---public Void MakePixelPerfect()
function m:MakePixelPerfect() end
---public Void AssumeNaturalSize()
function m:AssumeNaturalSize() end
---public Int32 GetCharacterIndexAtPosition(Vector2 localPos, Boolean precise)
---public Int32 GetCharacterIndexAtPosition(Vector3 worldPos, Boolean precise)
---@return number
---@param optional Vector3 worldPos
---@param optional Boolean precise
function m:GetCharacterIndexAtPosition(worldPos, precise) end
---public String GetWordAtPosition(Vector2 localPos)
---public String GetWordAtPosition(Vector3 worldPos)
---@return String
---@param optional Vector3 worldPos
function m:GetWordAtPosition(worldPos) end
---public String GetWordAtCharacterIndex(Int32 characterIndex)
---@return String
---@param optional Int32 characterIndex
function m:GetWordAtCharacterIndex(characterIndex) end
---public String GetUrlAtPosition(Vector2 localPos)
---public String GetUrlAtPosition(Vector3 worldPos)
---@return String
---@param optional Vector3 worldPos
function m:GetUrlAtPosition(worldPos) end
---public String GetUrlAtCharacterIndex(Int32 characterIndex)
---@return String
---@param optional Int32 characterIndex
function m:GetUrlAtCharacterIndex(characterIndex) end
---public Int32 GetCharacterIndex(Int32 currentIndex, KeyCode key)
---@return number
---@param optional Int32 currentIndex
---@param optional KeyCode key
function m:GetCharacterIndex(currentIndex, key) end
---public Void PrintOverlay(Int32 start, Int32 ed, UIGeometry caret, UIGeometry highlight, Color caretColor, Color highlightColor)
---@param optional Int32 start
---@param optional Int32 ed
---@param optional UIGeometry caret
---@param optional UIGeometry highlight
---@param optional Color caretColor
---@param optional Color highlightColor
function m:PrintOverlay(start, end, caret, highlight, caretColor, highlightColor) end
---public Void OnFill(BetterList`1 verts, BetterList`1 uvs, BetterList`1 cols)
---@param optional BetterList`1 verts
---@param optional BetterList`1 uvs
---@param optional BetterList`1 cols
function m:OnFill(verts, uvs, cols) end
---public Vector2 ApplyOffset(BetterList`1 verts, Int32 start)
---@return Vector2
---@param optional BetterList`1 verts
---@param optional Int32 start
function m:ApplyOffset(verts, start) end
---public Void ApplyShadow(BetterList`1 verts, BetterList`1 uvs, BetterList`1 cols, Int32 start, Int32 ed, Single x, Single y)
---@param optional BetterList`1 verts
---@param optional BetterList`1 uvs
---@param optional BetterList`1 cols
---@param optional Int32 start
---@param optional Int32 ed
---@param optional Single x
---@param optional Single y
function m:ApplyShadow(verts, uvs, cols, start, end, x, y) end
---public Int32 CalculateOffsetToFit(String text)
---@return number
---@param optional String text
function m:CalculateOffsetToFit(text) end
---public Void SetCurrentProgress()
function m:SetCurrentProgress() end
---public Void SetCurrentPercent(GameObject go)
---@param optional GameObject go
function m:SetCurrentPercent(go) end
---public Void SetCurrentSelection(GameObject go)
---@param optional GameObject go
function m:SetCurrentSelection(go) end
---public Boolean Wrap(String text, String& final)
---public Boolean Wrap(String text, String& final, Int32 height)
---@return bool
---@param String text
---@param optional String& final
---@param optional Int32 height
function m:Wrap(text, final, height) end
---public Void UpdateNGUIText()
function m:UpdateNGUIText() end
UILabel = m
return m

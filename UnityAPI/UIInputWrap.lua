---@class UIInput : UnityEngine.MonoBehaviour
---@field public current UIInput
---@field public selection UIInput
---@field public label UILabel
---@field public inputType UIInput.InputType
---@field public onReturnKey UIInput.OnReturnKey
---@field public keyboardType UIInput.KeyboardType
---@field public hideInput System.Boolean
---@field public selectAllTextOnFocus System.Boolean
---@field public validation UIInput.Validation
---@field public characterLimit System.Int32
---@field public savedAs System.String
---@field public activeTextColor UnityEngine.Color
---@field public caretColor UnityEngine.Color
---@field public selectionColor UnityEngine.Color
---@field public onSubmit System.Collections.Generic.List1EventDelegate
---@field public onChange System.Collections.Generic.List1EventDelegate
---@field public onValidate UIInput.OnValidate
---@field public defaultText System.String
---@field public inputShouldBeHidden System.Boolean
---@field public value System.String
---@field public isSelected System.Boolean
---@field public cursorPosition System.Int32
---@field public selectionStart System.Int32
---@field public selectionEnd System.Int32
---@field public caret UITexture

local m = { }
---public UIInput .ctor()
---@return UIInput
function m.New() end
---public String Validate(String val)
---@return String
---@param optional String val
function m:Validate(val) end
---public Boolean ProcessEvent(Event ev)
---@return bool
---@param optional Event ev
function m:ProcessEvent(ev) end
---public Void Insert(String text)
---@param optional String text
function m:Insert(text) end
---public Void Submit()
function m:Submit() end
---public Void UpdateLabel()
function m:UpdateLabel() end
---public Void RemoveFocus()
function m:RemoveFocus() end
---public Void SaveValue()
function m:SaveValue() end
---public Void LoadValue()
function m:LoadValue() end
UIInput = m
return m

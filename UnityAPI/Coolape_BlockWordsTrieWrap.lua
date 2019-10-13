---@class Coolape.BlockWordsTrie
local m = { }
---public BlockWordsTrie getInstanse()
---@return BlockWordsTrie
function m.getInstanse() end
---public Void init(String shieldWords)
---@param optional String shieldWords
function m:init(shieldWords) end
---public Void Add(String content)
---@param optional String content
function m:Add(content) end
---public Boolean isExistChar(Char first)
---@return bool
---@param optional Char first
function m:isExistChar(first) end
---public Boolean isExistCharArrays(Char[] text, Int32 begin, Int32 length)
---@return bool
---@param optional Char[] text
---@param optional Int32 begin
---@param optional Int32 length
function m:isExistCharArrays(text, begin, length) end
---public String filter(String text)
---@return String
---@param optional String text
function m:filter(text) end
---public Boolean isUnlawful(String text)
---@return bool
---@param optional String text
function m:isUnlawful(text) end
Coolape.BlockWordsTrie = m
return m

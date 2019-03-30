require("city.IDLUnitBase")
---@class IDLTree
IDLTree = class("IDLTree", IDLUnitBase)

function IDLTree:OnClick()
    self:getBase(IDLTree).OnClick(self)
end

function IDLTree:OnPress(go, isPress )
    -- no thing
end

function IDLTree:OnDrag( go, delta )
    -- no thing
end


function IDLTree:clean()
    self:getBase(IDLTree).clean(self)
end

return IDLTree

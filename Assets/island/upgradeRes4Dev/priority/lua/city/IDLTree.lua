require("city.IDLUnitBase")
---@class IDLTree:IDLBuilding
IDLTree = class("IDLTree", IDLBuilding)

function IDLTree:init(selfObj, id, star, lev, _isOffense, other)
    -- 通过这种模式把self传过去，不能 self.super:init()
    IDLTree.super.init(self, selfObj, id, star, lev, _isOffense, other)
    self.isTree = true
end

function IDLTree:OnClick()
    IDLTree.super.OnClick(self)
end

function IDLTree:OnPress(go, isPress )
    -- no thing
end

function IDLTree:OnDrag( go, delta )
    -- no thing
end


function IDLTree:clean()
    IDLTree.super.clean(self)
end

return IDLTree

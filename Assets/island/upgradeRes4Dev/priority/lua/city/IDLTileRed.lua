-- xx单元
local _cell = {}
---@type Coolape.CLCellLua
local csSelf = nil
local transform = nil
local mData = nil
local uiobjs = {}

-- 初始化，只调用一次
function _cell.init(csObj)
    csSelf = csObj
    transform = csSelf.transform
    --[[
    上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
	--]]
    ---@type UISprite
    uiobjs.sprite = csSelf:GetComponent("UISprite")
end

-- 显示，
-- 注意，c#侧不会在调用show时，调用refresh
function _cell.show(go, data)
    mData = data
    local index = mData.index
    if mData.isSide then
        ---@type Coolape.CLAStarNode
        local node = IDMainCity.grid.nodesMap[index]

        if
            IDMainCity.isCannotDeploySide(node.up.index) and IDMainCity.isCannotDeploySide(node.left.index) and
                IDMainCity.isCannotDeploySide(node.right.index)
         then
            uiobjs.sprite.spriteName = "shadow_tileLine3"
            uiobjs.sprite.transform.localEulerAngles = Vector3.zero
        elseif
            IDMainCity.isCannotDeploySide(node.down.index) and IDMainCity.isCannotDeploySide(node.left.index) and
                IDMainCity.isCannotDeploySide(node.right.index)
         then
            uiobjs.sprite.spriteName = "shadow_tileLine3"
            uiobjs.sprite.transform.localEulerAngles = Vector3(0, 0, 180)
        elseif
            IDMainCity.isCannotDeploySide(node.left.index) and IDMainCity.isCannotDeploySide(node.up.index) and
                IDMainCity.isCannotDeploySide(node.down.index)
         then
            uiobjs.sprite.spriteName = "shadow_tileLine3"
            uiobjs.sprite.transform.localEulerAngles = Vector3(0, 0, 90)
        elseif
            IDMainCity.isCannotDeploySide(node.right.index) and IDMainCity.isCannotDeploySide(node.up.index) and
                IDMainCity.isCannotDeploySide(node.down.index)
         then
            uiobjs.sprite.spriteName = "shadow_tileLine3"
            uiobjs.sprite.transform.localEulerAngles = Vector3(0, 0, 270)
        elseif IDMainCity.isCannotDeploySide(node.up.index) and IDMainCity.isCannotDeploySide(node.left.index) then
            uiobjs.sprite.spriteName = "shadow_tileLine2"
            uiobjs.sprite.transform.localEulerAngles = Vector3.zero
        elseif IDMainCity.isCannotDeploySide(node.up.index) and IDMainCity.isCannotDeploySide(node.right.index) then
            uiobjs.sprite.spriteName = "shadow_tileLine2"
            uiobjs.sprite.transform.localEulerAngles = Vector3(0, 0, 270)
        elseif IDMainCity.isCannotDeploySide(node.down.index) and IDMainCity.isCannotDeploySide(node.left.index) then
            uiobjs.sprite.spriteName = "shadow_tileLine2"
            uiobjs.sprite.transform.localEulerAngles = Vector3(0, 0, 90)
        elseif IDMainCity.isCannotDeploySide(node.down.index) and IDMainCity.isCannotDeploySide(node.right.index) then
            uiobjs.sprite.spriteName = "shadow_tileLine2"
            uiobjs.sprite.transform.localEulerAngles = Vector3(0, 0, 180)
        elseif IDMainCity.isCannotDeploySide(node.up.index) then
            uiobjs.sprite.spriteName = "shadow_tileLine"
            uiobjs.sprite.transform.localEulerAngles = Vector3.zero
        elseif IDMainCity.isCannotDeploySide(node.down.index) then
            uiobjs.sprite.spriteName = "shadow_tileLine"
            uiobjs.sprite.transform.localEulerAngles = Vector3(0, 0, 180)
        elseif IDMainCity.isCannotDeploySide(node.left.index) then
            uiobjs.sprite.spriteName = "shadow_tileLine"
            uiobjs.sprite.transform.localEulerAngles = Vector3(0, 0, 90)
        elseif IDMainCity.isCannotDeploySide(node.right.index) then
            uiobjs.sprite.spriteName = "shadow_tileLine"
            uiobjs.sprite.transform.localEulerAngles = Vector3(0, 0, 270)
        else
            uiobjs.sprite.spriteName = "shadow_tile"
        end
    else
        uiobjs.sprite.spriteName = "shadow_tile"
    end
end

-- 取得数据
function _cell.getData()
    return mData
end

--------------------------------------------
return _cell

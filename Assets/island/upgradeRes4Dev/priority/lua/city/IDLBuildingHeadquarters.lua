---@public 资源建筑
require("city.IDLBuilding")

---@class IDLBuildingHeadquarters
IDLBuildingHeadquarters = class("IDLBuildingHeadquarters", IDLBuilding)

function IDLBuildingHeadquarters:init(selfObj, id, star, lev, _isOffense, other)
    -- 通过这种模式把self传过去，不能 self.super:init()
    self:getBase(IDLBuildingHeadquarters).init(self, selfObj, id, star, lev, _isOffense, other)
end

function IDLBuildingHeadquarters:OnClick()
    if IDWorldMap.mode == GameModeSub.city then
        self:getBase(IDLBuildingHeadquarters).OnClick(self)
    elseif IDWorldMap.mode == GameModeSub.map then
        IDWorldMap.onClickSelfCity()
    end
end

---@public 当屏幕缩放时的逻辑处理
function IDLBuildingHeadquarters:onScaleScreen(scaleVal)
    local pos = self.transform.position
    if self.shadow then
        self.shadow.position = pos + Vector3.up * 0.02
        self.shadow.localScale = (Vector3.one * scaleVal) * (bio2number(self.attr.ShadowSize) / 10)
    end
end


---@public 显示隐藏（可能为连带做一些其它的处理）
function IDLBuildingHeadquarters:SetActive(active)
    -- self:getBase(IDLBuilding).SetActive(self, active)
    if active then
        self:upgrading()
        -- self:loadFloor()
    else
        self:fireWorker(true)
        self:unLoadProgressHud()
        -- if self.floor then
        --     SetActive(self.floor, active)
        -- end
    end
end
--------------------------------------------
return IDLBuildingHeadquarters

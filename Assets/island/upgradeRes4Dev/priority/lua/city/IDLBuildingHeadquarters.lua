---@public 资源建筑
require("city.IDLBuilding")

---@class IDLBuildingHeadquarters:IDLBuilding
IDLBuildingHeadquarters = class("IDLBuildingHeadquarters", IDLBuilding)

function IDLBuildingHeadquarters:init(selfObj, id, star, lev, _isOffense, other)
    -- 通过这种模式把self传过去，不能 self.super:init()
    IDLBuildingHeadquarters.super.init(self, selfObj, id, star, lev, _isOffense, other)
    ---@type IDCellWorldTileHudParam
    self.hudData4worldmap = {}
    self.hudData4worldmap.target = self.transform
    self.hudData4worldmap.offset = Vector3.zero
    ---@type NetProtoIsland.ST_mapCell
    local d = {}
    d.name = IDDBPlayer.myself.name
    d.lev = IDDBPlayer.myself.lev
    d.state = IDDBCity.curCity.stat
    self.hudData4worldmap.data = d
    self.hudData4worldmap.attr = DBCfg.getDataById(DBCfg.CfgPath.MapTile, 7)
end

function IDLBuildingHeadquarters:OnClick()
    if IDWorldMap.mode == GameModeSub.city then
        IDLBuildingHeadquarters.super.OnClick(self)
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

function IDLBuildingHeadquarters:showHud4WorldMap()
    --//TODO:显示能表示是自己主城的
    ---@type Coolape.CLCellLua
    if self.hud4Worldmap == nil then
        CLUIOtherObjPool.borrowObjAsyn(
            "WorldTileHud",
            function(name, obj, orgs)
                ---@param obj UnityEngine.GameObject
                if self.hud4Worldmap or (not self.gameObject.activeInHierarchy) then
                    CLUIOtherObjPool.returnObj(obj)
                    SetActive(obj, false)
                    return
                end
                self.hud4Worldmap = obj:GetComponent("CLCellLua")
                self.hud4Worldmap.transform.parent = MyCfg.self.hud3dRoot
                self.hud4Worldmap.transform.localScale = Vector3.one
                self.hud4Worldmap.transform.localEulerAngles = Vector3.zero
                self.hud4Worldmap:init(self.hudData4worldmap, nil)
                SetActive(self.hud4Worldmap.gameObject, true)
            end
        )
    end
    if self.hud4WorldmapTip == nil then
        CLUIOtherObjPool.borrowObjAsyn(
            "WorldTipHud",
            function(name, obj, orgs)
                ---@param obj UnityEngine.GameObject
                if self.hud4WorldmapTip or (not self.gameObject.activeInHierarchy) then
                    CLUIOtherObjPool.returnObj(obj)
                    SetActive(obj, false)
                    return
                end
                self.hud4WorldmapTip = obj:GetComponent("CLCellLua")
                self.hud4WorldmapTip.transform.parent = MyCfg.self.hud3dRoot
                self.hud4WorldmapTip.transform.localScale = Vector3.one
                self.hud4WorldmapTip.transform.localEulerAngles = Vector3.zero
                self.hud4WorldmapTip:init(self.hudData4worldmap, nil)
                SetActive(self.hud4WorldmapTip.gameObject, true)
            end
        )
    end
end

function IDLBuildingHeadquarters:hideHud4WorldMap()
    if self.hud4Worldmap then
        CLUIOtherObjPool.returnObj(self.hud4Worldmap.gameObject)
        SetActive(self.hud4Worldmap.gameObject, false)
        self.hud4Worldmap = nil
    end

    if self.hud4WorldmapTip then
        CLUIOtherObjPool.returnObj(self.hud4WorldmapTip.gameObject)
        SetActive(self.hud4WorldmapTip.gameObject, false)
        self.hud4WorldmapTip = nil
    end
end

---@public 显示隐藏（可能为连带做一些其它的处理）
function IDLBuildingHeadquarters:SetActive(active)
    if active then
        -- self:loadFloor()
        self:upgrading()
    else
        -- if self.floor then
        --     SetActive(self.floor, active)
        -- end
        self:fireWorker(true)
        self:unLoadProgressHud()
    end
end

function IDLBuildingHeadquarters:clean()
    IDLBuildingHeadquarters.super.clean(self)
    self:hideHud4WorldMap()
end
--------------------------------------------
return IDLBuildingHeadquarters

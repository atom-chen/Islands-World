-- 大地图地块
require("public.class")
---@class IDWorldTile:ClassBase
IDWorldTile = class("IDWorldTile")

function IDWorldTile:ctor()
    self.gidx = 0 -- 网格坐标
    self.type = nil
    self.serverData = nil -- 服务器数据
    self.baseData = nil -- 基础数据
    self.isInited = false
end

function IDWorldTile:__init(csobj)
    if self.isInited then
        return
    end
    self.isInited = true
    ---@type Coolape.CLBaseLua
    self.csSelf = csobj
    self.transform = csobj.transform
    self.gameObject = csobj.gameObject
    ---@type UnityEngine.BoxCollider
    self.boxCollider = self.csSelf:GetComponent("BoxCollider")
    self.body = getChild(self.transform, "body")
    if self.body then
        self.body = self.body.gameObject
    end
    self.seabed = getChild(self.transform, "seabed").gameObject
end

function IDWorldTile:init(csobj, gidx, type, data, attr)
    self:__init(csobj)
    self.gidx = gidx
    self.type = type
    ---@type NetProtoIsland.ST_mapCell
    self.serverData = data
    ---@type DBCFMapTileData
    self.attr = attr
    self.size = bio2number(self.attr.Size)
    ---@type Bounds
    self.bounds = self.boxCollider.bounds --Bounds(self.transform.position, Vector3.one * 5)
    ---@type IDCellWorldTileHudParam
    self.hudData = {}
    self.hudData.target = self.transform
    self.hudData.offset = Vector3.zero
    self.hudData.data = self.serverData
    self.hudData.attr = self.attr

    self.isShowingBody = true
    self:showBody()
end

function IDWorldTile:showHud()
    local gid = bio2number(self.attr.GID)
    if
        not (gid == IDConst.WorldmapCellType.user or gid == IDConst.WorldmapCellType.port) or
            not IDWorldMap.isVisibile(self.transform.position)
     then
        return
    end

    ---@type Coolape.CLCellLua
    if self.hud == nil then
        CLUIOtherObjPool.borrowObjAsyn(
            "WorldTileHud",
            function(name, obj, orgs)
                ---@param obj UnityEngine.GameObject
                if self.hud or (not self.gameObject.activeInHierarchy) then
                    CLUIOtherObjPool.returnObj(obj)
                    SetActive(obj, false)
                    return
                end
                self.hud = obj:GetComponent("CLCellLua")
                self.hud.transform.parent = MyCfg.self.hud3dRoot
                self.hud.transform.localScale = Vector3.one
                self.hud.transform.localEulerAngles = Vector3.zero
                self.hud:init(self.hudData, nil)
                SetActive(self.hud.gameObject, true)
            end
        )
    else
        self.hud:init(self.hudData, nil)
    end
end

function IDWorldTile:hideHud()
    if self.hud then
        CLUIOtherObjPool.returnObj(self.hud.gameObject)
        SetActive(self.hud.gameObject, false)
        self.hud = nil
    end
end

---@public 加载影子
function IDWorldTile:loadShadow()
    if self.shadow == nil then
        CLUIOtherObjPool.borrowObjAsyn(
            "shadow1",
            function(name, obj, orgs)
                if (not self.gameObject.activeInHierarchy) or self.shadow ~= nil or (not self.isShowingBody) then
                    CLUIOtherObjPool.returnObj(obj)
                    SetActive(obj, false)
                    return
                end
                self.shadow = obj.transform
                self.shadow.parent = MyCfg.self.shadowRoot
                self.shadow.localEulerAngles = Vector3.zero
                self.shadow.localScale = Vector3.one * 6 * bio2number(self.attr.Size)
                self.shadow.position = self.transform.position
                SetActive(self.shadow.gameObject, true)
            end
        )
    end
end

function IDWorldTile:releaseShadow()
    if self.shadow then
        CLUIOtherObjPool.returnObj(self.shadow.gameObject)
        SetActive(self.shadow.gameObject, false)
        self.shadow = nil
    end
end

-- function IDWorldTile:onScaleScreen(delta, offset)
--     if IDLCameraMgr.isInCameraView(self.bounds) then
--         if not self.isShowingBody then
--             self.isShowingBody = true
--             self:showBody()
--         end
--     else
--         if self.isShowingBody then
--             self.isShowingBody = false
--             self:hideBody()
--         end
--     end
-- end

function IDWorldTile:showBody()
    SetActive(self.body, true)
    SetActive(self.seabed, true)
    self:loadShadow()
    self:showHud()
end

function IDWorldTile:hideBody()
    SetActive(self.body, false)
    SetActive(self.seabed, false)
    self:releaseShadow()
    self:hideHud()
end

function IDWorldTile:clean()
    self:releaseShadow()
    self:hideHud()
end

function IDWorldTile:onNotifyLua(go)
    IDWorldMap.onClickTile(self)
end

return IDWorldTile

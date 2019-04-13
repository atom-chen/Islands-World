-- 大地图地块
require("public.class")
---@class IDWorldTile
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
    self.csSelf = csobj
    self.transform = csobj.transform
    self.gameObject = csobj.gameObject
    self.isInited = true
    self.body = getChild(self.transform, "body").gameObject
    self.seabed = getChild(self.transform, "seabed").gameObject
    self.bounds = Bounds(self.transform.position, Vector3.one * 5)
end

function IDWorldTile:init(csobj, gidx, type, data)
    self:__init(csobj)
    self.gidx = gidx
    self.type = type
    ---@type IDDBMapCell
    self.serverData = data
    if IDLCameraMgr.isInCameraView(self.bounds) then
        self.isShowingBody = true
        self:showBody()
    else
        self.isShowingBody = false
        self:hideBody()
    end
end

function IDWorldTile:onScaleScreen(delta, offset)
    if IDLCameraMgr.isInCameraView(self.bounds) then
        if not self.isShowingBody then
            self.isShowingBody = true
            self:showBody()
        end
    else
        if self.isShowingBody then
            self.isShowingBody = false
            self:hideBody()
        end
    end
end

---@public 加载影子
function IDWorldTile:loadShadow()
    if self.shadow == nil then
        CLUIOtherObjPool.borrowObjAsyn("shadow1",
                function(name, obj, orgs)
                    if (not self.gameObject.activeInHierarchy)
                            or self.shadow ~= nil
                            or (not self.isShowingBody) then
                        CLUIOtherObjPool.returnObj(obj)
                        SetActive(obj, false)
                        return
                    end
                    self.shadow = obj.transform;
                    self.shadow.parent = MyCfg.self.shadowRoot
                    self.shadow.localEulerAngles = Vector3.zero
                    self.shadow.localScale = Vector3.one * 7
                    self.shadow.position = self.transform.position
                    SetActive(self.shadow.gameObject, true)
                end)
    end
end

function IDWorldTile:releaseShadow()
    if self.shadow then
        CLUIOtherObjPool.returnObj(self.shadow.gameObject)
        SetActive(self.shadow.gameObject, false)
        self.shadow = nil
    end
end

function IDWorldTile:showBody()
    SetActive(self.body, true)
    SetActive(self.seabed, true)
    self:loadShadow()
end

function IDWorldTile:hideBody()
    SetActive(self.body, false)
    SetActive(self.seabed, false)
    self:releaseShadow()
end

function IDWorldTile:clean()
    self:releaseShadow()
end

function IDWorldTile:onNotifyLua(go)
    printe("IDWorldTile:OnClick")
end

return IDWorldTile

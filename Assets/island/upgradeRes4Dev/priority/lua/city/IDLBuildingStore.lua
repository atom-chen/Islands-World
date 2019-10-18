---@public 资源建筑
require("city.IDLBuilding")

---@class IDLBuildingStore:IDLBuilding
IDLBuildingStore = class("IDLBuildingStore", IDLBuilding)

function IDLBuildingStore:init(selfObj, id, star, lev, _isOffense, other)
    -- 通过这种模式把self传过去，不能 self.super:init()
    self:getBase(IDLBuildingStore).init(self, selfObj, id, star, lev, _isOffense, other)

    -- 设置建筑的资源类型
    self.resType = IDUtl.getResTypeByBuildingID(self.id)

    if self.serverData then
        self:showStoreState()
    else
        self:hideFullHud()
    end
end

---@public 资源数据
function IDLBuildingStore:showStoreState()
    if not self.serverData then
        return
    end
    local stored = bio2number(self.serverData.val)

    local maxStore =
        DBCfg.getGrowingVal(
        bio2number(self.attr.ComVal1Min),
        bio2number(self.attr.ComVal1Max),
        bio2number(self.attr.ComVal1Curve),
        bio2number(self.serverData.lev) / bio2number(self.attr.MaxLev)
    )
    if stored >= maxStore then
        -- 说明已经满了
        self:showFullHud()
    else
        self:hideFullHud()
    end
end

function IDLBuildingStore:showFullHud()
    if self.tipHud == nil then
        CLUIOtherObjPool.borrowObjAsyn(
            "TipHud",
            function(name, obj, orgs)
                if (not self.gameObject.activeInHierarchy) or self.tipHud ~= nil then
                    CLUIOtherObjPool.returnObj(obj)
                    SetActive(obj, false)
                    return
                end
                self.tipHud = obj:GetComponent("CLCellLua")
                self.tipHud.transform.parent = MyCfg.self.hud3dRoot
                self.tipHud.transform.localScale = Vector3.one
                self.tipHud.transform.localEulerAngles = Vector3.zero
                SetActive(self.tipHud.gameObject, true)
                self.tipHud:init(
                    {
                        target = self.csSelf,
                        data = self.serverData,
                        offset = Vector3(0, 2, 0),
                        label = "Full",
                        bgColor = Color.red
                    }
                )
            end
        )
    else
        SetActive(self.tipHud.gameObject, true)
        self.tipHud:init({target = self.csSelf, data = self.serverData, offset = Vector3(0, 2, 0)})
    end
end

function IDLBuildingStore:hideFullHud()
    if self.tipHud then
        CLUIOtherObjPool.returnObj(self.tipHud.gameObject)
        SetActive(self.tipHud.gameObject, false)
        self.tipHud = nil
    end
end

function IDLBuildingStore:OnClick()
    self:getBase(IDLBuildingStore).OnClick(self)
    if self.tipHud then
        CLAlert.add(LGet("MsgStoreIsFull"), Color.yellow, 1)
    end
end

function IDLBuildingStore:clean()
    self.csSelf:cancelInvoke4Lua()
    self:getBase(IDLBuildingStore).clean(self)
    self:hideFullHud()
end

--------------------------------------------
return IDLBuildingStore

require("city.IDLUnitBase")
---@class IDLBuilding
IDLBuilding = class("IDLBuilding", IDLUnitBase)

function IDLBuilding:init(selfObj, id, star, lev, _isOffense, other)
    -- 通过这种模式把self传过去，不能 self.super:init()
    self:getBase(IDLBuilding).init(self, selfObj, id, star, lev, _isOffense, other)
    self:loadFloor()
    self:upgrading()
end

---@public 加载地表
function IDLBuilding:loadFloor()
    if IDMainCity.isOnTheLand(self.gridIndex, self.size) then
        if self.floor == nil then
            CLThingsPool.borrowObjAsyn("buildingFloor",
                    function(name, obj, orgs)
                        ---@type UnityEngine.GameObject
                        local go = self.gameObject
                        if (not go.activeInHierarchy) or self.floor ~= nil then
                            CLThingsPool.returnObj(obj)
                            SetActive(obj, false)
                            return
                        end
                        obj.transform.parent = self.transform
                        obj.transform.localPosition = Vector3.zero
                        obj.transform.localScale = Vector3.one * self.size
                        SetActive(obj, true)
                        self.floor = obj
                    end)
        else
            SetActive(self.floor, true)
        end
    else
        if self.floor ~= nil then
            CLThingsPool.returnObj(self.floor)
            SetActive(self.floor, false)
            self.floor = nil
        end
    end
end

-- 当升级完成时的处理
function IDLBuilding:onFinishBuildingUpgrade()
    self:fireWorker()
    self:unLoadProgressHud()
    CLEffect.play("buildingLevUp", self.transform.position, self.transform)
    SoundEx.playSound("building_finished")
end

---@public 升级处理
function IDLBuilding:upgrading()
    ---@type IDDBBuilding
    local serverData = self.serverData
    if serverData then
        if bio2number(serverData.state) == IDConst.BuildingState.upgrade then
            -- 说明正在升级
            self:loadProgressHud()
            self:employWorker()
        elseif bio2number(serverData.state) == IDConst.BuildingState.renew then
            -- 恢复
        else
            self:unLoadProgressHud()
            self:fireWorker()
        end
    else
        self:unLoadProgressHud()
        self:fireWorker()
    end
end

---@public 加载进度
function IDLBuilding:loadProgressHud()
    if self.progressObj == nil then
        CLUIOtherObjPool.borrowObjAsyn("ProgressHud",
                function(name, obj, orgs)
                    if (not self.csSelf.gameObject.activeInHierarchy)
                            or self.serverData.state == IDConst.BuildingState.normal
                            or self.progressObj then
                        CLUIOtherObjPool.returnObj(obj)
                        SetActive(obj, false)
                        return
                    end
                    self.progressObj = obj:GetComponent("CLCellLua")
                    self.progressObj.transform.parent = MyCfg.self.hud3dRoot
                    self.progressObj.transform.localScale = Vector3.one
                    self.progressObj.transform.localEulerAngles = Vector3.zero
                    SetActive(obj, true)
                    self.progressObj:init({ target = self.csSelf, data = self.serverData, offset = Vector3(0, 2, 0) })
                end)
    else
        SetActive(self.progressObj.gameObject, true)
        self.progressObj:init({ target = self.csSelf, data = self.serverData, offset = Vector3(0, 2, 0) })
    end
end

---@public 释放进度
function IDLBuilding:unLoadProgressHud()
    if self.progressObj then
        CLUIOtherObjPool.returnObj(self.progressObj.gameObject)
        SetActive(self.progressObj.gameObject, false)
        self.progressObj = nil
    end
end

---@public 加载工人
function IDLBuilding:employWorker()
    if self.worker == nil then
        IDMainCity.employWorker(self,
                function(worker)
                    self.worker = worker
                    --self.worker:init(0, 0, 0, true, {})
                    --self.worker.luaTable.gotoWork(self)
                end)
    else
        --self.worker:init(0, 0, 0, true, {})
        --self.worker.luaTable.gotoWork(self)
    end
end

---@public 解雇工人
function IDLBuilding:fireWorker(immd)
    if self.worker ~= nil then
        IDMainCity.fireWorker(self)
        if immd then
            self.worker:onArrivedHome()
        end
        self.worker = nil
    end
end

---@public 显示隐藏（可能为连带做一些其它的处理）
function IDLBuilding:SetActive(active)
    self:getBase(IDLBuilding).SetActive(self, active)
    if active then
        self:upgrading()
        self:loadFloor()
    else
        self:fireWorker(true)
        self:unLoadProgressHud()
        if self.floor then
            SetActive(self.floor, active)
        end
    end
end

function IDLBuilding:clean()
    self:getBase(IDLBuilding).clean(self)

    if self.floor ~= nil then
        CLThingsPool.returnObj(self.floor)
        SetActive(self.floor, false)
        self.floor = nil
    end
    self:unLoadProgressHud()
    self:fireWorker()
end

return IDLBuilding

require("city.IDLUnitBase")
---@class IDLBuilding:IDLUnitBase
IDLBuilding = class("IDLBuilding", IDLUnitBase)

---@param csSelf MyUnit
function IDLBuilding:ctor(csSelf)
    IDLBuilding.super.ctor(self, csSelf)
    ---@type UnityEngine.Transform
    self.shadow = nil
end

function IDLBuilding:__init(selfObj, other)
    if IDLBuilding.super.__init(self, selfObj, other) then
        ---@type UnityEngine.Transform
        self.body = selfObj.mbody
        ---@type TweenScale
        self.tweenScale = getCC(self.transform, "body", "TweenScale")
        ---@type UnityEngine.Transform
        self.door = getChild(self.transform, "door")
        return true
    end
    return false
end

---@param other table  {index=网格位置 ,serverData=服务器数据（不是必须）}
function IDLBuilding:init(selfObj, id, star, lev, _isOffense, other)
    self.isDead = false
    self.isBuilding = true

    -- 通过这种模式把self传过去，不能 self.super:init()
    IDLBuilding.super.init(self, selfObj, id, star, lev, _isOffense, other)

    -- 取得属性配置
    ---@type NetProtoIsland.ST_building
    self.serverData = other.serverData
    ---@type DBCFBuildingData
    self.attr = DBCfg.getBuildingByID(id)
    -- 占格子
    self.size = bio2Int(self.attr.Size)
    self.gridIndex = other.index
    -- 所在格子的index
    self.oldindex = self.gridIndex

    if GameMode.battle == MyCfg.mode then
        -- 初始化数据
        ---@type UnitData4Battle
        self.data = {}
        self.data.HP =
            DBCfg.getGrowingVal(
            bio2number(self.attr.HPMin),
            bio2number(self.attr.HPMax),
            bio2number(self.attr.HPCurve),
            bio2number(self.serverData.lev) / bio2number(self.attr.MaxLev)
        )
        self.data.curHP = number2bio(self.data.HP)
        self.data.HP = number2bio(self.data.HP)
        self.data.damage =
            DBCfg.getGrowingVal(
            bio2number(self.attr.DamageMin),
            bio2number(self.attr.DamageMax),
            bio2number(self.attr.DamageCurve),
            bio2number(self.serverData.lev) / bio2number(self.attr.MaxLev)
        )
        self.data.damage = number2bio(self.data.damage)
    end

    if not other.hideShadow then
        -- 某种情况下是不需要影子的
        self:loadShadow()
    end
    self:loadFloor()
    self:upgrading()
end

---@public 加载影子
function IDLBuilding:loadShadow()
    if self.shadow == nil then
        local shadowType = bio2number(self.attr.ShadowType)
        if shadowType < 1 then
            return
        end
        local shadowName = joinStr("shadow", shadowType)
        CLUIOtherObjPool.borrowObjAsyn(
            shadowName,
            function(name, obj, orgs)
                if
                    self.shadow ~= nil or (not self.gameObject.activeInHierarchy) or
                        (not self.csSelf.mbody.gameObject.activeInHierarchy)
                 then
                    CLUIOtherObjPool.returnObj(obj)
                    SetActive(obj, false)
                    return
                end
                self.shadow = obj.transform
                self.shadow.parent = MyCfg.self.shadowRoot
                self.shadow.localEulerAngles = Vector3.zero
                self.shadow.localScale = Vector3.one * bio2number(self.attr.ShadowSize) / 10
                self.shadow.position = self.transform.position + Vector3.up * 0.02
                SetActive(self.shadow.gameObject, true)
            end
        )
    else
        self.shadow.parent = MyCfg.self.shadowRoot
        self.shadow.localEulerAngles = Vector3.zero
        self.shadow.localScale = Vector3.one * bio2number(self.attr.ShadowSize) / 10
        self.shadow.position = self.transform.position + Vector3.up * 0.02
        if (not self.gameObject.activeInHierarchy) or (not self.csSelf.mbody.gameObject.activeInHierarchy) then
            CLUIOtherObjPool.returnObj(self.shadow.gameObject)
            SetActive(self.shadow.gameObject, false)
            self.shadow = nil
        else
            SetActive(self.shadow.gameObject, true)
        end
    end
end

function IDLBuilding:hideShadow()
    if self.shadow then
        CLUIOtherObjPool.returnObj(self.shadow.gameObject)
        SetActive(self.shadow.gameObject, false)
        self.shadow = nil
    end
end

-- 可以放下
function IDLBuilding:isCanPlace(...)
    local grid = IDMainCity.getGrid()
    local index = grid:GetCellIndex(self.transform.position)
    if (IDMainCity.isSizeInFreeCell(index, self.size, self.attr.PlaceGround, self.attr.PlaceSea)) then
        return true
    end
    return false
end

-- 当点击角色时
function IDLBuilding:OnClick(...)
    self:_OnClick()
end
function IDLBuilding:_OnClick(...)
    if (not self.canClick) then
        return
    end

    if (MyCfg.mode == GameMode.battle) then
        -- 通知战场，玩家点击了我
        IDLBattle.onClickSomeObj(self, self.transform.position)
    else
        SoundEx.playSound(self.attr.SelectedEffect, 1, 1)
        self:jump()
        IDMainCity.onClickBuilding(self)
    end
end

function IDLBuilding:OnPress(go, isPress)
    self:_OnPress(go, isPress)
end

function IDLBuilding:_OnPress(go, isPress)
    if (not self.canClick) then
        return
    end
    if (IDMainCity.selectedUnit ~= self) then
        return
    end
    if (MyCfg.mode == GameMode.battle) then
        -- 通知战场，玩家按下了我
        IDLBattle.onPressRole(isPress, self.csSelf, self.transform.position)
    else
        if (isPress) then
            IDMainCity.setOtherUnitsColiderState(self, false)
            CLUIDrag4World.self.enabled = false --不可托动屏幕
        else
            self.isDraged = false

            self:jump()
            local grid = IDMainCity.getGrid()
            IDMainCity.grid:hide() -- 网格不显示
            local moved = false
            local index = grid:GetCellIndex(self.transform.position)
            if (IDMainCity.isSizeInFreeCell(index, self.size, self.attr.PlaceGround, self.attr.PlaceSea)) then
                IDLBuildingSize.hide()
                moved = (self.gridIndex ~= index)
                self.gridIndex = index
                IDMainCity.setOtherUnitsColiderState(nil, true)

                IDLCameraMgr.setPostProcessingProfile("normal")
                NGUITools.SetLayer(self.body.gameObject, LayerMask.NameToLayer("Building"))
                IDLBuildingSize.setLayer("Default")
            end
            -- 通知主城有释放建筑
            IDMainCity.onReleaseBuilding(self, moved)
            self.csSelf:invoke4Lua("setScreenCanDrag", 0.2)
        end
    end
end

function IDLBuilding:setScreenCanDrag()
    CLUIDrag4World.self.enabled = true --可托动屏幕
end

function IDLBuilding:onShowingGrid(...)
    if (not self.isDraged) then
        IDMainCity.hideGrid() --显示网格
    end
end

-- 拖动
function IDLBuilding:OnDrag(go, delta)
    self:_OnDrag(delta)
end

-- 拖动
function IDLBuilding:_OnDrag(delta)
    if (IDMainCity.selectedUnit ~= self) then
        return
    end
    if not self.isDraged then
        IDMainCity.grid:reShow(self.onShowingGrid) --显示网格
        IDLCameraMgr.setPostProcessingProfile("gray")
        NGUITools.SetLayer(self.body.gameObject, LayerMask.NameToLayer("Top"))

        IDMainCity.gridTileSidePorc.hide()
    end
    self.isDraged = true

    local grid = IDMainCity.getGrid()

    local inpos = MyMainCamera.lastTouchPosition
    inpos = Vector3(inpos.x, inpos.y, 0)
    local hit = Utl.getRaycastHitInfor(MyMainCamera.current.cachedCamera, inpos, Utl.getLayer("Water"))
    --local currBuildingPos = MyMainCamera.lastHit.point
    local currBuildingPos
    if hit then
        currBuildingPos = hit.point
    else
        currBuildingPos = MyMainCamera.lastHit.point
    end
    local index = grid:GetCellIndex(currBuildingPos)

    local posOffset
    if IDMainCity.isOnTheLand(index, self.size) then
        posOffset = IDMainCity.offset4Building
    else
        posOffset = IDWorldMap.offset4Ocean
    end
    local trf = self.transform
    local newPos = Vector3.zero
    if (grid:IsInBounds(index)) then
        if (self.size % 2 == 0) then
            newPos = grid:GetCellPosition(index)
        else
            newPos = grid:GetCellCenter(index)
        end
        newPos = newPos
        trf.position = newPos + posOffset
        if self.shadow then
            self.shadow.position = trf.position + Vector3.up * 0.02
        end

        local isOK = IDMainCity.isSizeInFreeCell(index, self.size, self.attr.PlaceGround, self.attr.PlaceSea)
        newPos = newPos + IDMainCity.offset4Tile
        IDLBuildingSize.show(self.size, isOK and Color.green or Color.red, newPos)
        IDLBuildingSize.setLayer("Top")
        if (isOK) then
            --self.csSelf:setMatToon()
            SFourWayArrow.setMatToon()
        else
            --SFourWayArrow.setMatToon()
            --csSelf:setMatOutLine()
        end
        if (index ~= self.oldindex) then
            self.oldindex = index
            if (isOK) then
                SoundEx.playSound("moving_07", 1, 1)
            else
                SoundEx.playSound("bad_move_06", 1, 1)
            end
        end
    end
end

function IDLBuilding:jump()
    if (self.tweenScale == nil) then
        return
    end
    self.isJump = true
    self.tweenScale:ResetToBeginning()
    self.tweenScale:Play(false)
end

function IDLBuilding:uiEventDelegate(go)
    self:_uiEventDelegate(go)
end

function IDLBuilding:_uiEventDelegate(go)
    local goName = go.name
    if (goName == "body") then
        if (self.isJump) then
            self.isJump = false
            self.csSelf:invoke4Lua(
                function()
                    self.tweenScale:Play(true)
                end,
                0.02
            )
        end
    end
end

-- 设置碰撞
function IDLBuilding:setCollider(val)
    self.csSelf.collider.enabled = val
    self.canClick = val
end

---@public 加载地表
function IDLBuilding:loadFloor()
    if self.attr.DontNeedFloor then
        return
    end
    if IDMainCity.isOnTheLand(self.gridIndex, self.size) then
        if self.floor == nil then
            CLThingsPool.borrowObjAsyn(
                "buildingFloor",
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
                end
            )
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
    if GameMode.map ~= MyCfg.mode or IDWorldMap.mode ~= GameModeSub.city then
        return
    end
    ---@type NetProtoIsland.ST_building
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
        CLUIOtherObjPool.borrowObjAsyn(
            "ProgressHud",
            function(name, obj, orgs)
                if
                    (not self.csSelf.gameObject.activeInHierarchy) or
                        self.serverData.state == IDConst.BuildingState.normal or
                        self.progressObj
                 then
                    CLUIOtherObjPool.returnObj(obj)
                    SetActive(obj, false)
                    return
                end
                self.progressObj = obj:GetComponent("CLCellLua")
                self.progressObj.transform.parent = MyCfg.self.hud3dRoot
                self.progressObj.transform.localScale = Vector3.one
                self.progressObj.transform.localEulerAngles = Vector3.zero
                SetActive(obj, true)
                self.progressObj:init({target = self.csSelf, data = self.serverData, offset = Vector3(0, 2, 0)})
            end
        )
    else
        SetActive(self.progressObj.gameObject, true)
        self.progressObj:init({target = self.csSelf, data = self.serverData, offset = Vector3(0, 2, 0)})
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
        IDMainCity.employWorker(
            self,
            function(worker)
                self.worker = worker
            end
        )
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
    IDLBuilding.super.SetActive(self, active)
    if active then
        self:upgrading()
        self:loadFloor()
        self:loadShadow()
    else
        self:fireWorker(true)
        self:unLoadProgressHud()
        if self.floor then
            SetActive(self.floor, active)
        end
        self:hideShadow()
    end
end

---@public 返回自己可被攻击的点坐标 //TODO:这个函数里算出来的点是有问题的
---@param attacker IDRoleBase
function IDLBuilding:getAttackPoint(attacker)
    local r = self.size / 2
    ---@type UnityEngine.Vector3
    local dir = attacker.transform.position - self.transform.position
    -- local topos = self.transform.position + dir.normalized * r

    -- local offsetx = attacker.csSelf:fakeRandom(-10, 10) / 10
    -- local offsetz = attacker.csSelf:fakeRandom2(-10, 10) / 10
    -- topos.x = topos.x + offsetx
    -- topos.z = topos.z + offsetz

    local angle = MyUtl.calculateAngle(attacker.transform.position, self.transform.position)
    angle = angle + attacker.csSelf:fakeRandom(-45, 45)
    local topos = AngleEx.getCirclePointV3(self.transform.position, r, angle)
    return topos
end

---@public 被击中
---@param damage number 伤害值
---@param attacker IDLUnitBase 攻击方
function IDLBuilding:onHurt(damage, attacker)
    IDLBuilding.super.onHurt(self, damage, attacker)
end

function IDLBuilding:iamDie()
    CLEffect.play("BombBuilding", self.transform.position)
    SetActive(self.gameObject, false)
    self.csSelf:clean()
end

function IDLBuilding:clean()
    IDLBuilding.super.clean(self)

    self.canClick = true
    self.isJump = false
    self:hideShadow()

    if self.floor ~= nil then
        CLThingsPool.returnObj(self.floor)
        SetActive(self.floor, false)
        self.floor = nil
    end
    self:unLoadProgressHud()
    self:fireWorker()
end

return IDLBuilding

--[[
//                    ooOoo
//                   8888888
//                  88" . "88
//                  (| -_- |)
//                  O\  =  /O
//               ____/`---'\____
//             .'  \\|     |//  `.
//            /  \\|||  :  |||//  \
//           /  _||||| -:- |||||-  \
//           |   | \\\  -  /// |   |
//           | \_|  ''\---/''  |_/ |
//            \ .-\__  `-`  ___/-. /
//         ___ `. .' /--.--\  `. . ___
//      ."" '<  `.___\_<|>_/___.'  >' "".
//     | | : ` - \`.`\ _ /`.`/- ` : | |
//     \ \ `-.    \_ __\ /__ _/   .-` / /
//======`-.____`-.___\_____/___.-`____.-'======
//                   `=---='
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//           佛祖保佑       永无BUG
//           游戏大卖       公司腾飞
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
--]]
require("public.class")
-- 建筑基础相关
---@class IDLUnitBase
IDLUnitBase = class("IDLUnitBase")

function IDLUnitBase:ctor(csSelf)
    self.csSelf = csSelf -- cs对象
    self.transform = nil
    self.gameObject = nil
    self.isOffense = false -- 是进攻方
    self.id = 0
    self.tweenScale = nil
    self.canClick = true -- 能否点击

    -----@type IDDBBuilding
    self.serverData = nil -- 服务器数据
    self.attr = nil -- 属性
    self.size = 1 -- 占格子
    self.gridIndex = -1 -- 所在格子的index
    self.oldindex = -1
    self.isFinishInited = false
end

function IDLUnitBase:__init(selfObj, other)
    if self.isFinishInited then
        return
    end
    self.isFinishInited = true
    self.csSelf = selfObj
    self.transform = selfObj.transform
    self.gameObject = selfObj.gameObject
    self.body = selfObj.mbody
    self.tweenScale = getCC(self.transform, "body", "TweenScale")
    self.door = getChild(self.transform, "door")
end

---@param other = {index=网格位置 ,serverData=服务器数据（不是必须）}
function IDLUnitBase:init(selfObj, id, star, lev, _isOffense, other)
    self:__init(selfObj, other)
    self.isOffense = _isOffense
    self.id = id

    -- 取得属性配置
    ---@type IDDBBuilding
    self.serverData = other.serverData
    self.attr = DBCfg.getBuildingByID(id)
    self.size = bio2Int(self.attr.Size)
    self.gridIndex = other.index
    self.oldindex = self.gridIndex
    self:loadShadow()
end

---@public 加载影子
function IDLUnitBase:loadShadow()
    if self.shadow == nil then
        local shadowType = bio2number(self.attr.ShadowType)
        if shadowType < 1 then
            return
        end
        local shadowName = joinStr("shadow", shadowType)
        CLUIOtherObjPool.borrowObjAsyn(
            shadowName,
            function(name, obj, orgs)
                if (not self.gameObject.activeInHierarchy) or self.shadow ~= nil then
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
        self.shadow.position = self.transform.position + Vector3.up * 0.02
        SetActive(self.shadow.gameObject, true)
    end
end

function IDLUnitBase:clean()
    self:_clean()
end
function IDLUnitBase:_clean()
    self.canClick = true
    self.isJump = false

    if self.shadow then
        CLUIOtherObjPool.returnObj(self.shadow.gameObject)
        SetActive(self.shadow.gameObject, false)
        self.shadow = nil
    end
end

-- 可以放下
function IDLUnitBase:isCanPlace(...)
    local grid = IDMainCity.getGrid()
    local index = grid:GetCellIndex(self.transform.position)
    if (IDMainCity.isSizeInFreeCell(index, self.size, self.attr.PlaceGround, self.attr.PlaceSea)) then
        return true
    end
    return false
end

-- 当点击角色时
function IDLUnitBase:OnClick(...)
    self:_OnClick()
end
function IDLUnitBase:_OnClick(...)
    if (not self.canClick) then
        return
    end

    if (MyCfg.mode == GameMode.battle) then
        -- 通知战场，玩家点击了我
        IDLBattle.onClickSomeObj(self.csSelf, self.transform.position)
    else
        SoundEx.playSound(self.attr.SelectSound, 1, 1)
        self:jump()
        IDMainCity.onClickBuilding(self)
    end
end

function IDLUnitBase:OnPress(go, isPress)
    self:_OnPress(go, isPress)
end

function IDLUnitBase:_OnPress(go, isPress)
    if (not self.canClick) then
        return
    end
    if (IDMainCity.selectedUnit ~= self) then
        return
    end
    if (MyCfg.mode == GameMode.battle) then
        -- 通知战场，玩家按下了我
        CLLBattle.onPressRole(isPress, self.csSelf, self.transform.position)
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

function IDLUnitBase:setScreenCanDrag()
    CLUIDrag4World.self.enabled = true --可托动屏幕
end

function IDLUnitBase:onShowingGrid(...)
    if (not self.isDraged) then
        IDMainCity.hideGrid() --显示网格
    end
end

-- 拖动
function IDLUnitBase:OnDrag(go, delta)
    self:_OnDrag(delta)
end

-- 拖动
function IDLUnitBase:_OnDrag(delta)
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

function IDLUnitBase:jump()
    if (self.tweenScale == nil) then
        return
    end
    self.isJump = true
    self.tweenScale:ResetToBeginning()
    self.tweenScale:Play(false)
end

function IDLUnitBase:uiEventDelegate(go)
    self:_uiEventDelegate(go)
end

function IDLUnitBase:_uiEventDelegate(go)
    local goName = go.name
    if (goName == "body") then
        if (self.isJump) then
            self.isJump = false
            self.tweenScale:Play(true)
        end
    end
end

-- 设置碰撞
function IDLUnitBase:setCollider(val)
    self.csSelf.collider.enabled = val
    self.canClick = val
end

function IDLUnitBase:SetActive(active)
    if self.shadow then
        if active then
            self.shadow.position = self.transform.position + Vector3.up * 0.02
        end
        SetActive(self.shadow.gameObject, active)
    end
    SetActive(self.gameObject, active)
end

return IDLUnitBase

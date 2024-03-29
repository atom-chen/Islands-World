﻿---@class IDLTile
local cell = {}

local mData

function cell.init(selfObj)
    cell.csSelf = selfObj
    cell.transform = selfObj.transform
    cell.gameObject = selfObj.gameObject
    cell.body = cell.gameObject
    ---@type TweenScale
    cell.tweenScale = cell.csSelf:GetComponent("TweenScale")
    cell.collider = cell.csSelf:GetComponent("BoxCollider")
end

function cell.show(go, data)
    mData = data
    cell.mData = data
    cell.size = 2
    cell.gridIndex = bio2number(mData.pos)
    cell.oldindex = cell.gridIndex
    cell.isTile = true
    cell.isJump = false
    cell.isDraged = false
    cell.canClick = true
    cell.isSelected = false
    cell.initSides()
end

function cell.getPosIndex()
    local grid = IDMainCity.getGrid()
    local index = grid:GetCellIndex(cell.transform.position)
    return index
end

function cell.OnClick()
    if (not cell.canClick) then
        return
    end

    if (MyCfg.mode == GameMode.battle) then
        -- 通知战场，玩家点击了我
        IDLBattle.onClickSomeObj(cell, cell.transform.position)
    else
        --SoundEx.playSound(self.attr.SelectSound, 1, 1)
        IDMainCity.onClickTile(cell)
    end
end

function cell.OnPress(go, isPress)
    if (not cell.canClick) then
        return
    end
    if (IDMainCity.selectedUnit ~= cell) then
        return
    end
    if (MyCfg.mode == GameMode.battle) then
        -- 通知战场，玩家按下了我
        CLLBattle.onPressRole(isPress, cell, cell.transform.position)
    else
        if (isPress) then
            IDMainCity.setOtherUnitsColiderState(cell, false)
            CLUIDrag4World.self.enabled = false --不可托动屏幕
        else
            cell.isDraged = false

            cell.jump()
            local grid = IDMainCity.getGrid()
            IDMainCity.hideGrid() -- 网格不显示
            local moved = false
            local index = grid:GetCellIndex(cell.transform.position)
            if (IDMainCity.canPlaceTile(index)) then
                moved = (cell.gridIndex ~= index)
                cell.gridIndex = index
                IDMainCity.setOtherUnitsColiderState(nil, true)

                IDLCameraMgr.setPostProcessingProfile("normal")
                cell.initSides()
            end
            --//TODO: 本来是当拖放的位置正好可以放下时才改变成tile layer，但是如果不改的话就没有办法触发onpress方法了，所以暂时这样处理吧
            NGUITools.SetLayer(cell.gameObject, LayerMask.NameToLayer("Tile"))
            -- 通知主城有释放建筑
            IDMainCity.onReleaseTile(cell, moved)
            cell.csSelf:invoke4Lua(cell.setScreenCanDrag, 0.2)
            -- CLUIDrag4World.self.enabled = true --可托动屏幕
        end
    end
end

function cell.setScreenCanDrag()
    CLUIDrag4World.self.enabled = true --可托动屏幕
end

function cell.OnDrag(go, delta)
    if (IDMainCity.selectedUnit ~= cell) then
        return
    end
    if not cell.isDraged then
        IDMainCity.showGrid(cell.onShowingGrid) --显示网格
        IDLCameraMgr.setPostProcessingProfile("gray")
        NGUITools.SetLayer(cell.csSelf.gameObject, LayerMask.NameToLayer("Top"))
        IDMainCity.gridTileSidePorc.clean()
    end
    cell.isDraged = true

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
    -- printe(index)
    local trf = cell.transform
    local newPos = Vector3.zero
    if (grid:IsInBounds(index)) then
        if (index ~= cell.oldindex) then
            cell.oldindex = index
            if (cell.size % 2 == 0) then
                newPos = grid:GetCellPosition(index)
            else
                newPos = grid:GetCellCenter(index)
            end
            newPos = newPos + IDMainCity.offset4Tile
            trf.position = newPos
            if cell.shadow then
                cell.shadow.transform.position = trf.position - Vector3.up * 5
            end

            local isOK = IDMainCity.canPlaceTile(index)
            newPos.y = newPos.y + 0.1
            --IDLBuildingSize.show(cell.size, isOK and Color.green or Color.red, newPos)
            --IDLBuildingSize.setLayer("Top")
            if (isOK) then
                --cell.csSelf:setMatToon()
                SFourWayArrow.setMatToon(Color.white)
                SoundEx.playSound("moving_07", 1, 1)
            else
                --csSelf:setMatOutLine()
                SFourWayArrow.setMatToon(Color.red)
                SoundEx.playSound("bad_move_06", 1, 1)
            end
        end
    end
end

function cell.onShowingGrid(...)
    if (not cell.isDraged) then
        IDMainCity.hideGrid() --显示网格
    end
end

function cell.uiEventDelegate(go)
    local goName = go.name
    if (goName == cell.csSelf.name) then
        if (cell.isJump) then
            cell.isJump = false
            cell.csSelf:invoke4Lua(
                function()
                    cell.tweenScale:Play(true)
                end,
                0.02
            )
        end
    end
end

function cell.clean()
    cell.isJump = false
    cell.setCollider(true)
end

function cell.jump()
    if (cell.tweenScale == nil) then
        return
    end
    cell.isJump = true
    cell.tweenScale:ResetToBeginning()
    cell.tweenScale:Play(false)
end

-- 设置碰撞
function cell.setCollider(val)
    cell.collider.enabled = val
    cell.canClick = val
end

function cell.initSides()
    local grid = IDMainCity.getGrid()
    local index = cell.gridIndex
    local x = grid:GetColumn(index)
    local y = grid:GetRow(index)
    local left1 = grid:GetCellIndex(x - 2, y)
    local left2 = grid:GetCellIndex(x - 2, y - 1)
    local right1 = grid:GetCellIndex(x + 1, y)
    local right2 = grid:GetCellIndex(x + 1, y - 1)
    local up1 = grid:GetCellIndex(x - 1, y + 1)
    local up2 = grid:GetCellIndex(x, y + 1)
    local down1 = grid:GetCellIndex(x - 1, y - 2)
    local down2 = grid:GetCellIndex(x, y - 2)
    local leftUp = grid:GetCellIndex(x - 2, y + 1)
    local leftDown = grid:GetCellIndex(x - 2, y - 2)
    local rightUp = grid:GetCellIndex(x + 1, y + 1)
    local rightDown = grid:GetCellIndex(x + 1, y - 2)
    cell.sides = {left1, left2, right1, right2, up1, up2, down1, down2, leftUp, leftDown, rightUp, rightDown}
end

---@public 取得四边的index
function cell.getSidesIndex()
    return cell.sides
end

function cell.getSidesAngleIndex()
    return cell.sides[9], cell.sides[10], cell.sides[11], cell.sides[12] --leftUp, leftDown, rightUp, rightDown
end

return cell

---@class IDWorldFleet 行军部队
local _cell = {}
---@type Coolape.CLBaseLua
local csSelf = nil
---@type UnityEngine.Transform
local transform = nil
local gameObject = nil
---@type Coolape.CLGridPoints
local pointRoot
local shipsCount = 0
---@type NetProtoIsland.ST_fleetinfor
local mData = nil
local fromPos
local toPos
local timeLen
local distance
---@type MyDirectionArrow
local dirLine
local shipsCount = 5
local ships = {}
local isFinishInited = false
local isLoadedships = false
local isMoving = false
--local marchFighting
local OffsetTime = 0
local roleHeader
local moveSpeed = 1
local atleaseTime = 0 -- 至少需要多少时间
local realFromePos
---@type SimpleFogOfWar.FogOfWarInfluence
local influence = nil
local isMyFleet = false

local tips = {}

-- 初始化，只调用一次
function _cell.init(csObj)
    csSelf = csObj
    transform = csSelf.transform
    _cell.transform = transform
    gameObject = csSelf.gameObject
    _cell.gameObject = gameObject
    pointRoot = getCC(transform, "grid", "CLGridPoints")
    influence = csSelf:GetComponent("FogOfWarInfluence")
    influence.ViewDistance = 150
    moveSpeed = bio2number(DBCfg.getConstCfg().FleetMoveSpeed) * 1000
    atleaseTime = bio2number(DBCfg.getConstCfg().FleetAtLeastSec) * 1000
end

function _cell.show(go, data)
    -- set data
    _cell.refreshData(data)
end
function _cell.refreshData(data)
    mData = data
    _cell.data = mData

    if bio2number(mData.cidx) == bio2number(IDDBCity.curCity.idx) then
        influence.Suspended = false
        isMyFleet = true
    else
        influence.Suspended = true
        isMyFleet = false
    end

    local fromIndex = bio2number(mData.frompos)
    local toIndex = bio2number(mData.topos)

    if mData.fromposv3 and mData.fromposv3.x then
        fromPos =
            Vector3(
            bio2number(mData.fromposv3.x) / 1000,
            bio2number(mData.fromposv3.y) / 1000,
            bio2number(mData.fromposv3.z) / 1000
        )
        fromPos = fromPos
    else
        fromPos = IDWorldMap.grid.grid:GetCellCenter(fromIndex)
    end
    -- fromPos = IDWorldMap.grid.grid:GetCellCenter(fromIndex)
    toPos = IDWorldMap.grid.grid:GetCellCenter(toIndex)
    -- 转换成单元格的距离，而非真实的距离
    local dis = Vector3.Distance(fromPos, toPos) / IDWorldMap.grid.cellSize
    timeLen = dis * moveSpeed -- bio2number(mData.arrivetime) - bio2Long(mData.beginTime)
    timeLen = timeLen < atleaseTime and atleaseTime or timeLen -- 至少要5秒
    mData.beginTime = bio2number(mData.arrivetime) - timeLen
    distance = toPos - fromPos
    Utl.RotateTowards(transform, distance)

    --可以处理fixed update
    --csSelf.canFixedInvoke = true
    roleHeader = nil
    -- 设置阵型的兵(里面会设置hudAnchor)
    _cell.setCamp(
        function()
            -- 画线
            if bio2number(mData.status) == IDConst.FleetState.moving then
                _cell.drawDirLine()
                _cell.doMarch()
            else
                transform.position = IDWorldMap.grid.grid:GetCellCenter(bio2number(mData.curpos))
            end

            -- csSelf:invoke4Lua(_cell.showTips, NumEx.NextInt(2, 5))
        end
    )
end

-- 设置阵型
function _cell.setCamp(callback)
    if isLoadedships then
        if #ships >= shipsCount then
            Utl.doCallback(callback)
        end
        return
    end
    isLoadedships = true

    local points = {}
    local num = shipsCount

    ----------------------
    -- 刷新部队中兵的间隙
    -- local interval = 1
    if (num == 1) then
        points[0] = pointRoot.vectors[4]
    elseif (num == 2) then
        points[0] = pointRoot.vectors[3]
        points[1] = pointRoot.vectors[4]
    elseif (num == 3) then
        points = {}
        points[0] = pointRoot.vectors[0]
        points[1] = pointRoot.vectors[2]
        points[2] = pointRoot.vectors[3]
    elseif (num == 5) then
        points = {}
        points[0] = pointRoot.vectors[0]
        points[1] = pointRoot.vectors[2]
        points[2] = pointRoot.vectors[3]
        points[3] = pointRoot.vectors[4]
        points[4] = pointRoot.vectors[5]
    elseif (num == 7) then
        points = {}
        points[0] = pointRoot.vectors[0]
        points[1] = pointRoot.vectors[2]
        points[2] = pointRoot.vectors[3]
        points[3] = pointRoot.vectors[4]
        points[4] = pointRoot.vectors[5]
        points[5] = pointRoot.vectors[6]
        points[6] = pointRoot.vectors[7]
    else
        points = pointRoot.vectors
    end

    -- 因为已经排好了位置，不需要在代码里重新计算位置了
    --pointRoot.widthInterval = interval
    --pointRoot.lengthInterval = interval
    --pointRoot:refreshPoint()
    ----------------------
    shipsCount = num
    for i = 0, shipsCount - 1 do
        -- if i == 0 and shipsCount > 6 then
        --     local initParas = {i, points[i], callback}
        --     -- 透传下小兵的传数
        --     CLRolePool.borrowObjAsyn(role8Name, _cell.onGetShip, initParas)
        -- else
        local initParas = {i, points[i], callback} -- 透传下小兵的参数
        CLRolePool.borrowObjAsyn("ship5", _cell.onGetShip, initParas)
        -- end
    end
end

function _cell.isOffense()
    local isOff = false
    if bio2number(mData.cidx) == bio2number(IDDBCity.curCity.idx) then
        isOff = true
    end
    return isOff
end

function _cell.onGetShip(name, go, orgs)
    if go == nil then
        return
    end

    if
        (not isLoadedships) or IDWorldMap == nil or
            (IDWorldMap.mode ~= GameModeSub.map and IDWorldMap.mode ~= GameModeSub.fleet) or
            (not csSelf.gameObject.activeInHierarchy)
     then
        CLRolePool.returnObj(go)
        SetActive(go.gameObject, false)
        return
    end
    ---@type MyUnit
    local role = go
    local i = orgs[1]
    local point = orgs[2]
    local callback = orgs[3]
    role.transform.parent = transform
    role.transform.localPosition = point
    role.transform.localScale = Vector3.one
    -- if (role.transform.parent ~= nil) then
    --     NGUITools.SetLayer(role.gameObject, role.transform.parent.gameObject.layer)
    -- end
    role.transform.localEulerAngles = Vector3.zero

    SetActive(role.gameObject, true)
    -- 把自身设置给role
    if role.luaTable == nil then
        ---@type IDRoleBase
        role.luaTable = IDUtl.newRoleLua(5)
        role:initGetLuaFunc()
    end
    role:init(5, 0, 1, _cell.isOffense(), {serverData = {}})

    if i == 0 then
        roleHeader = role
    end
    table.insert(ships, role)
    if #ships >= shipsCount then
        Utl.doCallback(callback)
    end
end

function _cell.showTips()
    if (not csSelf.gameObject.activeInHierarchy) or MyCfg.self.isGuidMode then
        return
    end
    if roleHeader then
        roleHeader.luaTable.showAirBall(tips[NumEx.NextInt(1, #tips + 1)], Vector3(0, 0.4, 0), 0.8)
        csSelf:invoke4Lua(_cell.hideTips, NumEx.NextInt(5, 10))
    end
    csSelf:invoke4Lua(_cell.showTips, nil, NumEx.NextInt(20, 50), true)
end

function _cell.hideTips()
    if roleHeader then
        roleHeader.luaTable.hideAirBall()
    end
end

function _cell.getData()
    return mData
end

function _cell.doMarch()
    csSelf:cancelInvoke4Lua(_cell._doAttack)
    InvokeEx.cancelInvokeByUpdate(_cell.doRefreshPosition)
    isMoving = true
    _cell.doRefreshPosition()
end

function _cell.doRefreshPosition()
    InvokeEx.invokeByUpdate(_cell.doRefreshPosition, 0.2)
    _cell.refreshPosition()
    if isMyFleet then
        IDWorldMap.onDragMove()
    end
end

-- 刷新位置
function _cell.refreshPosition()
    -- 减一秒，保证本地的进度比服务器慢
    local nowDate = DateEx.nowMS + OffsetTime
    local percent = (nowDate - mData.beginTime) / timeLen
    if percent < 0 then
        percent = 0
    end

    if percent >= 1 then
        percent = 1
        isMoving = false
        local pos = fromPos + distance * percent
        transform.position = pos
        -- 说明已经到目标位置，播放攻击动作

        _cell.onArrived()
    else
        local pos = fromPos + distance * percent
        transform.position = pos
    end
end

-- function _cell.setAction(actionName)
--     for i, v in ipairs(ships) do
--         if v.luaTable and type(v.luaTable) == "table" then
--             v.luaTable.setAction(actionName, NumEx.NextInt(0, 50) / 100)
--         end
--     end
-- end

function _cell.doAttack()
    local cellIndex = bio2Int(mData.targetMapidx)
    local target = KKWorldMap.getCellByIndex(cellIndex)
    if target == nil then
        return
    end

    _cell._doAttack()
end

function _cell._doAttack()
    -- _cell.setAction("attack")
    csSelf:invoke4Lua(_cell._doAttack, 1)
end

-- 清除数据，最好不要直接调用lua的clean，而是调用cs对象的clean。因为cs那边还有做其它的清理工作
function _cell.clean()
    InvokeEx.cancelInvokeByUpdate(_cell.doRefreshPosition)
    csSelf:cancelInvoke4Lua()
    for i, v in ipairs(ships) do
        if v then
            v:clean()
            CLRolePool.returnObj(v)
            SetActive(v.gameObject, false)
        end
    end
    ships = {}
    isLoadedships = false

    _cell.releaseLine()
    --if marchFighting ~= nil then
    --    SetActive(marchFighting.gameObject, false)
    --    CLUIOtherObjPool.returnObj(marchFighting.gameObject)
    --    marchFighting = nil
    --end
end

function _cell.releaseLine()
    if dirLine ~= nil then
        CLThingsPool.returnObj(dirLine.gameObject)
        SetActive(dirLine.gameObject, false)
        dirLine = nil
    end
end

function _cell.drawDirLine()
    local onLoadLine = function(name, obj, orgs)
        -- 保证只能加载一次（因为init会调用多次）
        if MyCfg.mode ~= GameMode.map or dirLine ~= nil or (not gameObject.activeInHierarchy) then
            CLThingsPool.returnObj(obj)
            SetActive(obj, false)
            return
        end
        if obj == nil or obj:IsNull() then
            return
        end
        dirLine = obj:GetComponent("MyDirectionArrow")
        _cell.showingLine()
    end

    if dirLine == nil then
        CLThingsPool.borrowObjAsyn("dirLine", onLoadLine)
    else
        _cell.showingLine()
    end
end

function _cell.showingLine()
    if dirLine == nil then
        return
    end
    local color = Color.green
    SetActive(dirLine.gameObject, true)
    dirLine:init(0.6, 0.6, color, color, 60)
    dirLine.line.positionCount = 2
    dirLine:SetPosition(fromPos, toPos)
    -- invoke 目的是为了重新设置直线的mainTextureScale
    csSelf:invoke4Lua(
        function()
            if dirLine then
                dirLine:SetPosition(fromPos, toPos)
            end
        end,
        0.2
    )
end

-- 队伍是空闲，canWalkAround＝true时，小兵可以在一定范围内到处走动
function _cell.IamIdel(canWalkAround)
    _cell.stopMove()
    canWalkAround = canWalkAround == nil and false or canWalkAround
    if (canWalkAround) then
        csSelf.state = RoleState.walkAround
    else
        csSelf.state = RoleState.idel
    end

    for i, v in ipairs(ships) do
        v.luaTable.IamIdel(canWalkAround)
    end
end

function _cell.stopMove(...)
    --csSelf:cancelFixedInvoke4Lua(_cell.reMoveTo)
    csSelf.aiPath:stop()
    for i, v in ipairs(ships) do
        v.luaTable.stopMove()
    end
end

function _cell.moveToTarget(target)
    csSelf:moveToTarget(target)
    for i, v in ipairs(ships) do
        v:moveToTarget(target)
    end
end

-- 移动到指定的位置
function _cell.moveTo(toPos)
    --csSelf:cancelFixedInvoke4Lua(_cell._doAttack)
    --csSelf:cancelFixedInvoke4Lua(_cell.setCanMove)
    --csSelf:cancelFixedInvoke4Lua(_cell.doSearchTarget)
    local dis = toPos - transform.position
    dis.y = 0
    Utl.RotateTowards(transform, dis)

    csSelf:moveTo(toPos)
end

-- 当完成寻路
function _cell.onPathComplete(p)
    -- 士兵也可以移动了
    local dis = csSelf.aiPath.targetPosition - transform.position
    local pos = nil
    for i, v in ipairs(ships) do
        pos = v.luaTable.getPosition(20)
        v.luaTable.moveTo(pos + dis)
    end
    csSelf.aiPath.canMove = true
end

function _cell.onMoving()
end

function _cell.onArrived()
    InvokeEx.cancelInvokeByUpdate(_cell.doRefreshPosition)
    local nowDate = DateEx.nowMS
    local marchType = bio2Int(mData.task)
    local marchState = bio2Int(mData.status)
    isMoving = false
    _cell.releaseLine()
    _cell.onFinishArrived()
end

function _cell.checkState(marchIdx)
    if MyCfg.mode ~= GameMode.map or csSelf == nil then
        return
    end
    local march = KKDBWorldMap.getMarchInfoByIdx(marchIdx)
    if march == nil or march.data == nil then
        return
    end
    --printe("@@@@@@@11==" .. bio2Int(march.data.marchType))
    --printe("@@@@@@@====" .. bio2Int(march.data.status))
    if bio2Int(march.data.marchType) == 3 and bio2Int(march.data.status) == 2 then
        -- 3进攻野地 and 2驻扎(驻扎不用)
        pcall(_cell.notifyMenu, KKPWorldMap.getLindiButtonIcon(), "mainCity_004lindi")
    end
end

function _cell.onFinishArrived()
    --已经超过x秒
    -- if not isMoving then
    --     _cell.setAction("idel")
    -- end
    IDWorldMap.onSomeFleetArrived(_cell)
end

-- 通知查看邮件
function _cell.notifyMenu(to, icon)
    if KKDBUser.isNeedGuidNewPlayer() then
        return
    end
    --local to = KKPWorldMap.getMailButtonIcon()
    if to == nil then
        return
    end
    local from = csSelf or MyMain.self
    local d = {from = from, to = to, offset = Vector3.zero, icon = icon}
    CLUIOtherObjPool.borrowObjAsyn(
        "CellNotifyTweenIcon",
        function(name, obj, orgs)
            if MyCfg.mode == GameMode.none or (not csSelf.gameObject.activeInHierarchy) then
                CLUIOtherObjPool.returnObj(obj)
                SetActive(obj, false)
                return
            end
            obj.transform.parent = CLUIInit.self.uiPublicRoot
            obj.transform.localScale = Vector3.one
            local tweenIcon = obj:GetComponent("CLCellLua")
            tweenIcon:init(d, nil)
            SetActive(obj, true)
        end
    )
end

function _cell.setRolesCameraLayer(camera, layer)
    for i, v in ipairs(ships) do
        v.luaTable.setRolesCameraLayer(camera, layer)
    end
end
--------------------------------------------
return _cell

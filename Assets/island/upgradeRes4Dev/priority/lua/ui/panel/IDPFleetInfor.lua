---@class _ParamIDPFleetInfor
---@field public parent IDPFleets
---@field public fleetidx number 舰队idx
---@field public toPos number 目标坐标

---@class IDPFleetInfor
local IDPFleetInfor = {}
local transform
---@type _ParamIDPFleetInfor
local mData
local uiobjs = {}
local cells = {}
---@type NetProtoIsland.ST_fleetinfor
local fleetInfor
local isShowing = false
local mode = 0 -- 0:hide 1:show

-- 初始化，只会调用一次
function IDPFleetInfor.init(csObj)
    ---@type Coolape.CLCellLua
    IDPFleetInfor.csSelf = csObj
    transform = csObj.transform
    ---@type UIGrid
    uiobjs.gridShips = getCC(transform, "PanelShips/GridShips", "UIGrid")
    uiobjs.gridShipsPrefab = getChild(uiobjs.gridShips.transform, "00000").gameObject
    ---@type TweenPosition
    uiobjs.tweenPosition = IDPFleetInfor.csSelf:GetComponent("TweenPosition")
    ---@type UIInput
    uiobjs.InputName = getCC(transform, "InputName", "UIInput")
    ---@type UIGrid
    uiobjs.buttons = getCC(transform, "buttons", "UIGrid")
    uiobjs.ButtonSaveLb = getCC(uiobjs.buttons.transform, "ButtonSave/Label", "UILabel")
    uiobjs.ButtonDepart = getChild(uiobjs.buttons.transform, "ButtonDepart").gameObject
    uiobjs.ButtonGoto = getChild(uiobjs.buttons.transform, "ButtonGoto").gameObject
    uiobjs.ButtonBack = getChild(uiobjs.buttons.transform, "ButtonBack").gameObject
end

function IDPFleetInfor.reset()
    uiobjs.tweenPosition:ResetToBeginning()
end

-- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
function IDPFleetInfor.show(go, data)
    mData = data or {}
    mode = 1
    if isShowing then
        isShowing = false
        uiobjs.tweenPosition:Play(false)
    else
        isShowing = true
        uiobjs.tweenPosition:Play(true)
    end
    IDPFleetInfor.refresh()
end

function IDPFleetInfor.refresh()
    if mData.fleetidx and mData.fleetidx > 0 then
        fleetInfor = IDDBCity.curCity.fleets[mData.fleetidx]
    else
        fleetInfor = nil
    end
    if mData.toPos and mData.toPos >= 0 then
        -- 说明是需要出征的操作
        if fleetInfor then
            SetActive(uiobjs.ButtonDepart, true)
        else
            SetActive(uiobjs.ButtonDepart, false)
        end
        SetActive(uiobjs.ButtonGoto, false)
    else
        SetActive(uiobjs.ButtonDepart, false)
        if mData.fleetidx and mData.fleetidx > 0 then
            -- 说明是在已有舰队上修改
            SetActive(uiobjs.ButtonGoto, true)
            SetActive(uiobjs.ButtonBack, true)
            uiobjs.ButtonSaveLb.text = LGet("SaveConfig")
        else
            SetActive(uiobjs.ButtonGoto, false)
            SetActive(uiobjs.ButtonBack, false)
            uiobjs.ButtonSaveLb.text = LGet("NewFleet")
        end
    end
    uiobjs.buttons.repositionNow = true
    uiobjs.InputName.value = fleetInfor and fleetInfor.name or "New Fleet"

    cells = {}
    CLUIUtl.resetList4Lua(
        uiobjs.gridShips,
        uiobjs.gridShipsPrefab,
        IDPFleetInfor.wrapShipList(),
        IDPFleetInfor.initShipCell
    )
end

function IDPFleetInfor.wrapShipList()
    local ret = {}
    ---@type _ParamCellSetShipNum
    local d

    local fleetsMap = {}
    ---@param v NetProtoIsland.ST_unitInfor
    for i, v in ipairs(fleetInfor and fleetInfor.units or {}) do
        fleetsMap[bio2number(v.id)] = v
    end

    local cityShipMap = IDDBCity.curCity:getAllShips()
    local list = DBCfg.getListByGID(DBCfg.CfgPath.Role, IDConst.RoleGID.ship)
    ---@param v DBCFRoleData
    for i, v in ipairs(list) do
        local id = bio2number(v.ID)
        if cityShipMap[id] then
            d = {}
            d.id = id
            if fleetsMap[id] then
                d.setNum = bio2number(fleetsMap[id].num)
            else
                d.setNum = 0
            end
            d.hadNum = cityShipMap[id] + d.setNum
            table.insert(ret, d)
        elseif fleetsMap[id] then
            d = {}
            d.id = id
            d.setNum = bio2number(fleetsMap[id].num)
            d.hadNum = d.setNum
            table.insert(ret, d)
        end
    end
    return ret
end

function IDPFleetInfor.initShipCell(cell, data)
    cell:init(data)
    table.insert(cells, cell.luaTable)
end

function IDPFleetInfor.hide()
    mode = 0
    if isShowing then
        uiobjs.tweenPosition:Play(false)
    end
    ---@param v IDCellSetShipNum
    for i, v in ipairs(cells) do
        v.hide()
    end
    cells = {}
end

-- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
function IDPFleetInfor.procNetwork(cmd, succ, msg, paras)
    --[[
    if(succ == NetSuccess) then
      if(cmd == "xxx") then
        -- TODO:
      end
    end
    --]]
end

function IDPFleetInfor.wrapSetedFleetsData()
    local ret = {}
    ---@type NetProtoIsland.ST_unitInfor
    local unit
    ---@param v IDCellSetShipNum
    for i, v in ipairs(cells) do
        local d, setNum = v.getData()
        if setNum > 0 then
            unit = {}
            unit.id = d.id
            unit.num = setNum
            unit.fidx = mData.fleetidx or 0
            unit.bidx = 0
            table.insert(ret, unit)
        end
    end
    return ret
end

-- 处理ui上的事件，例如点击等
function IDPFleetInfor.uiEventDelegate(go)
    local goName = go.name
    if goName == "SpriteClose" then
        if mData.parent then
            mData.parent.onHideFleetInfor()
        end
        IDPFleetInfor.hide()
    elseif goName == "ButtonSave" then
        local name = trim(uiobjs.InputName.value)
        local newName = cutStr_utf8(name, 14)
        if name ~= newName then
            CLAlert.add(LGetFmt("MsgOverLength", LGet("FleetName"), 7, 14), Color.yellow, 1)
            return
        end
        local fleets = IDPFleetInfor.wrapSetedFleetsData()
        if #fleets == 0 then
            CLAlert.add(LGet("MsgShipCfgIsNil"), Color.yellow, 1)
            return
        end
        local fidx = fleetInfor and bio2number(fleetInfor.idx) or 0
        showHotWheel()
        CLLNet.send(
            NetProtoIsland.send.saveFleet(
                bio2number(IDDBCity.curCity.idx),
                fidx,
                name,
                fleets,
                function(orgs, resutl)
                    hideHotWheel()
                    IDPFleetInfor.refresh()
                    CLAlert.add(LGet("MsgSuccess"), Color.green, 1)
                end
            )
        )
    elseif goName == "ButtonDepart" then
        if fleetInfor == nil then
            return
        end
        showHotWheel()
        CLLNet.send(
            NetProtoIsland.send.fleetDepart(
                bio2number(fleetInfor.idx),
                mData.toPos,
                ---@param result NetProtoIsland.RC_fleetDepart
                function(orgs, result)
                    hideHotWheel()
                    local p = CLPanelManager.getPanel("PanelFleets")
                    if p then
                        hideTopPanel(p)
                    end
                    --//TODO:
                end
            )
        )
    elseif goName == "ButtonGoto" then
    elseif goName == "PanelFleetInfor" then
        IDPFleetInfor.csSelf:invoke4Lua(
            function()
                if mode == 1 and (not isShowing) then
                    isShowing = true
                    uiobjs.tweenPosition:Play(true)
                end
            end,
            0.1
        )
    end
end

--------------------------------------------
return IDPFleetInfor

---@class _ParamIDPFleetInfor
---@field public fleetidx number 舰队idx
---@field public toPos number 目标坐标

---@class IDPFleetInfor
local IDPFleetInfor = {}
local transform
---@type _ParamIDPFleetInfor
local mData
local uiobjs = {}
local cells = {}

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
    uiobjs.ButtonSaveLb = getCC(transform, "ButtonSave/Label", "UILabel")
    uiobjs.ButtonDepart = getChild(transform, "ButtonDepart").gameObject
    uiobjs.ButtonGoto = getChild(transform, "ButtonGoto").gameObject
end

function IDPFleetInfor.reset()
    uiobjs.tweenPosition:ResetToBeginning()
end

-- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
function IDPFleetInfor.show(go, data)
    mData = data or {}
    uiobjs.tweenPosition:Play(true)
    IDPFleetInfor.refresh()
end

function IDPFleetInfor.refresh()
    if mData.toPos and mData.toPos >= 0 then
        -- 说明是需要出征的操作
        SetActive(uiobjs.ButtonDepart, true)
        SetActive(uiobjs.ButtonGoto, false)
    else
        SetActive(uiobjs.ButtonDepart, false)
        if mData.fleetidx and mData.fleetidx > 0 then
            -- 说明是在已有舰队上修改
            SetActive(uiobjs.ButtonGoto, true)
        else
            SetActive(uiobjs.ButtonGoto, false)
        end
    end

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
    local map = IDDBCity.curCity:getAllShips()
    local list = DBCfg.getListByGID(DBCfg.CfgPath.Role, IDConst.RoleGID.ship)
    ---@param v DBCFRoleData
    for i, v in ipairs(list) do
        local id = bio2number(v.ID)
        if map[id] then
            d = {}
            d.id = id
            d.hadNum = map[id]
            d.setNum = 0
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
    uiobjs.tweenPosition:Play(false)
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
        showHotWheel()
        CLLNet.send(
            NetProtoIsland.send.saveFleet(
                bio2number(IDDBCity.curCity.idx),
                0,
                name,
                fleets,
                function(orgs, resutl)
                    hideHotWheel()
                end
            )
        )
    elseif goName == "ButtonDepart" then
    elseif goName == "ButtonGoto" then
    end
end

--------------------------------------------
return IDPFleetInfor

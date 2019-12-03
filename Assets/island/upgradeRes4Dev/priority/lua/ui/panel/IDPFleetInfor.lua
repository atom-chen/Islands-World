---@type IDBasePanel
local IDBasePanel = require("ui.panel.IDBasePanel")
---@class IDPFleetInfor:IDBasePanel 舰队信息界面
local IDPFleetInfor = class("IDPFleetInfor", IDBasePanel)

-- 初始化，只会调用一次
function IDPFleetInfor:init(csObj)
    IDPFleetInfor.super.init(self, csObj)
    self.uiobjs = {}
    self.uiobjs.gridShips = getCC(self.transform, "PanelShips/GridShips", "UIGrid")
    self.uiobjs.gridShipsPrefab = getChild(self.uiobjs.gridShips.transform, "00000").gameObject
end

-- 设置数据
function IDPFleetInfor:setData(paras)
end

-- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
function IDPFleetInfor:show()
    CLUIUtl.resetList4Lua(
        self.uiobjs.gridShips,
        self.uiobjs.gridShipsPrefab,
        self:wrapShipList(),
        self:wrapFunc(self.initShipCell)
    )
end

function IDPFleetInfor:wrapShipList()
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

function IDPFleetInfor:initShipCell(cell, data)
    cell:init(data)
end

-- 刷新
function IDPFleetInfor:refresh()
end

-- 关闭页面
function IDPFleetInfor:hide()
end

-- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
function IDPFleetInfor:procNetwork(cmd, succ, msg, paras)
    --[[
    if(succ == NetSuccess) then
      if(cmd == "xxx") then
        -- TODO:
      end
    end
    --]]
end

-- 处理ui上的事件，例如点击等
function IDPFleetInfor:uiEventDelegate(go)
    local goName = go.name
    --[[
    if(goName == "xxx") then
      --TODO:
    end
    --]]
end

-- 当顶层页面发生变化时回调
function IDPFleetInfor:onTopPanelChange(topPanel)
end
--------------------------------------------
return IDPFleetInfor

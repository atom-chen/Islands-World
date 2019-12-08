---@class _ParamIDPFleets 调用参数
---@field toPos number 目标位置 如果不传时，则认为是查看舰队信息，否则认为是出征

---@class IDPFleets 舰队界面
local IDPFleets = {}

---@type Coolape.CLPanelLua
local csSelf = nil
---@type UnityEngine.Transform
local transform = nil
local uiobjs = {}
---@type _ParamIDPFleets
local mData

-- 初始化，只会调用一次
function IDPFleets.init(csObj)
    csSelf = csObj
    transform = csObj.transform
    ---@type Coolape.CLCellLua
    local fleetsInfor = getCC(transform, "PanelFleetInfor", "CLCellLua")
    fleetsInfor:setLua()
    ---@type IDPFleetInfor
    uiobjs.fleetsInfor = fleetsInfor.luaTable
    uiobjs.fleetsInfor.init(fleetsInfor)
    uiobjs.fleetsInfor.reset()

    local content = getChild(transform, "Anchor/content")
    ---@type UITable
    uiobjs.table = getCC(content, "Table", "UITable")
    uiobjs.cellPrefab = getChild(uiobjs.table.transform, "00000").gameObject
    ---@type UILabel
    uiobjs.LabelNums = getCC(content, "LabelNums", "UILabel")
end

-- 设置数据
function IDPFleets.setData(paras)
    mData = paras
end

--当有通用背板显示时的回调
function IDPFleets.onShowFrame()
end

-- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
function IDPFleets.show()
    CLUIUtl.resetList4Lua(uiobjs.table, uiobjs.cellPrefab, {}, IDPFleets.initCellFleet)
    ---@type DBCFHeadquartersLevsData
    local headquartersLev = DBCfg.getHeadquartersLevsDataByLev(bio2number(IDDBCity.curCity.headquarters.lev))
    uiobjs.LabelNums.text = joinStr(LGet("MyFleets"), ":", 0, "/", bio2number(headquartersLev.FleetsCount))
end

function IDPFleets.initCellFleet(cell, data)
end

-- 刷新
function IDPFleets.refresh()
end

-- 关闭页面
function IDPFleets.hide()
    uiobjs.fleetsInfor.hide()
end

-- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
function IDPFleets.procNetwork(cmd, succ, msg, paras)
    --[[
    if(succ == NetSuccess) then
      if(cmd == "xxx") then
        -- TODO:
      end
    end
    --]]
end

-- 处理ui上的事件，例如点击等
function IDPFleets.uiEventDelegate(go)
    local goName = go.name
    if goName == "collider4Hide" then
        hideTopPanel(csSelf)
    elseif goName == "ButtonAdd" then
        uiobjs.fleetsInfor.show(nil, nil)
    end
end

-- 当顶层页面发生变化时回调
function IDPFleets.onTopPanelChange(topPanel)
end

-- 当按了返回键时，关闭自己（返值为true时关闭）
function IDPFleets.hideSelfOnKeyBack()
    return true
end

--------------------------------------------
return IDPFleets

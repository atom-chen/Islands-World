-- 克拉肯海怪 攻击效果
local _cell = {}
---@type CLCellLua
local csSelf = nil
local transform = nil
local mData = nil
local objs = {}
local finishCallback

-- 初始化，只调用一次
function _cell.init(csObj)
    csSelf = csObj
    transform = csSelf.transform
    --[[
    上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
	--]]
    ---@type Coolape.CLRoleAction
    objs.action1 = getCC(transform, "haiguaizhuazi01", "CLRoleAction")
    objs.action2 = getCC(transform, "haiguaizhuazi02", "CLRoleAction")
    objs.action3 = getCC(transform, "haiguaizhuazi03", "CLRoleAction")
end

-- 显示，
-- 注意，c#侧不会在调用show时，调用refresh
function _cell.show(go, data)
    mData = data
end

function _cell.play(callback)
    finishCallback = callback
    local onCompleteMotion = ActCBtoList(100, _cell.onActionFinish)
    objs.action1:setAction(getAction("attack"), nil)
    objs.action2:setAction(getAction("attack"), nil)
    objs.action3:setAction(getAction("attack"), onCompleteMotion)
end

function _cell.onActionFinish(act)
    local actVal = act.currActionValue
    if actVal == getAction("attack") then
        local onCompleteMotion = ActCBtoList(100, _cell.onActionFinish)
        objs.action1:setAction(getAction("dead"), nil)
        objs.action2:setAction(getAction("dead"), nil)
        objs.action3:setAction(getAction("dead"), onCompleteMotion)
    elseif actVal == getAction("dead") then
        objs.action1:setAction(getAction("idel"), nil)
        objs.action2:setAction(getAction("idel"), nil)
        objs.action3:setAction(getAction("idel"), nil)
        if finishCallback then
            Utl.doCallback(finishCallback)
        end
    end
end

-- 取得数据
function _cell.getData()
    return mData
end

--------------------------------------------
return _cell

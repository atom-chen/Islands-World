-- 选择舰船数量
---@class _ParamCellSetShipNum
---@field public id number 舰船id
---@field public hadNum number 拥有舰船数量
---@field public setNum number 选择的舰船数量

local _cell = {}
---@type Coolape.CLCellLua
local csSelf = nil
local transform = nil
---@type _ParamCellSetShipNum
local mData = nil
local uiobjs = {}
local setNum = 0
-- 初始化，只调用一次
function _cell.init(csObj)
    csSelf = csObj
    transform = csSelf.transform
    --[[
    上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
	--]]
    uiobjs.SpriteIcon = getCC(transform, "SpriteIcon", "UISprite")
    uiobjs.LabelMaxNum = getCC(transform, "LabelMaxNum", "UILabel")
    ---@type Coolape.CLCellLua
    uiobjs.sliderNumer = nil
end

-- 显示，
-- 注意，c#侧不会在调用show时，调用refresh
function _cell.show(go, data)
    mData = data
    ---@type DBCFRoleData
    local attr = DBCfg.getDataById(DBCfg.CfgPath.Role, mData.id)
    uiobjs.SpriteIcon.spriteName = IDUtl.getRoleIcon(mData.id)
    uiobjs.LabelMaxNum.text = joinStr(mData.hadNum, "")
    _cell.setSliderNum()
end

function _cell.setSliderNum()
    if uiobjs.sliderNumer == nil then
        CLUIOtherObjPool.borrowObjAsyn("SliderNumber", _cell.onLoadSliderNum)
    else
        _cell.onLoadSliderNum("SliderNumber", uiobjs.sliderNumer.gameObject)
    end
end

---@param go UnityEngine.GameObject
function _cell.onLoadSliderNum(name, go, orgs)
    if uiobjs.sliderNumer or not csSelf.gameObject.activeInHierarchy then
        CLUIOtherObjPool.returnObj(go)
        SetActive(go, false)
        return
    end

    SetActive(go, true)
    uiobjs.sliderNumer = go:GetComponent("CLCellLua")
    uiobjs.sliderNumer.transform.parent = transform
    uiobjs.sliderNumer.transform.localScale = Vector3.one
    ---@type _Param4SliderNumber
    local sliderNumerData = {}
    sliderNumerData.default = mData.setNum
    sliderNumerData.min = 0
    sliderNumerData.max = mData.hadNum
    sliderNumerData.transScale = 0.7
    sliderNumerData.isSlenderMode = true
    sliderNumerData.onValChg = function(val)
        setNum = val
    end

    uiobjs.sliderNumer:init(sliderNumerData)
    uiobjs.sliderNumer.transform.localPosition = Vector3(344.7, 0, 0)
end

function _cell.OnDisable()
    if uiobjs.sliderNumer then
        CLUIOtherObjPool.returnObj(uiobjs.sliderNumer.gameObject)
        SetActive(uiobjs.sliderNumer.gameObject, false)
    end
end

-- 取得数据
function _cell.getData()
    return mData, setNum
end

--------------------------------------------
return _cell

-- xx单元
do
    local _cell = {}
    local csSelf = nil
    local transform = nil
    local uiobjs = {}
    local mData = nil
    local canClick = true

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj
        transform = csSelf.transform
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider")
        --]]
        uiobjs.SpriteIcon = getCC(transform, "SpriteIcon", "UISprite")
        uiobjs.LabelName = getCC(transform, "LabelName", "UILabel")
        uiobjs.LabelNum = getCC(transform, "LabelNum", "UILabel")
        uiobjs.LabelFood = getCC(transform, "LabelFood", "UILabel")
        uiobjs.LabelGold = getCC(transform, "LabelGold", "UILabel")
        uiobjs.LabelOil = getCC(transform, "LabelOil", "UILabel")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show ( go, data )
        mData = data
        canClick = true
        uiobjs.SpriteIcon.spriteName = joinStr("buildingIcon_", bio2number(mData.ID))
        uiobjs.LabelName.text = LGet(mData.NameKey)
        local val = DBCfg.getGrowingVal(
                bio2number(mData.BuildCostFoodMin),
                bio2number(mData.BuildCostFoodMax),
                bio2number(mData.BuildCostFoodCurve),
                1 / bio2number(mData.MaxLev))
        uiobjs.LabelFood.text = tostring(val)

        val = DBCfg.getGrowingVal(
                bio2number(mData.BuildCostGoldMin),
                bio2number(mData.BuildCostGoldMax),
                bio2number(mData.BuildCostGoldCurve),
                1 / bio2number(mData.MaxLev))
        uiobjs.LabelGold.text = tostring(val)

        val = DBCfg.getGrowingVal(
                bio2number(mData.BuildCostOilMin),
                bio2number(mData.BuildCostOilMax),
                bio2number(mData.BuildCostOilCurve),
                1 / bio2number(mData.MaxLev))
        uiobjs.LabelOil.text = tostring(val)
        local hadNum = IDMainCity.getBuildingCountByID(bio2number(mData.ID))
        local maxNum = IDMainCity.getMaxNumOfCurrHeadLev4Building(bio2number(mData.ID))
        uiobjs.LabelNum.text = joinStr(hadNum, "/", maxNum)
        if hadNum >= maxNum then
            canClick = false
            CLUIUtl.setAllSpriteGray(csSelf.gameObject, true)
        else
            canClick = true
            CLUIUtl.setAllSpriteGray(csSelf.gameObject, false)
        end
    end

    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.refresh( paras )
        --[[
        if(paras == 1) then   -- 刷新血
          -- TODO:
        elseif(paras == 2) then -- 刷新状态
          -- TODO:
        end
        --]]
    end

    -- 取得数据
    function _cell.getData ( )
        return mData, canClick
    end

    --------------------------------------------
    return _cell
end

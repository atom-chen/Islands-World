-- xx单元
do
    local _cell = {}
    local csSelf = nil
    local transform = nil
    local uiobjs = {}
    local mData = nil

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj
        transform = csSelf.transform
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider")
        --]]
        uiobjs.tweenScale = csSelf:GetComponent("TweenScale");
        uiobjs.SpriteIcon = getCC(transform, "SpriteIcon", "UISprite")
        uiobjs.Spritebg = getCC(transform, "Spritebg", "UISprite")
        uiobjs.Label = getCC(transform, "Label", "UILabel")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show ( go, data )
        mData = data
        uiobjs.Spritebg = mData.bg or "public_edit_circle_bt_shipshop_n"
        uiobjs.Label.text = LGet(mData.nameKey)

        uiobjs.tweenScale:ResetToBeginning();
        uiobjs.tweenScale.delay = mData.delay;
        uiobjs.tweenScale:Play(true);

        CLUIUtl.setSpriteFit(uiobjs.SpriteIcon, mData.icon, 80);
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
        return mData
    end

    --------------------------------------------
    return _cell
end

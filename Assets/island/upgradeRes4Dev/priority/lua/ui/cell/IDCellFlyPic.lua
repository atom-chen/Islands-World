-- xx单元
do
    local _cell = {}
    local csSelf = nil
    local transform = nil
    local mData = nil
    local objTb = {}
    local soundName = ""

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj
        transform = csSelf.transform
        ---@type TweenPosition
        objTb.twtf = transform:GetComponent("TweenTransform")
        ---@type Coolape.MyTween
        objTb.tween = transform:GetComponent("MyTween")
        objTb.pic = getCC(transform, "Sprite", "UISprite")
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show ( go, data )
        mData = data
        --[[
        mData.from:开始transform（可以为空）
        mData.to:目标transform
        mData.icon:图标
        mData.sound:音效
        --]]
        soundName = mData.sound
        CLUIUtl.setSpriteFit(objTb.pic, mData.icon, 65)

        if mData.from then
            objTb.twtf.from = mData.from
            transform.position = mData.from.position
        else
            objTb.twtf.from = transform
        end

        local _toPos = transform.position + Vector3(NumEx.NextInt(-30, 30) / 100, NumEx.NextInt(-30, 30) / 100, 0)
        objTb.tween:flyout(_toPos, NumEx.NextInt(120, 220) / 100, 0, nil,
                function()
                    --objTb.twtf.from = transform
                    objTb.twtf.to = mData.to
                    objTb.twtf.duration = NumEx.NextInt(90, 200) / 100
                    objTb.twtf:Play(true)
                end, true)

        if not isNilOrEmpty(soundName) then
            SoundEx.playSound(soundName, 1, 3)
        end
    end

    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.refresh( paras )

    end

    function _cell.uiEventDelegate(go)
        if go.name == "FlyPic" then
            objTb.twtf:ResetToBeginning()
            CLUIOtherObjPool.returnObj("FlyPic", go.gameObject)
            SetActive(go.gameObject, false)
        end
    end
    -- 取得数据
    function _cell.getData ( )
        return mData
    end

    --------------------------------------------
    return _cell
end

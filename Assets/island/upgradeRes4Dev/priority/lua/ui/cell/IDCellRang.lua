-- 范围
do
    local _cell = {}
    local csSelf = nil;
    local transform = nil;
    local mData = nil;
    local objs = {}

    -- 初始化，只调用一次
    function _cell.init (csObj)
        csSelf = csObj;
        transform = csSelf.transform;
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        local offset = getChild(transform, "offset")
        objs.sprites = {}
        table.insert(objs.sprites, getCC(offset, "SpriteLT", "UISprite"))
        table.insert(objs.sprites, getCC(offset, "SpriteRT", "UISprite"))
        table.insert(objs.sprites, getCC(offset, "SpriteRB", "UISprite"))
        table.insert(objs.sprites, getCC(offset, "SpriteLB", "UISprite"))
    end

    -- 显示，
    -- 注意，c#侧不会在调用show时，调用refresh
    function _cell.show (go, data)
        mData = data;
    end

    function _cell.showRang(color, size)
        for i,v in ipairs(objs.sprites) do
            v.color = color
        end
        transform.localScale = Vector3.one * size
    end

    -- 取得数据
    function _cell.getData ()
        return mData;
    end

    --------------------------------------------
    return _cell;
end

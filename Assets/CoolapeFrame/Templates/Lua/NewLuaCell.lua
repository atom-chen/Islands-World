-- xx单元
  local _cell = {}
  ---@type Coolape.CLCellLua
  local csSelf = nil
  local transform = nil
  local mData = nil
  local uiobjs = {}

  -- 初始化，只调用一次
  function _cell.init (csObj)
    csSelf = csObj
    transform = csSelf.transform
    --[[
    上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
    --]]
  end

  -- 显示，
  -- 注意，c#侧不会在调用show时，调用refresh
  function _cell.show ( go, data )
    mData = data
    --[[
    TODO:
    --]]
  end

  -- 取得数据
  function _cell.getData ( )
    return mData
  end

  --------------------------------------------
  return _cell
ƒ
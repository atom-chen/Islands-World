-- xx界面
do
    local IDPBuildingInfor = {}

    local csSelf = nil;
    local transform = nil;
    local uiobjs = {}
    local mData
    local buildingCamera
    local buildingAttrRoot
    local attr
    ---@type IDDBBuilding
    local serverData

    -- 初始化，只会调用一次
    function IDPBuildingInfor.init(csObj)
        csSelf = csObj;
        transform = csObj.transform;
        --[[
        上的组件：getChild(transform, "offset", "Progress BarHong"):GetComponent("UISlider");
        --]]
        uiobjs.Table = getCC(transform, "Table", "UITable")
        uiobjs.attrRoot = getChild(uiobjs.Table.transform, "1")
        uiobjs.LabelNextOpen = getCC(uiobjs.Table.transform, "2/LabelNextOpen", "UILabel")
        uiobjs.LabelLev = getCC(transform, "LabelLev", "UILabel")
        uiobjs.LabelDesc = getCC(transform, "LabelDesc", "UILabel")
    end

    -- 设置数据
    function IDPBuildingInfor.setData(paras)
        mData = paras
    end

    -- 显示，在c#中。show为调用refresh，show和refresh的区别在于，当页面已经显示了的情况，当页面再次出现在最上层时，只会调用refresh
    function IDPBuildingInfor.show()
        attr = mData.target.attr
        serverData = mData.target.serverData

        if buildingCamera == nil then
            CLUIOtherObjPool.borrowObjAsyn("BuildingCamera",
                    function(name, go, orgs)
                        buildingCamera = go:GetComponent("CLCellLua")
                        buildingCamera.transform.parent = transform
                        buildingCamera.transform.localScale = Vector3.one
                        buildingCamera.transform.localPosition = Vector3(-537, 100, 0)
                        SetActive(go, true)
                        buildingCamera:init(mData, nil)
                    end)
        end

        uiobjs.LabelLev.text = tostring(bio2number(serverData.lev))
        uiobjs.LabelDesc.text = LGet(attr.DescKey)
        IDPBuildingInfor.showNextLevOpen()

        IDPBuildingInfor.showAttr(
                function()
                    uiobjs.Table.repositionNow = true
                    --uiobjs.Table:Reposition()
                end)
    end

    -- 当加载好通用框的回调
    function IDPBuildingInfor.onShowFrame(cs)
        local title = LGet(attr.NameKey)
        csSelf.frameObj:init({ title = title, panel = csSelf })
    end

    -- 属性处理
    function IDPBuildingInfor.showAttr(callback)
        if buildingAttrRoot == nil then
            CLUIOtherObjPool.borrowObjAsyn("AttrUIPoc",
                    function(name, go, orgs)
                        buildingAttrRoot = go:GetComponent("CLCellLua")
                        buildingAttrRoot.transform.parent = uiobjs.attrRoot
                        buildingAttrRoot.transform.localScale = Vector3.one
                        buildingAttrRoot.transform.localPosition = Vector3.zero
                        SetActive(go, true)
                        buildingAttrRoot:init({ attr = attr, serverData = serverData, type = IDConst.AttrType.building, maxRow = 4 }, nil)
                        if callback then
                            callback()
                        end
                    end)
        else
            buildingAttrRoot:init({ attr = attr, serverData = serverData, type = IDConst.AttrType.building, maxRow = 4 }, nil)
        end
    end

    function IDPBuildingInfor.showNextLevOpen()
        local list = IDUtl.nextOpen(attr, bio2number(serverData.lev), IDConst.UnitType.building)
        if #list > 0 then
            local str = ""
            for i, v in ipairs(list) do
                if v.addVal > 0 then
                    str = joinStr(str, v.name, "+", v.addVal, ",")
                else
                    str = joinStr(str, v.name, v.addVal, ",")
                end
            end
            uiobjs.LabelNextOpen.text = str
        else
            uiobjs.LabelNextOpen.text = LGet("None")
        end
    end

    -- 刷新
    function IDPBuildingInfor.refresh()
    end

    -- 关闭页面
    function IDPBuildingInfor.hide()
        if buildingCamera then
            CLUIOtherObjPool.returnObj(buildingCamera.gameObject)
            SetActive(buildingCamera.gameObject, false)
            buildingCamera = nil
        end
        if buildingAttrRoot then
            CLUIOtherObjPool.returnObj(buildingAttrRoot.gameObject)
            SetActive(buildingAttrRoot.gameObject, false)
            buildingAttrRoot = nil
        end
    end

    -- 网络请求的回调；cmd：指命，succ：成功失败，msg：消息；paras：服务器下行数据
    function IDPBuildingInfor.procNetwork (cmd, succ, msg, paras)
        --[[
        if(succ == NetSuccess) then
          if(cmd == "xxx") then
          end
        end
        --]]
    end

    -- 处理ui上的事件，例如点击等
    function IDPBuildingInfor.uiEventDelegate(go)
        local goName = go.name;
        --[[
        if(goName == "xxx") then
        end
        --]]
    end

    -- 当按了返回键时，关闭自己（返值为true时关闭）
    function IDPBuildingInfor.hideSelfOnKeyBack()
        return true;
    end

    --------------------------------------------
    return IDPBuildingInfor;
end

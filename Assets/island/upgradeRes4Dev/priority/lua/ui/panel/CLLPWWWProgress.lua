do
    local pName;
    local csSelf;
    local transform;
    local table;
    local cellPrefab;

    local wwwList = ArrayList();
    local wwwMap = {};

    local PanelWWWProgress = {};
    function PanelWWWProgress.init(go)
        pName = go.name;
        csSelf = go;
        transform = csSelf.transform;
        table = getChild(transform, "AnchorBottomLeft", "Table");
        cellPrefab = getChild(table, "00000").gameObject;
        table = table:GetComponent("UITable");

        CLVerManager.self:setWWWListner(PanelWWWProgress.onAddWWW, PanelWWWProgress.onRmWWW);
    end

    function PanelWWWProgress.setData(pars)
    end

    function PanelWWWProgress.show()
        csSelf.panel.depth = CLUIInit.self.PanelWWWProgressDepth;
    end

    function PanelWWWProgress.initCell(go, data)
        local cell = go:GetComponent("CLCellLua");
        cell:init(nil, data);
    end

    function PanelWWWProgress.hide()
    end

    function PanelWWWProgress.refresh()
        CLUIUtl.resetList4Lua(table, cellPrefab, wwwList, PanelWWWProgress.initCell, true, false, 0);
    end

    function PanelWWWProgress.procNetwork(cmd, succ, msg, pars)
    end

    function PanelWWWProgress.onAddWWW(www, path, url)
        local d = {}
        d.www = www;
        d.path = path;
        d.url = url;
        wwwList:Add(d);
        wwwMap[url] = d;

        PanelWWWProgress.refresh();
    end

    function PanelWWWProgress.onRmWWW(url)
        local d = wwwMap[url];
        if (d ~= nil) then
            wwwList:Remove(d);
        end
        wwwMap[url] = nil;

        PanelWWWProgress.refresh();
    end

    --------------------------------------------
    return PanelWWWProgress;
end

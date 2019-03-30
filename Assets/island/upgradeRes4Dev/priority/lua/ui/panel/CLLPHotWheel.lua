-- 风火轮
do
    local pName = nil;
    local csSelf = nil;
    local transform = nil;
    local gameObject = nil;
    local LabelMsg = nil;
    local MaxShowTime = 10; --秒
    local msg = nil;

    local times = 0;
    local PanelHotWheel = {}

    function PanelHotWheel.init(go)
        pName = go.name;
        csSelf = go;
        transform = csSelf.transform;
        gameObject = csSelf.gameObject;
        LabelMsg = getChild(transform, "LabelMsg"):GetComponent("UILabel");
    end

    function PanelHotWheel.setData(pars)
        msg = pars;
        if (msg == nil or msg == "") then
            msg = Localization.Get("Loading");
        end
    end

    function PanelHotWheel.show()
        csSelf.panel.depth = CLUIInit.self.PanelHotWheelDepth;
        times = times + 1;
        csSelf:invoke4Lua(PanelHotWheel.hideSelf, MaxShowTime);
    end

    function PanelHotWheel.refresh()
        LabelMsg.text = msg;
    end

    function PanelHotWheel.hideSelf(...)
        if (times <= 0) then
            times = 0;
            return;
        end
        times = times - 1;
        if (times <= 0) then
            times = 0;
             --CLAlert.add("hide PanelHotWheel==", Color.red, 2);
            csSelf:cancelInvoke4Lua();
            CLPanelManager.hidePanel(csSelf);
        else
            csSelf:invoke4Lua(PanelHotWheel.hideSelf, MaxShowTime);
        end
    end

    function PanelHotWheel.hide()
        csSelf:cancelInvoke4Lua();
    end

    return PanelHotWheel;
end

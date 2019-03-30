-- 页面档板
do
    local csSelf = nil;
    local transform = nil;
    local gameObject = nil;
    local lastClickTime = 0; -- 上一次点击时间

    local CLLPBackplate = {}

    function CLLPBackplate.init(go)
        csSelf = go;
        transform = csSelf.transform;
        gameObject = csSelf.gameObject;
    end

    function CLLPBackplate.setData(pars)
    end

    function CLLPBackplate.show()
    end

    function CLLPBackplate.hide()
    end

    function CLLPBackplate.refresh()
    end

    function CLLPBackplate.procNetwork(cmd, succ, msg, pars)
    end

    function CLLPBackplate.OnClickBackplate(button)
        --[[
        local currTime = DateEx.nowMS;
        if ((currTime - lastClickTime) / 1000 < 0.3) then -- 保证在短时间内只能点击一次
        return;
        end
        lastClickTime = currTime;
        CLPanelManager.hideTopPanel();
        --]]
    end

    return CLLPBackplate;
end

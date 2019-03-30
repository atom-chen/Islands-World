--[[
-- 更新热更器处理
-- 判断热更新器本身是不是需要更新，同时判断渠道配置是否要更新
--]]
do
    ---@type System.Collections.Hashtable
    local localVer = Hashtable();
    ---@type System.Collections.Hashtable
    local serverVer = Hashtable();
    local serverVerStr = "";
    -- 热更新器的版本
    local upgraderVer = "upgraderVer.json";
    local localVerPath = upgraderVer;
    --local upgraderName = PStr.b():a(CLPathCfg.self.basePath):a("/upgradeRes/priority/lua/toolkit/CLLVerManager.lua"):e();
    local upgraderName = "preUpgradeList";
    -- 控制渠道更新的
    local channelName = "channels.json";
    local finishCallback = nil; -- finishCallback(isHaveUpdated)

    local isUpdatedUpgrader = false; -- 是否更新的热更新器
    ----------------------------------
    CLLUpdateUpgrader = {};
    function CLLUpdateUpgrader.checkUpgrader(ifinishCallback)
        isUpdatedUpgrader = false;
        finishCallback = ifinishCallback;
        CLVerManager.self:StartCoroutine(FileEx.readNewAllTextAsyn(localVerPath, CLLUpdateUpgrader.onGetLocalUpgraderVer));
    end

    function CLLUpdateUpgrader.onGetLocalUpgraderVer(content)
        localVer = JSON.DecodeMap(content);
        if (localVer == nil) then
            localVer = Hashtable();
        end
        local url = PStr.b():a(CLVerManager.self.baseUrl):a("/"):a(upgraderVer):e();
        url = Utl.urlAddTimes(url);

        WWWEx.get(url, CLAssetType.text,
            CLLUpdateUpgrader.onGetServerUpgraderVer,
            CLLUpdateUpgrader.onGetServerUpgraderVer, nil, true);
    end

    function CLLUpdateUpgrader.onGetServerUpgraderVer(content, orgs)
        serverVerStr = content;
        serverVer = JSON.DecodeMap(content);
        serverVer = serverVer == nil and Hashtable() or serverVer;
        -- print("MapEx.getInt(localVer, upgraderVer)==" .. MapEx.getInt(localVer, "upgraderVer"))
        -- print("MapEx.getInt(serverVer, upgraderVer)==" .. MapEx.getInt(serverVer, "upgraderVer"))
        if (MapEx.getString(localVer, "upgraderVer") ~= MapEx.getString(serverVer, "upgraderVer")) then
            CLLUpdateUpgrader.updateUpgrader();
        else
            CLLUpdateUpgrader.checkChannelVer(false);
        end
    end

    function CLLUpdateUpgrader.updateUpgrader(...)
        local url = "";
        local verVal = MapEx.getString(serverVer, "upgraderVer");
        url = PStr.b():a(CLVerManager.self.baseUrl):a("/"):a(upgraderName):a("."):a(verVal):e();
        WWWEx.get(url, CLAssetType.text,
            CLLUpdateUpgrader.ongetNewestPreupgradList,
            CLLUpdateUpgrader.ongetNewestPreupgradList,
            upgraderName, true);
    end

    function CLLUpdateUpgrader.ongetNewestPreupgradList(content, orgs)
        if (content ~= nil) then
            local preupgradList = JSON.DecodeList(content)
            if preupgradList == nil or preupgradList.Count == 0 then
                CLLUpdateUpgrader.checkChannelVer(false);
            else
                CLLUpdateUpgrader.loadServerRes({ preupgradList, 0 })
            end
        else
            CLLUpdateUpgrader.checkChannelVer(false);
        end
    end

    function CLLUpdateUpgrader.loadServerRes(orgs)
        local list = orgs[1]
        local i = orgs[2]
        if i >= list.Count then
            -- 完成
            CLLUpdateUpgrader.checkChannelVer(true);
        else
            local cell = list[i];
            local name = cell[0]
            local ver = cell[1]
            local url = PStr.b():a(CLVerManager.self.baseUrl):a("/"):a(name):a("."):a(ver):e();
            WWWEx.get(url, CLAssetType.bytes,
            CLLUpdateUpgrader.ongetNewestUpgrader,
            CLLUpdateUpgrader.ongetNewestUpgrader, { list, i, name }, true);
        end
    end

    function CLLUpdateUpgrader.ongetNewestUpgrader(content, orgs)
        local list = orgs[1]
        local i = orgs[2]
        local fileName = orgs[3]
        if (content ~= nil) then
            local file = PStr.begin():a(CLPathCfg.persistentDataPath):a("/"):a(fileName):e();
            FileEx.CreateDirectory(Path.GetDirectoryName(file));
            File.WriteAllBytes(file, content);
        else
            printe(joinStr(fileName , "get content == nil"));
        end
        CLLUpdateUpgrader.loadServerRes({ list, i + 1 })
    end

    -- 取得最新的渠道更新控制信息
    function CLLUpdateUpgrader.checkChannelVer(hadUpdatedUpgrader)
        isUpdatedUpgrader = hadUpdatedUpgrader;

        if (MapEx.getInt(localVer, "channelVer") < MapEx.getInt(serverVer, "channelVer")) then
            CLLUpdateUpgrader.getChannelInfor();
        else
            CLLUpdateUpgrader.finished()
        end
    end

    function CLLUpdateUpgrader.getChannelInfor(...)
        local verVal = MapEx.getInt(serverVer, "channelVer");
        -- 注意是加了版本号的，会使用cdn
        local url = PStr.b():a(CLVerManager.self.baseUrl):a("/"):a(channelName):a("."):a(verVal):e();
        WWWEx.get(url, CLAssetType.text,
        CLLUpdateUpgrader.onGetChannelInfor,
        CLLUpdateUpgrader.onGetChannelInfor, channelName, true);
    end

    function CLLUpdateUpgrader.onGetChannelInfor(content, orgs)
        if (content ~= nil) then
            local file = PStr.b():a(CLPathCfg.persistentDataPath):a("/"):a(channelName):e();
            FileEx.CreateDirectory(Path.GetDirectoryName(file));
            File.WriteAllText(file, content);
        end
        CLLUpdateUpgrader.finished()
    end

    function CLLUpdateUpgrader.finished()
        if isUpdatedUpgrader then
            local file = PStr.begin():a(CLPathCfg.persistentDataPath):a("/"):a(localVerPath):e();
            File.WriteAllText(file, serverVerStr);
        end
        Utl.doCallback(finishCallback, isUpdatedUpgrader);
    end
end

--module("CLLUpdateUpgrader", package.seeall)

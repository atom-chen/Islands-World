-- 资源更新器
do
    -- 服务器
    local csSelf = CLVerManager.self;
    local baseUrl = CLVerManager.self.baseUrl; --"http://gamesres.ultralisk.cn/cdn/test";
    local platform = "";
    local newestVerPath = "newestVers";
    local resVer = "resVer";
    local versPath = "VerCtl";
    local fverVer = "VerCtl.ver"; --本地所有版本的版本信息
    ---@type System.Collections.Hashtable
    local localverVer = Hashtable();
    ---@type System.Collections.Hashtable
    local serververVer = Hashtable();
    --========================
    local verPriority = "priority.ver";
    ---@type System.Collections.Hashtable
    local localPriorityVer = Hashtable(); --本地优先更新资源
    ---@type System.Collections.Hashtable
    local serverPriorityVer = Hashtable(); --服务器优先更新资源

    local verOthers = "other.ver";
    ---@type System.Collections.Hashtable
    local otherResVerOld = Hashtable(); --所有资源的版本管理
    ---@type System.Collections.Hashtable
    local otherResVerNew = Hashtable(); --所有资源的版本管理

    local tmpUpgradePirorityPath = "tmpUpgrade4Pirority";
    local haveUpgrade = false;
    local is2GNetUpgrade = CLVerManager.self.is2GNetUpgrade;
    local is3GNetUpgrade = CLVerManager.self.is3GNetUpgrade;
    local is4GNetUpgrade = CLVerManager.self.is4GNetUpgrade;

    local onFinishInit = nil;
    local progressCallback = nil;
    local mVerverPath = "";
    local mVerPrioriPath = "";
    local mVerOtherPath = "";

    ---@type System.Collections.Hashtable
    local needUpgradeVerver = Hashtable();
    local progress = 0;

    local isNeedUpgradePriority = false;
    local needUpgradePrioritis = Queue();
    local isSucessUpgraded = false;
    local verVerMD5 = "";

    CLLVerManager = {};

    -- 更新初始化
    --[[
      iprogressCallback: 进度回调，回调有两个参数
      ifinishCallback: 完成回调
      isdoUpgrade: 是否做更新处理
      --]]
    function CLLVerManager.init(iprogressCallback, ifinishCallback, isdoUpgrade, _verVerMD5)
        haveUpgrade = false;
        verVerMD5 = _verVerMD5
        CLVerManager.self.haveUpgrade = false;
        isNeedUpgradePriority = false;
        localverVer:Clear();
        serververVer:Clear();
        localPriorityVer:Clear();
        serverPriorityVer:Clear();
        otherResVerOld:Clear();
        otherResVerNew:Clear();
        platform = CLPathCfg.self.platform;
        CLVerManager.self.platform = platform;

        mVerverPath = PStr.begin():a(CLPathCfg.self.basePath):a("/"):a(resVer):a("/"):a(platform):a("/"):a(fverVer):e();
        mVerPrioriPath = PStr.begin():a(CLPathCfg.self.basePath):a("/"):a(resVer):a("/"):a(platform):a("/"):a(versPath):a("/"):a(verPriority):e();
        mVerOtherPath = PStr.begin():a(CLPathCfg.self.basePath):a("/"):a(resVer):a("/"):a(platform):a("/"):a(versPath):a("/"):a(verOthers):e();
        CLVerManager.self.mVerverPath = mVerverPath;
        CLVerManager.self.mVerPrioriPath = mVerPrioriPath;
        CLVerManager.self.mVerOtherPath = mVerOtherPath;

        progressCallback = iprogressCallback;
        onFinishInit = ifinishCallback;

        if (not isdoUpgrade) then
            CLLVerManager.loadPriorityVer();
            -- 后面会调用onFinish的回调
            CLLVerManager.loadOtherResVer(true);
            return;
        end
        --[[*
            ///     None 无网络
            ///     WiFi
            ///     2G
            ///     3G
            ///     4G
            ///     Unknown
            */
            --]]
        local netState = Utl.getNetState();
        local netActived = true;
        if (netState == "None") then
            netActived = false;
        elseif (netState == "2G") then
            if (not is2GNetUpgrade) then
                netActived = false;
            end
        elseif (netState == "3G") then
            if (not is3GNetUpgrade) then
                netActived = false;
            end
        elseif (netState == "4G") then
            if (not is4GNetUpgrade) then
                netActived = false;
            end
        elseif (netState == "WiFi") then
            netActived = true;
        elseif (netState == "Unknown") then
            netActived = true;
        end

        local canDoUpgrade = false;
        if (platform == "Android") then
            if (not CLCfgBase.self.isEditMode and netActived) then
                canDoUpgrade = true;
            end
        else
            if (not CLCfgBase.self.isEditMode and Utl.netIsActived()) then
                canDoUpgrade = true;
            end
        end
        if (canDoUpgrade) then
            canDoUpgrade = CLLVerManager.checkChannel();
            if (canDoUpgrade) then
                CLLVerManager.netWorkActived();
            else
                CLLVerManager.loadPriorityVer();
                -- 后面会调用onFinish的回调
                CLLVerManager.loadOtherResVer(true);
            end
        else
            -- 说明是编辑器环境
            Utl.doCallback(onFinishInit, true);
        end
    end

    -- 验证渠道是否需要更新
    function CLLVerManager.checkChannel()
        local defaultReuslt = false;
        -- 先判断是否已经取得取渠道
        local fpath = "channels.json";

        -- 得渠道控制醘数据
        local content = FileEx.readNewAllText(fpath);
        local channels = nil;
        if (content ~= nil) then
            channels = JSON.DecodeMap(content);
        else
            return defaultReuslt;
        end

        -- 取得当前版本的渠道数据
        fpath = "chnCfg.json"; -- 该文在打包时会自动放在streamingAssetsPath目录下，详细参见打包工具
        content = FileEx.readNewAllText(fpath);
        local chnCfg = nil;
        if (content ~= nil) then
            chnCfg = JSON.DecodeMap(content);
        else
            return defaultReuslt;
        end
        -- 取得当前包的渠道在渠道配置文件中是否有配置可更新
        if (MapEx.getBool(channels, MapEx.getString(chnCfg, "SubChannel"))) then
            return true;
        else
            return false;
        end

        return defaultReuslt;
    end

    -- 验证网络是否可用
    function CLLVerManager.netWorkActived()
        local onCheckNetSateSuc = function(...)
            CLVerManager.self:StartCoroutine(FileEx.readNewAllBytesAsyn(mVerverPath,
            CLLVerManager.onGetlcalVerverMap));
        end

        local onCheckNetSateFail = function(...)
            printw("Cannot connect Server or Net !!!");
            CLLVerManager.loadPriorityVer();
            -- 后面会调用onFinish的回调
            CLLVerManager.loadOtherResVer(false);
        end

        local url = Utl.urlAddTimes(PStr.b():a(baseUrl):a("/netState.txt"):e());
        WWWEx.get(url, CLAssetType.text,
        onCheckNetSateSuc,
        onCheckNetSateFail, nil, true);
    end


    --[[
      /// <summary>
      /// Ons the get verver map.取得本地版本文件的版本信息
      /// </summary>
      /// <param name='buff'>
      /// Buff.
      /// </param>
      --]]
    function CLLVerManager.onGetlcalVerverMap(buff)
        if (buff ~= nil) then
            localverVer = CLVerManager.self:toMap(buff);
        else
            localverVer = Hashtable();
        end
        CLLVerManager.getServerVerverMap();
    end

    --[[
      /// <summary>
      /// Gets the server verver map.取得服务器版本文件的版本信息
      /// </summary>
      --]]
    function CLLVerManager.getServerVerverMap(...)
        local url = "";
        if CLCfgBase.self.hotUpgrade4EachServer then
            -- 说明是每个服务器单独处理更新控制
            url = PStr.begin():a(baseUrl):a("/"):a(mVerverPath):a("."):a(verVerMD5):e();
        else
            url = PStr.begin():a(baseUrl):a("/"):a(mVerverPath):e();
        end

        WWWEx.get(Utl.urlAddTimes(url),
        CLAssetType.bytes,
        CLLVerManager.onGetServerVerverBuff,
        CLLVerManager.onGetServerVerverBuff, nil, true);
    end

    function CLLVerManager.onGetServerVerverBuff(content, orgs)
        if (content ~= nil) then
            serververVer = CLVerManager.self:toMap(content);
        else
            serververVer = Hashtable();
        end
        --判断哪些版本控制信息需要更新
        CLLVerManager.checkVervers();
    end

    function CLLVerManager.checkVervers()
        progress = 0;
        needUpgradeVerver:Clear();
        isNeedUpgradePriority = false;
        local ver = nil;
        local keysList = MapEx.keys2List(serververVer);
        local count = keysList.Count;
        local key = "";
        for i = 0, count - 1 do
            key = keysList[i];
            ver = MapEx.getString(localverVer, key);
            if (ver == nil or ver ~= MapEx.getString(serververVer, key)) then
                MapEx.set(needUpgradeVerver, key, false);
            end
        end
        keysList:Clear();
        keysList = nil;

        if (needUpgradeVerver.Count > 0) then
            if (progressCallback ~= nil) then
                progressCallback(needUpgradeVerver.Count, 0);
            end

            keysList = MapEx.keys2List(needUpgradeVerver);
            count = keysList.Count;
            key = "";
            for i = 0, count - 1 do
                key = keysList[i];
                CLLVerManager.getVerinfor(key, MapEx.getString(serververVer, key));
            end
            keysList:Clear();
            keysList = nil;
        else
            CLLVerManager.loadPriorityVer();
            CLLVerManager.loadOtherResVer(true);
        end
    end


    -- 取得版本文件
    function CLLVerManager.getVerinfor(fPath, verVal)
        local url = PStr.b():a(baseUrl):a("/"):a(fPath):a("."):a(verVal):e(); -- 注意是加了版本号的，可以使用cdn
        WWWEx.get(url, CLAssetType.bytes,
        CLLVerManager.onGetVerinfor,
        CLLVerManager.onGetVerinfor, fPath, true);
    end

    function CLLVerManager.onGetVerinfor(content, orgs)
        if (content ~= nil) then
            local fPath = orgs;
            progress = progress + 1;
            MapEx.set(localverVer, fPath, MapEx.getString(serververVer, fPath));

            local fName = PStr.b():a(CLPathCfg.persistentDataPath):a("/"):a(newestVerPath):a("/"):a(fPath):e();
            if (Path.GetFileName(fName) == "priority.ver") then
                -- 优先更新需要把所有资源更新完后才记录
                isNeedUpgradePriority = true;
                serverPriorityVer = CLVerManager.self:toMap(content);

            else
                FileEx.CreateDirectory(Path.GetDirectoryName(fName));
                File.WriteAllBytes(fName, content);
            end

            MapEx.set(needUpgradeVerver, fPath, true);

            if (progressCallback ~= nil) then
                progressCallback(needUpgradeVerver.Count, progress);
            end

            -- if (isFinishAllGet ()) then
            if (needUpgradeVerver.Count == progress) then
                if (not isNeedUpgradePriority) then
                    -- 说明没有优先资源需要更新，可以不做其它处理了
                    --同步到本地
                    local ms = MemoryStream();
                    B2OutputStream.writeMap(ms, localverVer);
                    local vpath = PStr.b():a(CLPathCfg.persistentDataPath):a("/"):a(mVerverPath):e();
                    FileEx.CreateDirectory(Path.GetDirectoryName(vpath));
                    File.WriteAllBytes(vpath, ms:ToArray());

                    CLLVerManager.loadPriorityVer();
                    CLLVerManager.loadOtherResVer(true);
                else
                    CLLVerManager.checkPriority(); --处理优先资源更新
                end
            end
        else
            CLLVerManager.initFailed();
        end
    end


    function CLLVerManager.checkPriority()
        --取得本地优先更新资源版本信息
        CLVerManager.self:StartCoroutine(FileEx.readNewAllBytesAsyn(mVerPrioriPath,
        CLLVerManager.onGetNewPriorityMap));
    end

    function CLLVerManager.onGetNewPriorityMap(buff)
        if (buff ~= nil) then
            localPriorityVer = CLVerManager.self:toMap(buff);
        else
            localPriorityVer = Hashtable();
        end
        CLVerManager.self.localPriorityVer = localPriorityVer; -- 同步到c#

        progress = 0;
        needUpgradeVerver:Clear();
        needUpgradePrioritis:Clear();
        local ver = nil;
        local keysList = MapEx.keys2List(serverPriorityVer);
        local key = nil;
        local count = keysList.Count;
        for i = 0, count - 1 do
            key = keysList[i];
            ver = MapEx.getString(localPriorityVer, key);
            if (ver == nil or ver ~= MapEx.getString(serverPriorityVer, key)) then
                MapEx.set(needUpgradeVerver, key, false);
                needUpgradePrioritis:Enqueue(key);
            end
        end
        keysList:Clear();
        keysList = nil;

        if (needUpgradePrioritis.Count > 0) then
            haveUpgrade = true;
            CLVerManager.self.haveUpgrade = true;
            if (progressCallback ~= nil) then
                progressCallback(needUpgradeVerver.Count, 0);
            end
            CLLVerManager.getPriorityFiles(needUpgradePrioritis:Dequeue());
        else
            --同步总的版本管理文件到本地
            local ms = MemoryStream();
            B2OutputStream.writeMap(ms, localverVer);
            local vpath = PStr.b():a(CLPathCfg.persistentDataPath):a("/"):a(mVerverPath):e();
            FileEx.CreateDirectory(Path.GetDirectoryName(vpath));
            File.WriteAllBytes(vpath, ms:ToArray());

            CLLVerManager.loadOtherResVer(true);
        end
    end

    -- 取得优先更新的资源
    function CLLVerManager.getPriorityFiles(fPath)
        local Url = "";
        local verVal = MapEx.getString(serverPriorityVer, fPath);
        Url = PStr.begin():a(baseUrl):a("/"):a(fPath):a("."):a(verVal):e(); -- 把版本号拼在后面
        -- print("Url==" .. Url);

        WWWEx.get(Url, CLAssetType.bytes,
        CLLVerManager.onGetPriorityFiles,
        CLLVerManager.initFailed, fPath, true);

        if (progressCallback ~= nil) then
            progressCallback(needUpgradeVerver.Count, progress, WWWEx.getWwwByUrl(Url));
        end
    end

    function CLLVerManager.onGetPriorityFiles(content, orgs)
        if (content == nil) then
            CLLVerManager.initFailed();
            return;
        end

        local fPath = orgs;
        progress = progress + 1;
        -- 先把文件放在tmp目录，等全部下载好后再移到正式目录
        local fName = PStr.b():a(CLPathCfg.persistentDataPath):a("/"):a(tmpUpgradePirorityPath):a("/"):a(fPath):e();
        FileEx.CreateDirectory(Path.GetDirectoryName(fName));
        File.WriteAllBytes(fName, content);

        --同步到本地
        MapEx.set(localPriorityVer, fPath, MapEx.getString(serverPriorityVer, fPath));
        MapEx.set(needUpgradeVerver, fPath, true);
        CLVerManager.self.localPriorityVer = localPriorityVer;

        if (progressCallback ~= nil) then
            progressCallback(needUpgradeVerver.Count, progress);
        end

        if (needUpgradePrioritis.Count > 0) then
            CLLVerManager.getPriorityFiles(needUpgradePrioritis:Dequeue());
        else
            --已经把所有资源取得完成
            -- 先把文件放在tmp目录，等全部下载好后再移到正式目录
            local keysList = MapEx.keys2List(needUpgradeVerver);
            local count = keysList.Count;
            local key = nil;
            local fromFile = "";
            local toFile = "";
            for i = 0, count - 1 do
                key = keysList[i];
                fromFile = PStr.b():a(CLPathCfg.persistentDataPath):a("/"):a(tmpUpgradePirorityPath):a("/"):a(key):e();
                toFile = PStr.begin():a(CLPathCfg.persistentDataPath):a("/"):a(key):e();
                FileEx.CreateDirectory(Path.GetDirectoryName(toFile));
                File.Copy(fromFile, toFile, true);
            end
            Directory.Delete(PStr.b():a(CLPathCfg.persistentDataPath):a("/"):a(tmpUpgradePirorityPath):e(), true);
            keysList:Clear();
            keysList = nil;

            --同步优先资源更新的版本管理文件到本地
            local ms = MemoryStream();
            B2OutputStream.writeMap(ms, localPriorityVer);
            local vpath = PStr.b():a(CLPathCfg.persistentDataPath):a("/"):a(mVerPrioriPath):e();
            FileEx.CreateDirectory(Path.GetDirectoryName(vpath));
            File.WriteAllBytes(vpath, ms:ToArray());

            --同步总的版本管理文件到本地
            ms = MemoryStream();
            B2OutputStream.writeMap(ms, localverVer);
            vpath = PStr.b():a(CLPathCfg.persistentDataPath):a("/"):a(mVerverPath):e();
            FileEx.CreateDirectory(Path.GetDirectoryName(vpath));
            File.WriteAllBytes(vpath, ms:ToArray());

            CLLVerManager.loadOtherResVer(true);
        end
    end


    function CLLVerManager.loadPriorityVer()
        CLVerManager.self:StartCoroutine(FileEx.readNewAllBytesAsyn(mVerPrioriPath, CLLVerManager.onGetVerPriority));
    end

    function CLLVerManager.onGetVerPriority(buff)
        if (buff ~= nil) then
            localPriorityVer = CLVerManager.self:toMap(buff);
        else
            localPriorityVer = Hashtable();
        end
        CLVerManager.self.localPriorityVer = localPriorityVer;
    end

    function CLLVerManager.loadOtherResVer(sucessProcUpgrade)
        isSucessUpgraded = sucessProcUpgrade;
        CLVerManager.self:StartCoroutine(FileEx.readNewAllBytesAsyn(mVerOtherPath, CLLVerManager.onGetVerOther));
    end

    function CLLVerManager.onGetVerOther(buff)
        if (buff ~= nil) then
            otherResVerOld = CLVerManager.self:toMap(buff);
        else
            otherResVerOld = Hashtable();
        end
        CLVerManager.self.otherResVerOld = otherResVerOld;
        local path = PStr.b():a(newestVerPath):a("/"):a(mVerOtherPath):e();
        CLVerManager.self:StartCoroutine(FileEx.readNewAllBytesAsyn(path, CLLVerManager.onGetNewVerOthers));
    end

    function CLLVerManager.onGetNewVerOthers(buff)
        if (buff ~= nil) then
            otherResVerNew = CLVerManager.self:toMap(buff);
        else
            otherResVerNew = Hashtable();
        end
        CLVerManager.self.otherResVerNew = otherResVerNew;

        progressCallback = nil;
        Utl.doCallback(onFinishInit, isSucessUpgraded);
    end

    function CLLVerManager.initFailed(...)
        if (progressCallback ~= nil) then
            progressCallback(needUpgradeVerver.Count, progress, nil);
        end
        CLLVerManager.loadPriorityVer();
        CLLVerManager.loadOtherResVer(false);
        printw("initFailed");
    end

    function CLLVerManager.isHaveUpgrade(...)
        return haveUpgrade;
    end

    return CLLVerManager;
end

--module("CLLVerManager", package.seeall)

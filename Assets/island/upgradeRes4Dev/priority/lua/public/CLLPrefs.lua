-- 本地存储
do
    local UserName = "UserName";
    local UserPsd = "UserPsd";
    local AutoFight = "AutoFight";
    local TestMode = "isTestMode";
    local TestModeUrl = "TestModeUrl";
    local soundEffSwitch = "soundEffSwitch";
    local musicSwitch = "musicSwitch";
    local userInfor = "userInfor"
    local currServer = "currServer"
    local lastLoginBtn = "lastLoginBtn"

    Prefs = {};

    function Prefs.setUserName(v)
        PlayerPrefs.SetString(UserName, v);
    end

    function Prefs.getUserName()
        return PlayerPrefs.GetString(UserName, "");
    end

    function Prefs.setUserPsd(v)
        PlayerPrefs.SetString(UserPsd, v);
    end

    function Prefs.getUserPsd(...)
        return PlayerPrefs.GetString(UserPsd, "");
    end

    function Prefs.setAutoFight(v)
        PlayerPrefs.SetInt(AutoFight, v and 0 or 1);
    end

    function Prefs.getAutoFight(...)
        return (PlayerPrefs.GetInt(AutoFight, 0) == 0) and true or false;
    end

    function Prefs.setTestMode(v)
        PlayerPrefs.SetInt(TestMode, v and 0 or 1);
    end

    function Prefs.getTestMode(v)
        return (PlayerPrefs.GetInt(TestMode, 0) == 0) and true or false;
    end

    function Prefs.setTestModeUrl(v)
        PlayerPrefs.SetString(TestModeUrl, v);
    end

    function Prefs.getTestModeUrl()
        return PlayerPrefs.GetString(TestModeUrl, "");
    end

    function Prefs.setUserInfor(v)
        PlayerPrefs.SetString(userInfor, v);
    end

    function Prefs.getUserInfor()
        return PlayerPrefs.GetString(userInfor, "");
    end

    function Prefs.setCurrServer(v)
        PlayerPrefs.SetString(currServer, v);
    end

    function Prefs.getCurrServer()
        return PlayerPrefs.GetString(currServer, "");
    end

    function Prefs.getSoundEffSwitch()
        local f = PlayerPrefs.GetInt(soundEffSwitch, 0);
        return (f == 0 and true or false);
    end

    function Prefs.setSoundEffSwitch(v)
        local f = v and 0 or 1;
        PlayerPrefs.SetInt("soundEffSwitch", f);
    end

    function Prefs.getMusicSwitch()
        local f = PlayerPrefs.GetInt(musicSwitch, 0);
        return (f == 0 and true or false);
    end

    function Prefs.setMusicSwitch(v)
        local f = v and 0 or 1;
        PlayerPrefs.SetInt("musicSwitch", f);
    end

    function Prefs.setLastLoginBtn(v)
        return PlayerPrefs.SetString(lastLoginBtn, v);
    end

    function Prefs.getLastLoginBtn()
        return PlayerPrefs.GetString(lastLoginBtn, "");
    end
end

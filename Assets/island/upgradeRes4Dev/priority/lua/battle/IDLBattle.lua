--[[
-- //                    ooOoo
-- //                   8888888
-- //                  88" . "88
-- //                  (| -_- |)
-- //                  O\  =  /O
-- //               ____/`---'\____
-- //             .'  \\|     |//  `.
-- //            /  \\|||  :  |||//  \
-- //           /  _||||| -:- |||||-  \
-- //           |   | \\\  -  /// |   |
-- //           | \_|  ''\---/''  |_/ |
-- //            \ .-\__  `-`  ___/-. /
-- //         ___`. .'  /--.--\  `. . ___
-- //      ."" '<  `.___\_<|>_/___.'  >' "".
-- //     | | : ` - \`.`\ _ /`.`/- ` : | |
-- //     \ \ `-.    \_ __\ /__ _/   .-` / /
-- //======`-.____`-.___\_____/___.-`____.-'======
-- //                   `=---='
-- //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
-- //           佛祖保佑       永无BUG
-- //           游戏大卖       公司腾飞
-- //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
--]]
---@public 战斗逻辑
IDLBattle = {}

local csSelf = nil
local transform = nil
local city = nil -- 城池对象
IDLBattle.offData = nil -- 进攻方数据
IDLBattle.defData = nil -- 防守方数据

--------------------------------------------
function IDLBattle._init()
    local cs = GameObject("battleRoot"):AddComponent(typeof(CLBaseLua))
    csSelf = cs
    IDLBattle.csSelf = cs
    IDLBattle.gameObject = cs.gameObject
    IDLBattle.transform = cs.transform
    IDLBattle.transform.parent = MyMain.self.transform
    IDLBattle.transform.localPosition = Vector3.zero
    IDLBattle.transform.localScale = Vector3.one
end

---@public 初始化
---@param offData table 进攻方数据
---@param defData IDDBCity 防守方数据
---@param callback 回调
---@param progressCB 进度回调
function IDLBattle.init(defData, offData, callback, progressCB)
    IDLBattle.defData = defData
    IDLBattle.offData = offData
    -- 先暂停资源释放
    CLAssetsManager.self:pause()
    -- 加载城
    IDMainCity.init(
        defData,
        function()
            city = IDMainCity
            -- 预加载进攻方兵种
            IDLBattle.prepareSoliders(offData, callback, progressCB)
        end,
        progressCB
    )
end

---@public 预加载进攻方兵种
function IDLBattle.prepareSoliders(data, callback, progressCB)
end

function IDLBattle.clean()
    -- 恢复资源释放
    CLAssetsManager.self:regain()
    if city then
        city.clean()
        city = nil
    end
end

function IDLBattle.destory()
    IDLBattle.clean()
    GameObject.DestroyImmediate(IDLBattle.gameObject, true)
end

return IDLBattle

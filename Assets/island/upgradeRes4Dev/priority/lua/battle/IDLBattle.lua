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
-- //     | | : ` - \`.` \ _ / `.`/- ` : | |
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
---@type IDPreloadPrefab
local IDPreloadPrefab = require("public.IDPreloadPrefab")
---@class BattleData 战场数据
---@field targetCity IDDBCity 目标城
---@field offShips table key:舰船id; value:{id=舰船id，num=数量(注意bio)}

local csSelf = nil
local transform = nil
---@type IDMainCity
local city = nil -- 城池对象
---@type CLGrid
local grid
---@type BattleData
IDLBattle.mData = nil -- 战斗方数据

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
---@param data BattleData 进攻方数据
---@param callback 回调
---@param progressCB 进度回调
function IDLBattle.init(data, callback, progressCB)
    IDLBattle.mData = data
    -- 先暂停资源释放
    CLAssetsManager.self:pause()
    -- 加载城
    IDMainCity.init(
        IDLBattle.mData.targetCity,
        function()
            city = IDMainCity
            grid = city.grid
            -- 预加载进攻方兵种
            IDLBattle.prepareSoliders(IDLBattle.mData.offShips, callback, progressCB)
        end,
        progressCB
    )
end

---@public 预加载进攻方兵种
function IDLBattle.prepareSoliders(data, callback, progressCB)
    IDPreloadPrefab.preloadRoles(data, callback, progressCB)
end

---@public
function IDLBattle.onClickOcean()
    local clickPos = MyMainCamera.lastHit.point
    local grid = grid.grid
    local index = grid:GetCellIndex(clickPos)
    local cellPos = grid:GetCellCenter(index)
    printe(index)
end

---@public 通知战场，玩家点击了我
function IDLBattle.onClickSomeObj(obg, pos)
end

function IDLBattle.onPressRole(isPress, role, pos)
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

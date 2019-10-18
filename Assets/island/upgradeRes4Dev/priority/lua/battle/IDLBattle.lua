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
---@class IDLBattle  战斗逻辑
IDLBattle = {}
---@type IDPreloadPrefab
local IDPreloadPrefab = require("public.IDPreloadPrefab")
---@type IDLBattleSearcher
local IDLBattleSearcher = require("battle.IDLBattleSearcher")
---@class BattleData 战场数据
---@field type IDConst.BattleType
---@field targetCity IDDBCity 目标城
---@field offShips table key:舰船id; value:{id=舰船id，num=数量(注意bio)}

---@type WrapBattleUnitData
IDLBattle.currSelectedUnit = nil
---@type Coolape.CLBaseLua
local csSelf = nil
---@type UnityEngine.Transform
local transform = nil
---@type IDMainCity
local city = nil -- 城池对象
---@type CLGrid
local grid
---@type BattleData
IDLBattle.mData = nil -- 战斗方数据
IDLBattle.isFirstDeployShip = true
-- 一次部署的数量
local EachDeployNum = 3
-- 进攻舰船
IDLBattle.offShips = {}
local __isInited = false
IDLBattle.isDebug = false

--------------------------------------------
function IDLBattle._init()
    if __isInited then
        return
    end
    __isInited = true
    local go = GameObject("battleRoot")
    csSelf = go:AddComponent(typeof(CLBaseLua))
    ---@type Coolape.CLBaseLua
    IDLBattle.csSelf = csSelf
    IDLBattle.csSelf.luaTable = IDLBattle

    IDLBattle.gameObject = csSelf.gameObject
    IDLBattle.transform = csSelf.transform
    transform = IDLBattle.transform
    IDLBattle.transform.parent = MyMain.self.transform
    IDLBattle.transform.localPosition = Vector3.zero
    IDLBattle.transform.localScale = Vector3.one
end

---@public 初始化
---@param data BattleData 进攻方数据
---@param callback 回调
---@param progressCB 进度回调
function IDLBattle.init(data, callback, progressCB)
    IDLBattle._init()
    IDLBattle.mData = data
    -- 先暂停资源释放
    CLAssetsManager.self:pause()
    IDWorldMap.addFinishEnterCityCallback(IDLBattle.onEnterCity)
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

function IDLBattle.onEnterCity()
    -- 初始化寻敌器
    IDLBattleSearcher.init(city)
end

---@public 预加载进攻方兵种
function IDLBattle.prepareSoliders(data, callback, progressCB)
    IDPreloadPrefab.preloadRoles(data, callback, progressCB)
end

---@public 设置当前选择的战斗单元
function IDLBattle.setSelectedUnit(data)
    IDLBattle.currSelectedUnit = data
end

---@public 点击了海面
function IDLBattle.onClickOcean()
    local clickPos = MyMainCamera.lastHit.point
    IDLBattle.deployBattleUnit()
end
---@public 通知战场，玩家点击了我
function IDLBattle.onClickSomeObj(obj, pos)
    if IDLBattle.isDebug and obj.isBuilding then
        IDLBattleSearcher.debugBuildingAttackRange(obj)
    end
    IDLBattle.deployBattleUnit()
end

---@public 开始战斗
function IDLBattle.begain()
    local buildings = city.getBuildings()
    ---@param v IDLBuilding
    for k, v in pairs(buildings) do
        if v.begainAttack then
            v:begainAttack()
        end
    end
end

function IDLBattle.deployBattleUnit()
    if IDLBattle.currSelectedUnit == nil then
        CLAlert.add(LGet("MsgSelectBattleUnit"), Color.yellow, 1)
        return
    end
    if bio2Int(IDLBattle.currSelectedUnit.num) <= 0 then
        CLAlert.add(LGet("MsgSelectBattleUnit"), Color.yellow, 1)
        return
    end
    local pos = MyMainCamera.lastHit.point
    pos.y = 0
    local grid = grid.grid
    local index = grid:GetCellIndex(pos)
    local cellPos = grid:GetCellCenter(index)
    if (not city.astar4Ocean:isObstructNode(pos)) or IDLBattle.currSelectedUnit.type == IDConst.UnitType.skill then
        if IDLBattle.isFirstDeployShip then
            -- 首次投放战斗单元，的处理

            IDLBattle.isFirstDeployShip = false
            if IDLBattle.mData.type == IDConst.BattleType.pvp then
                SoundEx.playMainMusic("BattleSound1")
            elseif IDLBattle.mData.type == IDConst.BattleType.pvp then
                SoundEx.playMainMusic("npc")
            end
            -- 战斗正式开始
            IDLBattle.csSelf:invoke4Lua(IDLBattle.begain, 1)
        end

        if
            IDLBattle.currSelectedUnit.type == IDConst.UnitType.ship or
                IDLBattle.currSelectedUnit.type == IDConst.UnitType.pet
         then
            IDLBattle.deployShip(IDLBattle.currSelectedUnit, pos)
        elseif IDLBattle.currSelectedUnit.type == IDConst.UnitType.skill then
        --//TODO: 技能释放
        end
    else
        --//TODO: can not place ship
    end
end

---@public 部署舰船
---@param shipData WrapBattleUnitData
---@param pos UnityEngine.Vector3
function IDLBattle.deployShip(shipData, pos)
    CLEffect.play("EffectDeploy", pos)
    SoundEx.playSound("water_craft_place_01", 1, 2)

    local num = bio2Int(shipData.num)
    local id = shipData.id
    local deployNum = 0
    if num >= EachDeployNum then
        deployNum = EachDeployNum
    else
        deployNum = num
    end
    shipData.num = int2Bio(num - deployNum)
    -- 通知ui
    if IDPBattle and IDPBattle.csSelf and IDPBattle.csSelf.gameObject.activeInHierarchy then
        IDPBattle.onDeployBattleUnit(shipData)
    end
    -- 加载舰船
    for i = 1, deployNum do
        CLRolePool.borrowObjAsyn(IDUtl.getRolePrefabName(id), IDLBattle.onLoadShip, {serverData = shipData, pos = pos})
    end
end

---@param ship Coolape.CLUnit
function IDLBattle.onLoadShip(name, ship, orgs)
    local serverData = orgs.serverData
    local pos = orgs.pos
    ship.transform.parent = transform
    ship.transform.localScale = Vector3.one
    -- ship.transform.localEulerAngles = Vector3.zero
    local headquarters = city.Headquarters
    local dir = headquarters.transform.position - pos
    Utl.RotateTowards(ship.transform, dir)
    if ship.luaTable == nil then
        ---@type IDRoleBase
        ship.luaTable = IDUtl.newRoleLua(serverData.id)
        ship:initGetLuaFunc()
    end
    SetActive(ship.gameObject, true)
    ship:init(serverData.id, 0, 1, true, {serverData = serverData})

    local offsetx = ship:fakeRandom(-10, 10) / 10
    local offsetz = ship:fakeRandom2(-10, 10) / 10
    pos = Vector3(offsetx + pos.x, pos.y, offsetz + pos.z)
    ship.transform.position = pos
    IDLBattle.offShips[ship.instanceID] = ship.luaTable
    IDLBattle.someOneJoin(ship.luaTable)
end

---public 有单位加入战场
---@param unit IDLUnitBase
function IDLBattle.someOneJoin(unit)
    IDLBattleSearcher.refreshUnit(unit)
end

---public 有单位死掉了
---@param unit IDLUnitBase
function IDLBattle.someOneDead(unit)
    --//TODO: 有单位死掉了
end

function IDLBattle.onPressRole(isPress, role, pos)
end

function IDLBattle.searchTarget(unit)
    return IDLBattleSearcher.searchTarget(unit)
end

function IDLBattle.clean()
    IDLBattle.isFirstDeployShip = true
    IDLBattle.currSelectedUnit = nil
    IDWorldMap.rmFinishEnterCityCallback(IDLBattle.onEnterCity)
    -- 恢复资源释放
    CLAssetsManager.self:regain()

    ---@param v IDRoleBase
    for k, v in pairs(IDLBattle.offShips) do
        v.csSelf:clean() -- 只能过能csSelf调用clean,不然要死循环
        CLRolePool.returnObj(v.csSelf)
        SetActive(v.gameObject, false)
    end
    IDLBattle.offShips = {}

    -- 城市清理
    if city then
        city.clean()
        city = nil
    end
    if IDLBattleSearcher then
        IDLBattleSearcher.clean()
    end
end

function IDLBattle.destory()
    IDLBattle.clean()
    GameObject.DestroyImmediate(IDLBattle.gameObject, true)
end

return IDLBattle

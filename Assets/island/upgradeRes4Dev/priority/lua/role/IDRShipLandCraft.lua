require("role.IDRShip")
---@class IDRShipLandCraft:IDRShip 登陆艇
IDRShipLandCraft = class("IDRShipLandCraft", IDRShip)

function IDRShipLandCraft:doAttack()
    if MyCfg.mode ~= GameMode.battle then
        return
    end
    -- 取得最近的海岸
    local pos = self:getNearestBeachPos()
    if pos then
        self.state = RoleState.landing
        self.seeker:seekAsyn(pos)
    else
        printe("取得最近海岸为 nil")
    end
end


function IDRShipLandCraft:onSearchPath(pathList, canReach)
    if MyCfg.mode == GameMode.battle then
        self.seeker:startMove()
    else
        self:getBase(IDRShipLandCraft).onSearchPath(self, pathList, canReach)
    end
end

function IDRShipLandCraft:onFinishLandSoldiers()
    self:onDead()
end

function IDRShipLandCraft:getNearestBeachPos()
    local list
    for i = 2, 50 do
        list = IDMainCity.grid.grid:getCircleCells(self.transform.position, i)
        for i = 0, list.Count - 1 do
            if IDMainCity.isIndexOnTheBeach(list[i]) then
                return IDMainCity.grid.grid:GetCellCenter(list[i])
            end
        end
    end
end

return IDRShipLandCraft

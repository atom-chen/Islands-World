-- 建筑大小
IDLBuildingSize = {}
local material = nil
local transform = nil
---@type UnityEngine.GameObject
local gameObject = nil
local isFinished = false

function IDLBuildingSize.init(  )
    if (isFinished) then
        return
    end
    isFinished = true
    CLThingsPool.borrowObjAsyn("BuildingSize",
            function(name, go, orgs)
                gameObject = go
                if go then
                    transform = gameObject.transform
                    transform.parent = MyMain.self.transform
                    transform.localEulerAngles = Vector3.zero
                    material = getCC(transform, "size","MeshRenderer").sharedMaterial
                    SetActive(go, false)
                else
                    printe("get building size is null")
                end
            end)
end

function IDLBuildingSize.show( size, color, pos )
    IDLBuildingSize.init()
    if gameObject then
        transform.localScale = Vector3(size, size, size)
        transform.position = pos
        material.color = color
        SetActive(gameObject, true)
    end
end

function IDLBuildingSize.hide(  )
    IDLBuildingSize.init()
    if gameObject then
        SetActive(gameObject, false)
    end
end

function IDLBuildingSize.setLayer( layerName )
    IDLBuildingSize.init()
    if gameObject then
        NGUITools.SetLayer(gameObject, LayerMask.NameToLayer(layerName))
    end
end

------------------
return IDLBuildingSize

require("public.class")
---@class IDBasePanel:ClassBase
local IDBasePanel = class("IDBasePanel")

-- 初始化，只会调用一次
---@param csObj Coolape.CLPanelLua
function IDBasePanel:init(csObj)
	---@type Coolape.CLPanelLua
	self.csSelf = csObj
	---@type UnityEngine.Transform
    self.transform = csObj.transform
end

---@public 当有通用背板显示时的回调
---@param cs Coolape.CLPanelLua
function IDBasePanel:onShowFrame(cs)
	if cs.frameObj then
		---@type _FrameData
		local d = {}
		d.title = LGet(cs.titleKeyName)
		d.panel = cs
		cs.frameObj:init(d)
	end
end

-- 当按了返回键时，关闭自己（返值为true时关闭）
function IDBasePanel:hideSelfOnKeyBack()
    return true
end

--------------------------------------------
return IDBasePanel

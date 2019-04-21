--与   同为1，则为1
--或   有一个为1，则为1
--非   true为 false，其余为true
--异或 相同为0，不同为1

BitUtl = {}
local __andBit = function(left, right) --与
    return (left == 1 and right == 1) and 1 or 0
end

local __orBit = function(left, right) --或
    return (left == 1 or right == 1) and 1 or 0
end

local __xorBit = function(left, right) --异或
    return (left + right) == 1 and 1 or 0
end

local __base = function(left, right, op) --对每一位进行op运算，然后将值返回
    if left < right then
        left, right = right, left
    end
    local res = 0
    local shift = 1
    while left ~= 0 do
        local ra = left % 2 --取得每一位(最右边)
        local rb = right % 2
        res = shift * op(ra, rb) + res
        shift = shift * 2
        left = math.modf(left / 2) --右移
        right = math.modf(right / 2)
    end
    return res
end

---@public 与
function BitUtl.andOp(left, right)
    return __base(left, right, __andBit)
end

---@public 或
function BitUtl.orOp(left, right)
    return __base(left, right, __orBit)
end

---@public 异或
function BitUtl.xorOp(left, right)
    return __base(left, right, __xorBit)
end

---@public 非
function BitUtl.notOp(left)
    return left > 0 and -(left + 1) or -left - 1
end

---@public left左移num位
function BitUtl.lShiftOp(left, num)
    return left * (2 ^ num)
end

---@public right右移num位
function BitUtl.rShiftOp(left, num)
    return math.floor(left / (2 ^ num))
end
return BitUtl

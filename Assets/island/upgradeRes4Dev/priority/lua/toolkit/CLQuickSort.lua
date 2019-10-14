---@class CLQuickSort 快速排序
local sort = {}
-- 平均运行时间: O(N * logN)
-- 最坏运行时间：O(N^2)
-- 快速排序函数入口
--@param list 数据
--@param compareFunc 比较函数
function sort.quickSort(list, compareFunc)
    -- 第一种算法
    sort.quickSort1(list, 1, #list, compareFunc)
    -- 第二种算法
    -- sort.quickSort2(list, 1, #list, compareFunc)
end

--递归调用，快速排序
--@param list 数据
--@param left 左下标
--@param right 右下标
--@param compareFunc 比较函数
function sort.quickSort1(list, left, right, compareFunc)
    if left < right then
        local k = sort.split1(list, left, right, compareFunc)
        sort.quickSort1(list, left, k - 1, compareFunc)
        sort.quickSort1(list, k + 1, right, compareFunc)
    end
end

--按枢纽元划分序列（取第一个值为枢纽元）
--@param list 数据
--@param left 左下标
--@param right 右下标
--@param compareFunc 比较函数
--@return 枢纽元位置
function sort.split1(list, left, right, compareFunc)
    --选用第一个值为枢纽元
    local pivot = list[left]
    local i = left
    local k = left
    for k = left + 1, right do
        if compareFunc(list[k], pivot) then
            i = i + 1
            if i ~= k then
                list[i], list[k] = list[k], list[i]
            end
        end
    end
    list[left], list[i] = list[i], list[left]
    return i
end

--==========================================
--递归调用，快速排序
--@param list 数据
--@param left 左下标
--@param right 右下标
--@param compareFunc 比较函数
function sort.quickSort2(list, left, right, compareFunc)
    if left < right then
        local k = sort.split2(list, left, right, compareFunc)
        -- dump(list)
        -- print(k)
        sort.quickSort2(list, left, k - 1, compareFunc)
        sort.quickSort2(list, k + 1, right, compareFunc)
    end
end

--按枢纽元划分序列（选用三数中指分割为枢纽元）
--@param list 数据
--@param left 左下标
--@param right 右下标
--@param compareFunc 比较函数
--@return 枢纽元位置
function sort.split2(list, left, right, compareFunc)
    --选用三数中指分割为枢纽元
    local pivot, k = sort.midian3(list, left, right, compareFunc)
    local i = left
    local j = k
    -- dump(list)
    -- print(pivot, i, j)
    while true do
        i = i + 1
        while compareFunc(list[i], pivot) and i < k do
            i = i + 1
        end
        j = j - 1
        while compareFunc(pivot, list[j]) and j > left do
            j = j - 1
        end
        if i < j then
            list[i], list[j] = list[j], list[i]
        else
            break
        end
    end
    list[i], list[k] = list[k], list[i]
    return i
end

--三数中指分割
--@desc 避免选出劣质枢纽元，当数组是预排序的或是反排序时，
--@desc 枢纽元选择影响快速排序的时间，劣质的枢纽元的选择，快速排序时间可能是二次的
--@param list 数据
--@param left 左下标
--@param right 右下标
--@param compareFunc 比较函数
function sort.midian3(list, left, right, compareFunc)
    if left == right then
        return list[right], right
    end
    local center = math.floor((left + right) / 2)
    if compareFunc(list[center], list[left]) then
        list[left], list[center] = list[center], list[left]
    end
    if compareFunc(list[right], list[left]) then
        list[left], list[right] = list[right], list[left]
    end
    if compareFunc(list[right], list[center]) then
        list[center], list[right] = list[right], list[center]
    end
    list[center], list[right - 1] = list[right - 1], list[center]
    return list[right - 1], right - 1
end

return sort

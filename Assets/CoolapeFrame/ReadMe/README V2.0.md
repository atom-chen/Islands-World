#CLFrame
CLFrame是基于NGUI、XLua的Unity开发游戏的框架。 我是在unity5.3.4的版本开发测试的，低版本不知道会不会有问题，没有测试。
##Getting start
![Getting start](http://coolape.net/md/CoolapeFrame/CoolapeFrame_Start1.jpeg)
![Getting start](http://coolape.net/md/CoolapeFrame/CoolapeFrame_Start2.png)
![Getting start](http://coolape.net/md/CoolapeFrame/CoolapeFrame_Start3.png)
![Getting start](http://coolape.net/md/CoolapeFrame/CoolapeFrame_Start4.png)
![Getting start](http://coolape.net/md/CoolapeFrame/CoolapeFrame_Start5.jpeg)
![Getting start](http://coolape.net/md/CoolapeFrame/CoolapeFrame_Start6.png)

##Features
###UI
###Resource
###Network
###Data Config
###Hot Fix
#####Client config
#####Server config
##File directory description
 CoolapeFrame     
┠ CoolapeFrame/Editor：*辅助工具*     
┃┠ CoolapeFrame/Editor/Inspectors：一些组件的inspector扩展*     
┃┠ CoolapeFrame/Editor/Tools：*主要的工具窗口类实现*     
┃┠ CoolapeFrame/Editor/Utl：*编辑器工具类*     
┃┖ CoolapeFrame/Editor/png：*窗口用到的图片*     
┠ CoolapeFrame/Examples：*例子，实现上是空的*     
┠ CoolapeFrame/NGUI_Enhance：*NGUI,这个版本的NGUI是被修改过的*     
┃┠ CoolapeFrame/NGUI_Enhance/DepthMask：*遮挡剔除，可以用到新手引导。但是使用比较麻烦，可以考虑用框架中新加的"UISlicedSprite"组件，很方便*     
┃┃┖ CoolapeFrame/NGUI_Enhance/DepthMask/Examples：**     
┃┠ CoolapeFrame/NGUI_Enhance/Editor     
┃┃┖ CoolapeFrame/NGUI_Enhance/Editor/Preview     
┃┠ CoolapeFrame/NGUI_Enhance/Examples     
┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Animations     
┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Atlases     
┃┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Atlases/Fantasy     
┃┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Atlases/Refractive     
┃┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Atlases/SciFi     
┃┃┃┖ CoolapeFrame/NGUI_Enhance/Examples/Atlases/Wooden     
┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Materials     
┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Models     
┃┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Models/Orc     
┃┃┃┖ CoolapeFrame/NGUI_Enhance/Examples/Models/Orc Armor     
┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Other     
┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Resources     
┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Scenes     
┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Scripts     
┃┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Scripts/InventorySystem     
┃┃┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Scripts/InventorySystem/Editor     
┃┃┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Scripts/InventorySystem/Game     
┃┃┃┃┖ CoolapeFrame/NGUI_Enhance/Examples/Scripts/InventorySystem/System     
┃┃┃┖ CoolapeFrame/NGUI_Enhance/Examples/Scripts/Other     
┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Shaders     
┃┃┠ CoolapeFrame/NGUI_Enhance/Examples/Sounds     
┃┃┖ CoolapeFrame/NGUI_Enhance/Examples/Textures     
┃┠ CoolapeFrame/NGUI_Enhance/HUD Text：*HUD,可以做血条等等*     
┃┃┠ CoolapeFrame/NGUI_Enhance/HUD Text/Editor     
┃┃┠ CoolapeFrame/NGUI_Enhance/HUD Text/Examples     
┃┃┃┠ CoolapeFrame/NGUI_Enhance/HUD Text/Examples/Scene     
┃┃┃┖ CoolapeFrame/NGUI_Enhance/HUD Text/Examples/Scripts     
┃┃┠ CoolapeFrame/NGUI_Enhance/HUD Text/Prefabs     
┃┃┖ CoolapeFrame/NGUI_Enhance/HUD Text/Scripts     
┃┠ CoolapeFrame/NGUI_Enhance/MyTexteara：*扩展的多行文本功能，实现文字出现的效果，详细看例子*     
┃┃┠ CoolapeFrame/NGUI_Enhance/MyTexteara/Scene：*例子*     
┃┃┖ CoolapeFrame/NGUI_Enhance/MyTexteara/Scripts     
┃┃　┖ CoolapeFrame/NGUI_Enhance/MyTexteara/Scripts/Editor     
┃┠ CoolapeFrame/NGUI_Enhance/Resources     
┃┃┖ CoolapeFrame/NGUI_Enhance/Resources/Shaders     
┃┠ CoolapeFrame/NGUI_Enhance/RichText4Chat：*支持图文混排的聊天，只能支持小图，例如表情图标，参见例子*     
┃┃┠ CoolapeFrame/NGUI_Enhance/RichText4Chat/Editor     
┃┃┠ CoolapeFrame/NGUI_Enhance/RichText4Chat/Scene     
┃┃┃┖ CoolapeFrame/NGUI_Enhance/RichText4Chat/Scene/faces     
┃┃┖ CoolapeFrame/NGUI_Enhance/RichText4Chat/Scripts     
┃┖ CoolapeFrame/NGUI_Enhance/Scripts     
┃　┠ CoolapeFrame/NGUI_Enhance/Scripts/Editor     
┃　┠ CoolapeFrame/NGUI_Enhance/Scripts/Interaction     
┃　┠ CoolapeFrame/NGUI_Enhance/Scripts/Internal     
┃　┠ CoolapeFrame/NGUI_Enhance/Scripts/Tweening     
┃　┖ CoolapeFrame/NGUI_Enhance/Scripts/UI     
┠ CoolapeFrame/Plugins：***框架中android的插件，具体可以参见java代码***     
┃┖ CoolapeFrame/Plugins/Android     
┠ CoolapeFrame/PluginsJava：*java代码*     
┃┖ CoolapeFrame/PluginsJava/src     
┃　┖ CoolapeFrame/PluginsJava/src/com     
┃　　┖ CoolapeFrame/PluginsJava/src/com/coolape     
┃　　　┖ CoolapeFrame/PluginsJava/src/com/coolape/u3dPlugin     
┠ CoolapeFrame/Scripts：*主要的核心代码*     
┃┠ CoolapeFrame/Scripts/Lua：*lua的封装*     
┃┠ CoolapeFrame/Scripts/assets：*资源管理核心代码*     
┃┠ CoolapeFrame/Scripts/main：*主入口*     
┃┠ CoolapeFrame/Scripts/net：*网络相关代码*     
┃┠ CoolapeFrame/Scripts/public：*公共*     
┃┠ CoolapeFrame/Scripts/resMgr：*资源更新管理*     
┃┠ CoolapeFrame/Scripts/role：*角色相关*     
┃┠ CoolapeFrame/Scripts/toolkit：*工具类*     
┃┖ CoolapeFrame/Scripts/ui：*UI相关代码*     
┃　┠ CoolapeFrame/Scripts/ui/NguiExtend：*对NGUI的扩展*     
┃　┠ CoolapeFrame/Scripts/ui/other：*empty*     
┃　┖ CoolapeFrame/Scripts/ui/public：*UI公共处理及工具类*     
┠ CoolapeFrame/Templates：*模板*     
┃┠ CoolapeFrame/Templates/DataCfg：*项目的配置数据模板*     
┃┠ CoolapeFrame/Templates/Localization：*NGUI的多语言配置模板*     
┃┠ CoolapeFrame/Templates/Lua：*重要Lua代码模板*     
┃┃┠ CoolapeFrame/Templates/Lua/cfg：*Lua取得配置数据的代码*     
┃┃┠ CoolapeFrame/Templates/Lua/net：*Lua与网络相关的代码*     
┃┃┠ CoolapeFrame/Templates/Lua/public：*Lua的公共代码*     
┃┃┠ CoolapeFrame/Templates/Lua/toolkit：*Lua工具代码*     
┃┃┖ CoolapeFrame/Templates/Lua/ui：*重要UI相关的代码*     
┃┃　┖ CoolapeFrame/Templates/Lua/ui/panel：*重要页面相关的lua代码*     
┃┠ CoolapeFrame/Templates/Textures：*一张空白png*     
┃┠ CoolapeFrame/Templates/cs：*C#代码模板*     
┃┠ CoolapeFrame/Templates/hotUpgradeCfg：*更新相关的配置模板*     
┃┖ CoolapeFrame/Templates/prefab：*预制件*     
┃　┖ CoolapeFrame/Templates/prefab/ui：*页面预制件*     
┖ CoolapeFrame/UnityEditorHelper：*unity编辑器工具插件，参见该插件的readme*     
　┠ CoolapeFrame/UnityEditorHelper/Attributes     
　┖ CoolapeFrame/UnityEditorHelper/Editor     
　　┖ CoolapeFrame/UnityEditorHelper/Editor/PropertyDrawer     

##Importemt API

##FAQ
CoolapFrame是一个很简单的框架，如果在使用过程中遇到什么问题建议及bug，非常欢迎告诉我们，再次谢谢！
QQ		:181752725   
Email	:181752725@qq.com

  
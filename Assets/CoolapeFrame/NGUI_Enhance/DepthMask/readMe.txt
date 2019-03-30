Unity3D 用Depth Mask实现的遮罩技术

Unity3D 用Depth Mask实现的遮罩技术
遮罩技术是一个基本的技术方法,有很多很多中用法.你可以在Flash中经常看到遮罩的使用,
它可以另一些视觉元素更加惊艳.当我看到Unity中没有包含任何几何体和图片的遮罩技术
很不爽,不过,幸运的我找到了解决方案,就是"Depth Mask"着色器.
 
先看看着色器的代码,没错,非常短.

Shader "Depth Mask" {

    SubShader{
        ColorMask 0
        Pass {}
    }
}

    如果用了多维材质,你需要像下面这样写:

Shader "Depth Mask Complex"
{
    SubShader
    {
        Tags {"Queue" = "Background"}
        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off
        ZWrite On
        ZTest Always
        Pass
        {
            Color(0,0,0,0)
        }
    }
}

如果你想更改任何一个物体材质的特性座遮罩(纹理,颜色等等)将是失败的,
除非你用SetPass()去设置着色器,关于这个方法的文档在这
(http://unity3d.com/support/documentation/ScriptReference/Material.SetPass.html).

示例项目下载:http://pixelplacement.com/wp-content/uploads/2011/02/Masking.zip

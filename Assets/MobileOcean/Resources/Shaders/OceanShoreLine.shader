Shader "OceanShader/OceanShoreLine" {
	Properties {  
		_WaterTex ("Normal Map (RGB), Foam (A)", 2D) = "white" {}
		_WaterTex2 ("Normal Map (RGB), Foam (B)", 2D) = "white" {}
		_ShoreLineTex ("ShoreLine Foam", 2D) = "white" {}
		_ShoreGray ("Shore Gray(gray)", 2D) = "white" {}
		_ShoreLineIntensity("ShoreLine Intensity", Float) = 2
//		_ShoreLineColors("ShoreLine Color",Color) = (0.4,0.4,0.4,1)
      
		_Tiling ("Wave Scale", Range(0.00025, 1)) = 0.25
		_WaveSpeed("Wave Speed", Float) = 0.4
		
		_SpecularRatio ("Specular Ratio", Range(10,500)) = 200
 
		_BottomColor("Bottom Color",Color) = (0,0,0,0)
		_TopColor("Top Color",Color) = (0,0,0,0)
		_Alpha("Alpha", Range(0, 1)) = 1
		 
//		_ReflectionTex ("Reflection", 2D) = "white" { TexGen ObjectLinear }
		_ReflectionIntensity("Reflection Intensity", Range(0, 1)) = 0.1
		 
		_OceanWidth("Ocean Width", Float) =10240 
		_OceanHeight("Ocean Height",Float) = 10240
	}   
	                  
	SubShader {    
		LOD 250	  
		Tags {
			"Queue"="Transparent-200"
			 "RenderType"="Transparent" 
			"IgnoreProjector" = "True"
		}    
		Lighting On
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass{ 
//			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			#define HasMirrorReflection
			#define HasShoreLine
			#pragma vertex Ocean_Vert
			#pragma fragment Frag
 
			
			#include "UnityCG.cginc"
			#include "Assets/MobileOcean/Resources/Shaders/Includes/OceanShoreLineInclude.cginc"

			float4 Frag(Ocean_VS_OUT IN):COLOR 
			{
				float4 col = Ocean_Frag(IN);  
				return col;
			}
			
		ENDCG	
		}  
		
	} 
	 
	 
	SubShader {  
		LOD 200	
		Tags {
			"Queue"="Geometry"
			 "RenderType"="Opaque" 
			"IgnoreProjector" = "True"
			"LightMode" = "ForwardBase" 
		}
		Lighting On
		Pass{
//			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
			#define HasMirrorReflection
			#define HasShoreLine
			#pragma vertex Ocean_Vert
			#pragma fragment Frag
 
			
			#include "UnityCG.cginc"
			#include "Assets/MobileOcean/Resources/Shaders/Includes/OceanShoreLineInclude.cginc"

			float4 Frag(Ocean_VS_OUT IN):COLOR 
			{
				float4 col = Ocean_Frag(IN);  
				return col;
			}
			
		ENDCG	
		}   
		
	} 
	
	
	SubShader {  
		LOD 150	
		Tags {
			"Queue"="Transparent-200"
			 "RenderType"="Transparent" 
			"IgnoreProjector" = "True"
			"LightMode" = "ForwardBase" 
		}
		Lighting On
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Pass{
//			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
//			#define HasMirrorReflection
			#define HasShoreLine
			#pragma vertex Ocean_Vert
			#pragma fragment Frag
 
			
			#include "UnityCG.cginc"
			#include "Assets/MobileOcean/Resources/Shaders/Includes/OceanShoreLineInclude.cginc"

			float4 Frag(Ocean_VS_OUT IN):COLOR 
			{
				float4 col = Ocean_Frag(IN);  
				return col;
			} 
			
		ENDCG	
		}  
		    
	}   
	                 
	SubShader {  
		LOD 100	
		Tags {
			"Queue"="Geometry"
			 "RenderType"="Opaque" 
			"IgnoreProjector" = "True"
			"LightMode" = "ForwardBase" 
		}  
		Lighting On 
		Pass{
//			Tags { "LightMode" = "ForwardBase" }
			CGPROGRAM
//			#define HasMirrorReflection
			#define HasShoreLine 
			#pragma vertex Ocean_Vert
			#pragma fragment Frag
 
			
			#include "UnityCG.cginc"
			#include "Assets/MobileOcean/Resources/Shaders/Includes/OceanShoreLineInclude.cginc"

			float4 Frag(Ocean_VS_OUT IN):COLOR 
			{
				float4 col = Ocean_Frag(IN);  
				return col;
			}
			
		ENDCG	
		}  
		
	}
	
	
	FallBack "Diffuse"
}

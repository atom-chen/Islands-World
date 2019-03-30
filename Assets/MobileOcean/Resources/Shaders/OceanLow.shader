Shader "OceanShader/OceanLow" {
	Properties {
		_WaterTex ("Normal Map (RGB), Foam (A)", 2D) = "white" {}
		_WaterTex2 ("Normal Map (RGB), Foam (B)", 2D) = "white" {}

		_Tiling ("Wave Scale", Range(0.00025, 1)) = 0.25
		_WaveSpeed("Wave Speed", Float) = 0.4
		_ReflectionIntensity("Reflection Intensity", Range(0, 1)) = 0.1
		
		_SpecularRatio ("Specular Ratio", Range(10,500)) = 200

		_BottomColor("Bottom Color",Color) = (0,0,0,0)
		_TopColor("Top Color",Color) = (0,0,0,0)
		
		
//		_ReflectionTex ("Reflection", 2D) = "white" { TexGen ObjectLinear }
	}
	
	SubShader {  
		Tags {
			"Queue"="Transparent-200"
			"RenderType"="Transparent" 
			"IgnoreProjector" = "True"
			"LightMode" = "ForwardBase"
		}
		LOD 250
		Pass{
			Lighting On
			ZWrite On
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#define HasMirrorReflection
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
		Tags {
			"Queue"="Geometry"
			"RenderType"="Opaque" 
			"IgnoreProjector" = "True"
			"LightMode" = "ForwardBase"
		}
		LOD 200	
		Pass{
			Lighting On
		
			CGPROGRAM
			#define HasMirrorReflection
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
		
		Tags {
			"Queue"="Transparent-200"
			"RenderType"="Transparent" 
			"IgnoreProjector" = "True"
			"LightMode" = "ForwardBase"
		}
		LOD 150	
		
		Pass{
			Lighting On
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
//			#define HasMirrorReflection
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
		Tags {
			"Queue"="Geometry"
			 "RenderType"="Opaque" 
			"IgnoreProjector" = "True"
			"LightMode" = "ForwardBase"
		}
		LOD 100	
		
		Pass{
			Lighting On
			
			CGPROGRAM
//			#define HasMirrorReflection
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

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#ifndef OCEAN_INCLUDE_SHORELINE
#define OCEAN_INCLUDE_SHORELINE
uniform float _Tiling;
uniform float _WaveSpeed;
uniform float _SpecularRatio;
uniform float _ReflectionIntensity;


uniform sampler2D _WaterTex;
uniform sampler2D _WaterTex2;
uniform sampler2D _ReflectionTex;
uniform sampler2D _ShoreGray;

uniform float4x4 _ProjMatrix;
uniform float4 _LightColor0;

uniform float4 _BottomColor;
uniform float4 _TopColor;

uniform float _Alpha;


//shore line
#ifdef HasShoreLine
uniform float _ShoreLineIntensity;
uniform sampler2D _ShoreLineTex;
float4 _ShoreLineTex_ST;
//uniform fixed4 _ShoreLineColors;
uniform float _OceanWidth;
uniform float _OceanHeight;
#endif

struct Ocean_VS_OUT
{

 float4 position  : POSITION;
 float3 worldPos  : TEXCOORD0;
 float3 tilingAndOffset:TEXCOORD2;
 #ifdef HasMirrorReflection
 float4 texCoordProj :TEXCOORD1; 
 #endif

};





Ocean_VS_OUT Ocean_Vert(appdata_full v)
{
 Ocean_VS_OUT o;
 o.worldPos = mul(unity_ObjectToWorld, v.vertex);
 o.position = UnityObjectToClipPos(v.vertex);// clip space coordinate position
 o.tilingAndOffset.z =frac( _Time.x * _WaveSpeed);
 #ifdef HasMirrorReflection
 o.texCoordProj = mul( mul(_ProjMatrix,unity_ObjectToWorld),  v.vertex);
 #endif
 o.tilingAndOffset.xy = o.worldPos.xz*_Tiling;

 return o;
}

float4 Ocean_Frag(Ocean_VS_OUT IN)
{
  float3 lightColor=_LightColor0.xyz*2;
  
  float3 worldView = -normalize(IN.worldPos - _WorldSpaceCameraPos);

  float2 tiling = IN.tilingAndOffset.xy;

  float4 nmap1 = tex2D(_WaterTex, tiling.yx +float2(IN.tilingAndOffset.z,0));
  
  float4 nmap2 = tex2D(_WaterTex2, tiling.yx -float2(IN.tilingAndOffset.z,0));
  
  float3 worldNormal  = normalize((nmap1.xyz+nmap2.xyz)*2-2);
  
  float dotLightWorldNomal = dot(worldNormal, float3(0,1,0));
  
  float3 light = _WorldSpaceLightPos0.xyz;
//    float3 reflLight =  reflect(-light, worldNormal);
  float3 specularReflection = float3(0,0,0) ;
  
  if (dotLightWorldNomal < 0.0) {
        // light source on the wrong side?
        specularReflection = float3(0.0, 0.0, 0.0); 
     }
     else{
      
     
 //              fixed dotSpecular = dot(reflLight,  float3( worldView.x,-worldView.y,worldView.z));
    float dotSpecular = dot(worldNormal,  normalize( worldView+light));
    specularReflection = pow(max(0.0, dotSpecular), _SpecularRatio);
     }
  
 
  float4 col;
  float fresnel = 0.5*dotLightWorldNomal+0.5;
   
  col.rgb  = lerp(_BottomColor.xyz, _TopColor.xyz, fresnel);
  
  col.a = 1;

  #ifdef HasMirrorReflection
  float4 newTecCoord = IN.texCoordProj;
  newTecCoord.xz+=(worldNormal.xz)*5;
  
  float4 reflectCol = float4(0,0,0,1);
//  if(newTecCoord.w>0){
  	 reflectCol = tex2Dproj(_ReflectionTex, newTecCoord);
//  }
  
   
  
  col.rgb = col.rgb*(1-_ReflectionIntensity)+reflectCol.rgb*_ReflectionIntensity;
  #endif
  
  col.rgb+=specularReflection;
  
  
  col.rgb*=lightColor;
  
  col.a = _Alpha;
  
  #ifdef HasShoreLine
  fixed grayAlpha = tex2D(_ShoreGray, float2(  IN.worldPos.x*_OceanWidth+0.5,IN.worldPos.z*_OceanHeight+0.5 ) ).a;
  if(grayAlpha>0){
//		float4 shoreLineCol = tex2D(_ShoreLineTex, IN.worldPos.xz*_ShoreLineTex_ST.xy*0.005+(worldNormal.xz)*0.08);
		float4 shoreLineCol = tex2D(_ShoreLineTex, IN.worldPos.xz*_ShoreLineTex_ST.xy*0.005+(worldNormal.xz)*0.08);
//		col.rgb += _ShoreLineIntensity*(grayAlpha)*shoreLineCol*_ShoreLineColors.rgb; 
		col.rgb += _ShoreLineIntensity*(grayAlpha)*shoreLineCol;
  }
  #endif

  return col;
}
 
#endif 











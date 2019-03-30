using UnityEngine;
using System.Collections;
public class FrameCount : MonoBehaviour {
	public Light directLight;
	public MirrorReflection mirrorReflection;
	public Material oceanMat;
	float frameCount = 0;
	
	int lastFrameCount = 0;
	int currenFrameCount = 0;
	float intevalTime = 1;
	
	Color lightColor;
	GUIStyle style;
	
	float reflectionIntensity = 0.3f;
	float shoreLineIntensity = 4.8f;
	float alpha = 0.7f;
	
	void Awake(){
		mirrorReflection = GameObject.FindObjectOfType(typeof(MirrorReflection)) as MirrorReflection;
	}
	// Use this for initialization
	void Start () {
		lightColor = directLight.color;
		style = new GUIStyle();
		style.fontSize = 25;
		style.normal.textColor = Color.white;
		
		InvokeRepeating("GetFrameCount",0,intevalTime);
	}
	
	// Update is called once per frame
	void Update () {
		directLight.color = lightColor;
		oceanMat.SetFloat("_ReflectionIntensity",reflectionIntensity);
		oceanMat.SetFloat("_ShoreLineIntensity",shoreLineIntensity);
	}
	
	void GetFrameCount(){
		
		currenFrameCount = Time.frameCount;
		frameCount = (currenFrameCount - lastFrameCount)/intevalTime;
		lastFrameCount = currenFrameCount;
	}
	
	
	void OnGUI(){
		
		GUILayout.Label("frame:"+frameCount,style);
		GUILayout.Label("press w,s,a,d to move.");
		GUILayout.Label("press UpArrow,LeftArrow,RightArrow,DownArrow to rotate.");
		GUILayout.Label("light red");
		lightColor.r = GUILayout.HorizontalScrollbar(lightColor.r,0.1f,0,1,GUILayout.Width(200));
		GUILayout.Label("light green");
		lightColor.g = GUILayout.HorizontalScrollbar(lightColor.g,0.1f,0,1,GUILayout.Width(200));
		GUILayout.Label("light blue");
		lightColor.b = GUILayout.HorizontalScrollbar(lightColor.b,0.1f,0,1,GUILayout.Width(200));
		
		GUILayout.Label("Shore line intensity:");
		shoreLineIntensity = GUILayout.HorizontalScrollbar(shoreLineIntensity,0.2f,0,5f,GUILayout.Width(200));

		GUILayout.Label("alpha:");
		alpha = GUILayout.HorizontalScrollbar(alpha,0.1f,0,1f,GUILayout.Width(200));

		
		mirrorReflection.enableMirrorReflection =  GUILayout.Toggle(mirrorReflection.enableMirrorReflection ,"Mirror reflection");
		
		if(mirrorReflection.enableMirrorReflection){
			GUILayout.Label("Reflection intensity:");
			reflectionIntensity = GUILayout.HorizontalScrollbar(reflectionIntensity,0.1f,0,1f,GUILayout.Width(200));
		}

		mirrorReflection.alpha = alpha;
		
		
	}
}

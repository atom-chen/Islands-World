using UnityEngine;
using System.Collections;
using UnityEditor;
using MobileOcean;

public class CreateOcean : ScriptableWizard {
	int meshWidthSegments = 1;
	public float meshWidth = 500;
	public int meshCountX = 1;
	public int meshCountZ = 1;
	public bool enableShoreLine = false;
	public bool enableMirrorReflect = true;
	public LayerMask shoreLineDetectLayer;
	public int shoreLineDepth = 50;

	public float alpha= 1;

	[MenuItem("GameObject/MobileOcean/Generate Ocean")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard("Generate Ocean",typeof(CreateOcean));
	}

	void OnWizardCreate()
	{

		GameObject plane = new GameObject();

		plane.name = "Water";

		plane.transform.position = Vector3.zero;
		
		Vector2 anchorOffset = Vector2.zero;;
		
		MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
		plane.AddComponent(typeof(MeshRenderer));
		
		string planeAssetName = plane.name +"S" + meshWidthSegments + "W" + meshWidth + ".asset";
		Mesh m = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/MobileOcean/Meshes/" + planeAssetName,typeof(Mesh));
		
		if (m == null)
		{
			m = new Mesh();
			m.name = plane.name;
			
			int hCount2 = meshWidthSegments+1;
			int vCount2 = meshWidthSegments+1;
			int numTriangles = meshWidthSegments * meshWidthSegments * 6;
			int numVertices = hCount2 * vCount2;
			
			Vector3[] vertices = new Vector3[numVertices];
			Vector2[] uvs = new Vector2[numVertices];
			int[] triangles = new int[numTriangles];
			
			int index = 0;
			float uvFactorX = 1.0f/meshWidthSegments;
			float uvFactorY = 1.0f/meshWidthSegments;
			float scaleX = meshWidth/meshWidthSegments;
			float scaleY = meshWidth/meshWidthSegments;
			for (float y = 0.0f; y < vCount2; y++)
			{
				for (float x = 0.0f; x < hCount2; x++)
				{
					vertices[index] = new Vector3(x*scaleX - meshWidth/2f - anchorOffset.x, 0.0f, y*scaleY - meshWidth/2f - anchorOffset.y);
					uvs[index++] = new Vector2(x*uvFactorX, y*uvFactorY);
				}
			}
			
			index = 0;
			for (int y = 0; y < meshWidthSegments; y++)
			{
				for (int x = 0; x < meshWidthSegments; x++)
				{
					triangles[index]   = (y     * hCount2) + x;
					triangles[index+1] = ((y+1) * hCount2) + x;
					triangles[index+2] = (y     * hCount2) + x + 1;
					
					triangles[index+3] = ((y+1) * hCount2) + x;
					triangles[index+4] = ((y+1) * hCount2) + x + 1;
					triangles[index+5] = (y     * hCount2) + x + 1;
					index += 6;
				}
			}
			
			m.vertices = vertices;
			m.uv = uvs;
			m.triangles = triangles;
			m.RecalculateNormals();
			 
			AssetDatabase.CreateAsset(m, "Assets/MobileOcean/Meshes/" + planeAssetName);
			AssetDatabase.SaveAssets();
		}
		
		meshFilter.sharedMesh = m;
		m.RecalculateBounds();

		Selection.activeObject = plane;

		GameObject ocean = new GameObject();

		ocean.transform.position = Vector3.zero;
		MirrorReflection mirrorReflection = ocean.AddComponent<MirrorReflection>();
		mirrorReflection.enableMirrorReflection = enableMirrorReflect;
		mirrorReflection.alpha = alpha;
		Material oceanMaterial;
//		mirrorReflection.enableShoreLine = enableShoreLine;
		if(enableShoreLine){
			int terrainWidth = (int)meshWidth*meshCountX;
			int terrainHeight = (int)meshWidth*meshCountZ;
			int texWidth = 1024;
			int texHeight = 1024;

			Material oceanMaterialPrefab = Resources.Load("OceanResources/OceanShoreLine") as Material; 
			oceanMaterial = new Material(oceanMaterialPrefab);
		
			string path = "Assets/MobileOcean/Materials/OceanMats/OceanShoreLine_"+System.DateTime.Now.ToString("yyyyMMddHHmmss")+".mat";
			AssetDatabase.CreateAsset(oceanMaterial, path);

			string texPath = GrayScaleCreator.CreateTexture(texWidth,texHeight,terrainWidth,terrainHeight,shoreLineDetectLayer,shoreLineDepth);
			Texture2D grayTex = (Texture2D) (AssetDatabase.LoadAssetAtPath(texPath, typeof(Texture2D)));
			

			
			oceanMaterial.SetTexture("_ShoreGray",grayTex);
			oceanMaterial.SetFloat("_OceanWidth",1f/terrainWidth);
			oceanMaterial.SetFloat("_OceanHeight",1f/terrainHeight);

			
			
			AssetDatabase.SaveAssets(); 
			AssetDatabase.Refresh();
			ocean.name = "OceanHigh";
		}else{
			oceanMaterial = Resources.Load("OceanResources/OceanLow") as Material;
//			mirrorReflection.enableShoreLine = false;
			ocean.name = "OceanLow";
		}
		ocean.layer = LayerMask.NameToLayer("Water");
		plane.GetComponent<Renderer>().material = oceanMaterial;
		mirrorReflection.oceanMaterial = oceanMaterial;

		if(enableMirrorReflect){
			mirrorReflection.enableMirrorReflection = true;
		}

		for(int i = 0; i<meshCountX;i++){
			for(int j = 0;j<meshCountZ;j++){
				GameObject newPlane = Instantiate(plane) as GameObject;
				newPlane.transform.parent = ocean.transform;
				newPlane.name = "Water"+i+j;
				float posX = -meshWidth*meshCountX*0.5f+0.5f*meshWidth+i*meshWidth;
				float posZ = -meshWidth*meshCountZ*0.5f+0.5f*meshWidth+j*meshWidth;
				newPlane.transform.localPosition = new Vector3(posX,0,posZ);
				newPlane.layer = LayerMask.NameToLayer("Water");
			}
		}


		DestroyImmediate(plane);
		SavePlayerPrefs();
	}

	bool isFirstUpdate = true;
	void OnWizardUpdate()
	{
		meshWidth = Mathf.Max(meshWidth,0);
		meshCountX = Mathf.Max(meshCountX,1);
		meshCountZ = Mathf.Max(meshCountZ,1);
		alpha = Mathf.Clamp01(alpha);

		if(isFirstUpdate)
		{
			if(PlayerPrefs.HasKey("MobileOcean_meshWidth"))
			{
				meshWidth = PlayerPrefs.GetFloat("MobileOcean_meshWidth");
			}
			if(PlayerPrefs.HasKey("MobileOcean_meshCountX"))
			{
				meshCountX = PlayerPrefs.GetInt("MobileOcean_meshCountX");
			}
			if(PlayerPrefs.HasKey("MobileOcean_meshCountZ"))
			{
				meshCountZ = PlayerPrefs.GetInt("MobileOcean_meshCountZ");
			}
			if(PlayerPrefs.HasKey("MobileOcean_enableShoreLine"))
			{
				enableShoreLine =bool.Parse( PlayerPrefs.GetString("MobileOcean_enableShoreLine"));
			}
			if(PlayerPrefs.HasKey("MobileOcean_enableMirrorReflect"))
			{
				enableMirrorReflect =bool.Parse( PlayerPrefs.GetString("MobileOcean_enableMirrorReflect"));
			}
			if(PlayerPrefs.HasKey("MobileOcean_shoreLineDetectLayer"))
			{
				shoreLineDetectLayer.value =PlayerPrefs.GetInt("MobileOcean_shoreLineDetectLayer");
			}

			if(PlayerPrefs.HasKey("MobileOcean_alpha"))
			{
				alpha =PlayerPrefs.GetFloat("MobileOcean_alpha");
			}

			if(PlayerPrefs.HasKey("MobileOcean_shoreLineDepth"))
			{
				shoreLineDepth = PlayerPrefs.GetInt("MobileOcean_shoreLineDepth");
			}
		
			isFirstUpdate = false;
		}

		SavePlayerPrefs();


	}

	void SavePlayerPrefs(){
		PlayerPrefs.SetFloat("MobileOcean_meshWidth",meshWidth);
		PlayerPrefs.SetInt("MobileOcean_meshCountX",meshCountX);
		PlayerPrefs.SetInt("MobileOcean_meshCountZ",meshCountZ);
		PlayerPrefs.SetString("MobileOcean_enableShoreLine",enableShoreLine.ToString());
		PlayerPrefs.SetString("MobileOcean_enableMirrorReflect",enableMirrorReflect.ToString());
		PlayerPrefs.SetInt("MobileOcean_shoreLineDetectLayer",shoreLineDetectLayer.value);
		PlayerPrefs.SetFloat("MobileOcean_alpha",alpha);
		PlayerPrefs.SetInt("MobileOcean_shoreLineDepth",shoreLineDepth);
	}


}

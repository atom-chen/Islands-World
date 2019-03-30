using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

namespace MobileOcean{

	public class GrayScaleCreator
	{

		static string path = "Assets/MobileOcean/Textures/OceanTextures/GrayTextures/";

		 
		public static string CreateTexture (int width, int height, int mapWidth, int mapHeight, LayerMask shoreLineDetectLayer, float detectDistance)
		{
			Vector3 beginPos = new Vector3 (-mapWidth / 2, 0f, -mapHeight / 2);
			float widthPixDis = (float)mapWidth / (float)width;
			float heighPixDis = (float)mapHeight / (float)height;

			Texture2D texture = new Texture2D (width, height, TextureFormat.ARGB32, false);
			for (int i = 0; i<width; i++) {
				for (int j = 0; j<height; j++) {
					Vector3 orgPos = beginPos + new Vector3 (widthPixDis * i, 0, heighPixDis * j);
					float shoreLinePower = RayCastToTerrain (orgPos, shoreLineDetectLayer,detectDistance);
//					Debug.Log(shoreLinePower);

					texture.SetPixel (i, j, new Color (shoreLinePower, shoreLinePower, shoreLinePower, shoreLinePower));
				}
			} 


	//				for (int i = 0; i<width; i++) {
	//					for (int j = 0; j<height; j++) {
	//						Color col = GaussianBlur(i,j,texture,true);
	//						texture.SetPixel(i,j,col);
	//					}
	//				}
	//
	//				for (int i = 0; i<width; i++) {
	//					for (int j = 0; j<height; j++) {
	//						Color col = GaussianBlur(i,j,texture,false);
	//						texture.SetPixel(i,j,col);
	//					}
	//				}


			texture.Apply ();

			byte[] fileCodes = texture.EncodeToPNG ();
			string pngName = "Shore_" + System.DateTime.Now.ToString ("yyyyMMddHHmmss") + ".png";

			string newPath = path + pngName;
			File.WriteAllBytes (newPath, fileCodes);


			AssetDatabase.Refresh ();
			TextureImporter tImporter = AssetImporter.GetAtPath (newPath) as TextureImporter; 
			tImporter.maxTextureSize = 512;
			tImporter.textureType = TextureImporterType.Default;
			tImporter.textureFormat = TextureImporterFormat.Alpha8;
			tImporter.mipmapEnabled = false;
			AssetDatabase.ImportAsset (newPath, ImportAssetOptions.ForceUpdate); 
			GameObject.DestroyImmediate (texture);

			return newPath;

		}

		private static float RayCastToTerrain (Vector3 oriPos, LayerMask shoreLineDetectLayer,float detectDistance)
		{

				RaycastHit hit;

				Vector3 offsetUp = new Vector3 (0, 20, 0);
				//		int detectlayer =  (1<<8);
				if (Physics.Raycast (oriPos + offsetUp, Vector3.down, out hit, detectDistance + offsetUp.y, shoreLineDetectLayer.value)) {
						float dis = hit.distance - offsetUp.y;
						if (dis < 0) {
								dis = 0;
						}
						float power = dis / detectDistance;
			
			
						power = Mathf.Clamp01 (power);
		//					power = Mathf.Sqrt (power);
						return 1 - power;
				}

				return 0;

		}

		private static Color GaussianBlurPixel(float weight,float kernel,Texture2D texture,int x,int y,bool isX){
			float blurAmount  = 3;
			if(isX){

				x = x + (int)(kernel*blurAmount);

				x = Mathf.Clamp(x,0,texture.width-1);
			}else{
				y = y + (int)(kernel*blurAmount);
				
				y = Mathf.Clamp(y,0,texture.width-1);
			}

			Color pixel = texture.GetPixel(x,y)*weight;
			return pixel;
		}

		private  static Color  GaussianBlur(int x,int y,Texture2D texture,bool isX){
			Color  col = Color.clear;
			col+=GaussianBlurPixel(0.05f,-4,texture,x,y,isX);
			col+=GaussianBlurPixel(0.09f,-3f,texture,x,y,isX);
			col+=GaussianBlurPixel(0.12f,-2f,texture,x,y,isX);
			col+=GaussianBlurPixel(0.15f,-1f,texture,x,y,isX);
			col+=GaussianBlurPixel(0.18f,0f,texture,x,y,isX);
			col+=GaussianBlurPixel(0.15f,1f,texture,x,y,isX);
			col+=GaussianBlurPixel(0.12f,2f,texture,x,y,isX);
			col+=GaussianBlurPixel(0.09f,3f,texture,x,y,isX);
			col+=GaussianBlurPixel(0.05f,4f,texture,x,y,isX);

			return col;
		}

	}

}

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using Coolape;
using System.Collections.Generic;


[CustomEditor (typeof(CLRoleAvata), true)]
public class CLRoleAvataInspector : CLBehaviour4LuaInspector
{
	CLRoleAvata avata;
	private static bool isShowNewBodyPart = false;
	private static CLBodyPart newBodyPart = new CLBodyPart ();
	private static string cellName = "";
	private static GameObject onePartObj;
	private static Material material;
	private static int selectedPartindex = -1;
	private string testPartName = "";
	private string testCellName = "";
	static bool isShowBones = false;
	static bool isAddBones = false;
	static string addBoneName = "";
	static Transform addBone;

	public override void OnInspectorGUI ()
	{
		avata = (CLRoleAvata)target;
		ECLEditorUtl.BeginContents ();
		{
			if (isAddBones || isShowBones) {
				GUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Bone Name", GUILayout.Width (100));
					EditorGUILayout.LabelField ("Bone Transform", GUILayout.Width (150));
				}
				GUILayout.EndHorizontal ();
			}

			if (isShowBones) {
				for (int i = 0; i < avata.bonesNames.Count; i++) {
					GUILayout.BeginHorizontal ();
					{
						EditorGUILayout.TextField (avata.bonesNames [i], GUILayout.Width (100));
						EditorGUILayout.ObjectField (avata.bonesList [i], typeof(Transform), GUILayout.Width (150));
						if (GUILayout.Button ("-")) {
							if (EditorUtility.DisplayDialog ("Confirm", "确定要删除？", "Okay", "Cancel")) {
								avata.bonesNames.RemoveAt (i);
								avata.bonesList.RemoveAt (i);
								EditorUtility.SetDirty (avata);
								break;
							}
						}
					}
					GUILayout.EndHorizontal ();
				}
			}

			if (isAddBones) {
				GUILayout.BeginHorizontal ();
				{
					addBoneName = EditorGUILayout.TextField (addBoneName, GUILayout.Width (100));
					addBone = (Transform)(EditorGUILayout.ObjectField (addBone, typeof(Transform), GUILayout.Width (150)));
					if (GUILayout.Button ("+")) {
						if (string.IsNullOrEmpty (addBoneName)) {
							EditorUtility.DisplayDialog ("Confirm", "Bone Name can not null？", "Okay");
							return;
						}
						if (avata.bonesMap.ContainsKey (addBoneName)) {
							EditorUtility.DisplayDialog ("Confirm", "Bone Name allready exsit, please check then name Uniqueness？", "Okay");
							return;
						}
						if (addBone == null) {
							EditorUtility.DisplayDialog ("Confirm", "Bone can not null？", "Okay");
							return;
						}
						avata.bonesNames.Add (addBoneName);
						avata.bonesList.Add (addBone);
						avata.bonesMap [addBoneName] = addBone;
						EditorUtility.SetDirty (avata);
						addBone = null;
						addBoneName = "";
					}
				}
				GUILayout.EndHorizontal ();
			}

			GUILayout.BeginHorizontal ();
			{
				if (GUILayout.Button (isShowBones ? "Hide Bones" : "Show Bones")) {
					isShowBones = !isShowBones;
				}
				if (GUILayout.Button ("Add Bones")) {
					isAddBones = true;
				}
			}
			GUILayout.EndHorizontal ();
		}
		ECLEditorUtl.EndContents ();
		
		ECLEditorUtl.BeginContents ();
		{
			if (avata.bodyPartNames != null) {
				for (int i = 0; i < avata.bodyPartNames.Count; i++) {
					GUILayout.BeginHorizontal ();
					{
						if (selectedPartindex == i) {
							GUI.color = Color.yellow;
						}
						if (GUILayout.Button (avata.bodyPartNames [i])) {
							
							selectedPartindex = i;
							isShowNewBodyPart = false;
						}
						GUI.color = Color.white;
						if (GUILayout.Button ("-", GUILayout.Width (30))) {
							if (EditorUtility.DisplayDialog ("Confirm", "确定要删除？", "Okay", "Cancel")) {
								avata.bodyPartNames.RemoveAt (i);
								break;
							}
						}
					}
					GUILayout.EndHorizontal ();
					if (selectedPartindex == i) {
						avata.bodyParts [i] = showOnePart (avata.bodyParts [i], false);
					}
				}
			}
			if (isShowNewBodyPart) {
				newBodyPart = newBodyPart == null ? new CLBodyPart () : newBodyPart;
				newBodyPart = showOnePart (newBodyPart, true);
				GUILayout.BeginHorizontal ();
				{
					if (GUILayout.Button ("Clean")) {
						newBodyPart.cellNames.Clear ();
//                        newBodyPart.materials.Clear ();
						newBodyPart.materialNames.Clear ();
						newBodyPart.partObjs.Clear ();
						newBodyPart.animatorControllers.Clear ();
						cellName = "";
						onePartObj = null;
					}
					if (GUILayout.Button ("Save Body Part")) {
						doAddBodyPart ();
					}
				}
				GUILayout.EndHorizontal ();
			}

			if (GUILayout.Button ("Add Body Part")) {
				selectedPartindex = -1;
				newBodyPart = new CLBodyPart ();
				isShowNewBodyPart = true;
			}
		}
		ECLEditorUtl.EndContents ();
		testPartName = EditorGUILayout.TextField ("Part Name", testPartName);
		testCellName = EditorGUILayout.TextField ("Cell Name", testCellName);
		if (GUILayout.Button ("test")) {
			avata.setMapindex ();
			avata.switch2xx (testPartName, testCellName);
		}

		if (GUILayout.Button ("clean Material")) {
			avata.cleanMaterial ();
		}
		if (GUILayout.Button ("set Default Material")) {
			avata.setDefaultMaterial ();
		}
	}

	CLBodyPart showOnePart (CLBodyPart aBodyPart, bool isNew)
	{
		NGUIEditorTools.BeginContents ();
		{
			if (isNew) {
				GUI.color = Color.red;
				EditorGUILayout.LabelField ("新增一个部位", GUILayout.Width (200));
				GUI.color = Color.yellow;
			} else {
				GUI.color = Color.white;
			}

			GUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("身体部位", GUILayout.Width (100));
				aBodyPart.partName = GUILayout.TextField (aBodyPart.partName);
			}
			GUILayout.EndHorizontal ();
			
			//=========================
			GUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("换装方式", GUILayout.Width (100));
				aBodyPart.switchType = (CLSwitchType)EditorGUILayout.EnumPopup ("", aBodyPart.switchType);
			}
			GUILayout.EndHorizontal ();
			//=========================
			EditorGUILayout.LabelField ("该部位中所有部件(" + aBodyPart.cellNames.Count + ")", GUILayout.Width (150));
			if (aBodyPart.switchType == CLSwitchType.showOrHide) {
				NGUIEditorTools.BeginContents ();
				{
					if (aBodyPart.cellNames.Count > 0) {
						GUILayout.BeginHorizontal ();
						{
							EditorGUILayout.LabelField ("名字Key", GUILayout.Width (100));
							EditorGUILayout.LabelField ("部件(GameObject)");
						}
						GUILayout.EndHorizontal ();
					}
					for (int i = 0; i < aBodyPart.cellNames.Count; i++) {
						GUILayout.BeginHorizontal ();
						{
							aBodyPart.cellNames [i] = EditorGUILayout.TextField (aBodyPart.cellNames [i], GUILayout.Width (100));
							aBodyPart.partObjs [i] = (GameObject)(EditorGUILayout.ObjectField (aBodyPart.partObjs [i], typeof(GameObject)));
							if (GUILayout.Button ("-")) {
								aBodyPart.cellNames.RemoveAt (i);
								aBodyPart.partObjs.RemoveAt (i);
								break;
							}
						}
						GUILayout.EndHorizontal ();
						//=========================
					}
					GUILayout.BeginHorizontal ();
					{
						cellName = EditorGUILayout.TextField (cellName, GUILayout.Width (100));
						onePartObj = (GameObject)(EditorGUILayout.ObjectField (onePartObj, typeof(GameObject)));

						if (cellName == "" && onePartObj != null) {
							cellName = onePartObj.name;
						}
						if (GUILayout.Button ("+")) {
							if (string.IsNullOrEmpty (cellName) || onePartObj == null) {
								EditorUtility.DisplayDialog ("Alert", "名字和对象不能为空!", "ok");
							} else {
								aBodyPart.cellNames.Add (cellName);
								aBodyPart.partObjs.Add (onePartObj);
								cellName = "";
								onePartObj = null;
							}
						}
					}
					GUILayout.EndHorizontal ();
					//=========================
				}
				NGUIEditorTools.EndContents ();
				//=========================
			} else if (aBodyPart.switchType == CLSwitchType.switchShader) {
				GUILayout.BeginHorizontal ();
				{
					EditorGUILayout.LabelField ("Render(渲染器)", GUILayout.Width (100));
					aBodyPart.render = (Renderer)(EditorGUILayout.ObjectField (aBodyPart.render, typeof(Renderer)));
				}
				GUILayout.EndHorizontal ();
				//=========================
				NGUIEditorTools.BeginContents ();
				{
					if (aBodyPart.cellNames.Count > 0) {
						GUILayout.BeginHorizontal ();
						{
							EditorGUILayout.LabelField ("名字Key", GUILayout.Width (100));
							EditorGUILayout.LabelField ("部件(Material)");
						}
						GUILayout.EndHorizontal ();
					}
					for (int i = 0; i < aBodyPart.cellNames.Count; i++) {
						GUILayout.BeginHorizontal ();
						{
							aBodyPart.cellNames [i] = EditorGUILayout.TextField (aBodyPart.cellNames [i], GUILayout.Width (100));
							Material mat = null;
							if (aBodyPart.materialNames.Count > i) {
								mat = (Material)(EditorGUILayout.ObjectField (getMat (aBodyPart.materialNames [i]), typeof(Material)));
							} else {
								aBodyPart.materialNames = new List<string> (aBodyPart.cellNames.Count);
								mat = (Material)(EditorGUILayout.ObjectField (mat, typeof(Material)));
							}
							if (mat != null) {
								aBodyPart.materialNames [i] = getMatName (mat);
							}
							if (GUILayout.Button ("-")) {
								aBodyPart.cellNames.RemoveAt (i);
//                                aBodyPart.materials.RemoveAt (i);
								aBodyPart.materialNames.RemoveAt (i);
								break;
							}
						}
						GUILayout.EndHorizontal ();
						//=========================
					}
					GUILayout.BeginHorizontal ();
					{
						cellName = EditorGUILayout.TextField (cellName, GUILayout.Width (100));
						material = (Material)(EditorGUILayout.ObjectField (material, typeof(Material)));
						if (cellName == "" && material != null) {
							cellName = material.name;
						}
						if (GUILayout.Button ("+")) {
							if (string.IsNullOrEmpty (cellName) || material == null) {
								EditorUtility.DisplayDialog ("Alert", "名字和对象不能为空!", "ok");
							} else {
								aBodyPart.cellNames.Add (cellName);
								aBodyPart.materialNames.Add (getMatName (material));
								cellName = "";
								material = null;
							}
						}
					}
					GUILayout.EndHorizontal ();
					//=========================
				}
				NGUIEditorTools.EndContents ();
			}
			
			GUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("是否需要换动作", GUILayout.Width (100));
				aBodyPart.needSwitchController = EditorGUILayout.Toggle (aBodyPart.needSwitchController);
			}
			GUILayout.EndHorizontal ();
			if (aBodyPart.needSwitchController) {
				if (aBodyPart.cellNames.Count > 0) {
					GUILayout.BeginHorizontal ();
					{
						EditorGUILayout.LabelField ("名字Key", GUILayout.Width (100));
						EditorGUILayout.LabelField ("部件(AnimatorController)");
					}
					GUILayout.EndHorizontal ();
				}
				for (int i = aBodyPart.animatorControllers.Count; i < aBodyPart.cellNames.Count; i++) {
					aBodyPart.animatorControllers.Add (null);
				}
				for (int i = 0; i < aBodyPart.cellNames.Count; i++) {
					GUILayout.BeginHorizontal ();
					{
						EditorGUILayout.LabelField (aBodyPart.cellNames [i], GUILayout.Width (100));
						aBodyPart.animatorControllers [i] = (RuntimeAnimatorController)(EditorGUILayout.ObjectField (aBodyPart.animatorControllers [i], typeof(RuntimeAnimatorController)));
					}
					GUILayout.EndHorizontal ();
				}
			}
		}
		NGUIEditorTools.EndContents ();
		GUI.color = Color.white;
		return aBodyPart;
	}

	void doAddBodyPart ()
	{
		if (string.IsNullOrEmpty (newBodyPart.partName)) {
			EditorUtility.DisplayDialog ("Alert", "身体部位名称不能为空!", "ok");
			return;
		}
		if (newBodyPart.switchType == CLSwitchType.showOrHide) {
			if (newBodyPart.partObjs.Count <= 0) {
				EditorUtility.DisplayDialog ("Alert", "没有部件可保存!", "ok");
				return;
			}
			newBodyPart.materialNames.Clear ();
		} else if (newBodyPart.switchType == CLSwitchType.switchShader) {
			if (newBodyPart.materialNames.Count <= 0) {
				EditorUtility.DisplayDialog ("Alert", "没有部件可保存!", "ok");
				return;
			}
			newBodyPart.partObjs.Clear ();
		}

		avata.bodyPartNames.Add (newBodyPart.partName);

		avata.bodyParts.Add (newBodyPart);
		Debug.LogError ("newBodyPart.materialNames.Count==" + newBodyPart.materialNames.Count);
		Debug.LogError ("newBodyPart.partObjs.Count==" + newBodyPart.partObjs.Count);
		EditorUtility.SetDirty (avata);
		newBodyPart = null;
		isShowNewBodyPart = false;
	}

	string getMatName (Material mat)
	{
		string materialPath = ECLEditorUtl.getPathByObject (mat);
		materialPath = materialPath.Replace (CLPathCfg.self.basePath + "/upgradeRes4Dev/other/Materials/", "");
		materialPath = materialPath.Replace (".mat", "");
		materialPath = materialPath.Replace ("/", ".");
		return materialPath;
	}

	Material getMat (string matName)
	{
		if (string.IsNullOrEmpty (matName))
			return null;
		string path = "Assets/" + CLPathCfg.self.basePath + "/upgradeRes4Dev/other/Materials/" + matName.Replace(".", "/") + ".mat";
		Material mat = AssetDatabase.LoadAssetAtPath (path, typeof(Material)) as Material;
		return mat;
	}
}

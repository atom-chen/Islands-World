/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   纸娃娃
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Coolape
{
	//	[RequireComponent(typeof(Animator))]
	public class CLRoleAvata : MonoBehaviour
	{
		// 需要用到的骨骼关节点
		[SerializeField]
		public List<string> bonesNames = new List<string> ();
		[SerializeField]
		public List<Transform> bonesList = new List<Transform> ();
		Hashtable _bonesMap;

		public Hashtable bonesMap {
			get {
				if (_bonesMap == null) {
					_bonesMap = new Hashtable ();
					for (int i = 0; i < bonesNames.Count; i++) {
						_bonesMap [bonesNames [i]] = bonesList [i];
					}
				}
				return _bonesMap;
			}
		}

		public Transform getBoneByName (string bname)
		{
			return (Transform)(bonesMap [bname]);
		}

		public Animator animator;

		[SerializeField]
		public List<string> bodyPartNames = new List<string> ();
		[SerializeField]
		public List<CLBodyPart> bodyParts = new List<CLBodyPart> ();
		Hashtable mapIndex = new Hashtable ();
		bool isInited = false;

		public void setMapindex ()
		{
			for (int i = 0; i < bodyPartNames.Count; i++) {
				mapIndex [bodyPartNames [i]] = i;
			}
		}
		bool isQuit = false;
		public void OnApplicationQuit (){
			isQuit = true;
		}

		public void OnDestroy ()
		{
			if(!isQuit)
				cleanMaterial ();
		}
		public void cleanMaterial ()
		{
			if (!isInited) {
				isInited = true;
				setMapindex ();
			}
			for (int i = 0; i < bodyParts.Count; i++) {
				bodyParts [i].cleanMaterial ();
			}
		}

		public void setDefaultMaterial ()
		{
			if (!isInited) {
				isInited = true;
				setMapindex ();
			}
			for (int i = 0; i < bodyParts.Count; i++) {
				bodyParts [i].setDefaultMaterial ();
			}
		}

		/// <summary>
		/// Switch2xx the specified partName and cellName.变装
		/// </summary>
		/// <param name="partName">Part name.</param>身体部位
		/// <param name="cellName">Cell name.</param>服装、表情、装备等的名称
		public void switch2xx (string partName, string cellName)
		{
			switch2xx (partName, cellName, null);
		}
		public void switch2xx (string partName, string cellName, object callback)
		{
			if (!isInited) {
				isInited = true;
				setMapindex ();
			}
			try {
				int index = MapEx.getInt (mapIndex, partName);
				CLBodyPart part = bodyParts [index];
				if (part == null)
					return;
				part.switchByName (cellName, animator, callback);
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
		}
	}

	[System.Serializable]
	public class CLBodyPart
	{
		public string partName = "";
		//身体部位
		public CLSwitchType switchType = CLSwitchType.showOrHide;
		public List<string> cellNames = new List<string> ();
		//身体部位中各个部件的名字
		public Renderer render;
		//    [System.NonSerialized]
		//    public List<Material> materials = null;
		public List<string> materialNames = new List<string> ();
		public List<GameObject> partObjs = new List<GameObject> ();
		public bool needSwitchController = false;
		public List<RuntimeAnimatorController> animatorControllers = new List<RuntimeAnimatorController> ();
		public Hashtable mapIndex = new Hashtable ();
		[System.NonSerialized]
		bool isInited = false;

		public void init ()
		{
			if (!isInited) {
				isInited = true;
				for (int i = 0; i < cellNames.Count; i++) {
					mapIndex [cellNames [i]] = i;
				}
			}
		}

		public void cleanMaterial ()
		{
			if (switchType == CLSwitchType.switchShader) {
				if (render.sharedMaterial != null) {
					#if UNITY_EDITOR
					if (Application.isPlaying) {
						CLMaterialPool.returnObj (render.sharedMaterial.name);
					}
					#else
					CLMaterialPool.returnObj (render.sharedMaterial.name);
					#endif
					render.sharedMaterial = null;
				}
			}
		}

		public void setDefaultMaterial ()
		{
			if (switchType == CLSwitchType.switchShader) {
				if (materialNames.Count > 0) {
					setMat (render, materialNames [0], null);
				}
			}
		}

		public void switchByName (string cellName, Animator animator, object callback)
		{
			if (!isInited) {
				init ();
			}
			int index = MapEx.getInt (mapIndex, cellName);

			if (needSwitchController) {
				if (animator != null) {
					animator.runtimeAnimatorController = animatorControllers [index];
				} else {
					Debug.LogError ("animator is null");
				}
			}

			if (switchType == CLSwitchType.showOrHide) {
				for (int i = 0; i < partObjs.Count; i++) {
					if (i == index) {
						NGUITools.SetActive (partObjs [i], true);
					} else {
						NGUITools.SetActive (partObjs [i], false);
					}
				}
				Utl.doCallback (callback);
			} else if (switchType == CLSwitchType.switchShader) {
				if (render.sharedMaterial != null) {
					string mName = render.sharedMaterial.name;
//					mName = mName.Replace(" (Instance)", "");
					CLMaterialPool.returnObj (mName);
					render.sharedMaterial = null;
				}
				setMat (render, materialNames [index], callback);
			}

		}

		public void setMat (Renderer render, string materialName, object callback)
		{
			ArrayList list = new ArrayList ();
			list.Add (render);
			list.Add (materialName);
			list.Add (callback);
			#if UNITY_EDITOR
			if (Application.isPlaying) {
				CLMaterialPool.borrowObjAsyn (materialName, (Callback)onSetPrefab, list);
			} else {
				string path = "Assets/" + CLPathCfg.self.basePath + "/upgradeRes4Dev/other/Materials/" + materialName.Replace (".", "/") + ".mat";
				Material mat = AssetDatabase.LoadAssetAtPath (path, typeof(Material)) as Material;
				render.sharedMaterial = mat;
			}
			#else
			CLMaterialPool.borrowObjAsyn (materialName, (Callback)onSetPrefab, list);
			#endif
		}

		void onSetPrefab (params object[] args)
		{
			Material mat = (Material)(args [1]);
			if (mat != null) {
				ArrayList list = (ArrayList)(args [2]);
				Renderer render = (Renderer)(list [0]);
				string name = list [1].ToString ();
				object callback = list [2];
//            setMat(render, name);
				if (render != null) {
					render.sharedMaterial = mat;
				}
				Utl.doCallback (callback);
			} else {
				Debug.LogWarning ("Get material is null: " + args [0]);
			}
		}
	}

	public enum CLSwitchType
	{
		showOrHide,
		switchShader,
	}
}

/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   
  * 1.render ->material->texture 
  * 2.mesh->fbx 如何通过mesh的名字找到fbx对象(mesh fillter, SkinnedMeshRenderer.mesh)
  * 3.Animator->controller->fbx
  * 4.Animation->fbx
  * 5.sound
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Coolape
{
	public class CLSharedAssets : MonoBehaviour
	{
		public static CLDelegate OnFinishSetCallbacks = new CLDelegate ();
		[SerializeField]
		public List<CLMaterialInfor> materials = new List<CLMaterialInfor> ();
		[SerializeField]
		public List<CLMeshInfor> meshs = new List<CLMeshInfor> ();

		bool isFinishInited = false;
		bool isAllAssetsLoaded = false;
		public bool isDonnotResetAssets = false;
		public bool isSkipModel = true;
		public object progressCallback;

		public List<EventDelegate> onFinshLoad = new List<EventDelegate> ();

		public void reset ()
		{
			isAllAssetsLoaded = false;
		}
		// Use this for initialization
		void Start ()
		{
			if (isFinishInited)
				return;
			isFinishInited = true;
			#if UNITY_EDITOR
			init ((Callback)onFinishSetAsset, null, progressCallback);
			#else
			init ((Callback)onFinishSetAsset, null);
			#endif
		}

		void onFinishSetAsset (params object[] paras)
		{
//			if (isResetShaderInEdeitorMode) {
//				Utl.setBodyMatEdit (transform);
//			}
			EventDelegate.Execute (onFinshLoad, gameObject);
		}

		public bool isEmpty ()
		{
			return (materials == null || materials.Count == 0) && (meshs == null || meshs.Count == 0);
		}

		public void cleanRefOnly ()
		{
			foreach (var matInfor in materials) {
				if (matInfor.render.sharedMaterials != null && matInfor.render.sharedMaterials.Length > matInfor.index) {
					Material[] mats = matInfor.render.sharedMaterials;
					mats [matInfor.index] = null;
					matInfor.render.sharedMaterials = mats;
					if (matInfor.index == 0) {
						matInfor.render.sharedMaterial = null;
						matInfor.render.material = null;
					}
				}
			}
			foreach (var meshInfor in meshs) {
				if (meshInfor.meshFilter != null) {
					meshInfor.meshFilter.sharedMesh = null;
				}
				if (meshInfor.skinnedMesh != null) {
					meshInfor.skinnedMesh.sharedMesh = null;
				}
				if(meshInfor.animator != null) {
					meshInfor.animator.avatar = null;
				}
			}
		}

		public void cleanRefAssets ()
		{
			#if UNITY_EDITOR
			foreach (var matInfor in materials) {
				string matPath = "Assets/" + CLPathCfg.self.basePath + "/upgradeRes4Dev/other/Materials/" + matInfor.materialName.Replace (".", "/") + ".mat";
				Debug.Log("matPath==" + matPath);
				Material mat = AssetDatabase.LoadAssetAtPath (matPath, typeof(Material)) as Material;
				if (mat == null) {
					continue; 
				}
				CLMaterialPool.cleanTexRef (matInfor.materialName, mat);
				if (matInfor.render.sharedMaterials != null && matInfor.render.sharedMaterials.Length > matInfor.index) {
					Material[] mats = matInfor.render.sharedMaterials;
					mats [matInfor.index] = null;
					matInfor.render.sharedMaterials = mats;
					if (matInfor.index == 0) {
						matInfor.render.sharedMaterial = null;
						matInfor.render.material = null;
					}
				}
			}

			foreach (var meshInfor in meshs) {
				if (meshInfor.meshFilter != null) {
					meshInfor.meshFilter.sharedMesh = null;
				}
				if (meshInfor.skinnedMesh != null) {
					meshInfor.skinnedMesh.sharedMesh = null;
				}
				if(meshInfor.animator != null) {
					meshInfor.animator.avatar = null;
				}
			}
			#endif
		}

		public void init (object finishCallback, object orgs)
		{
			init (finishCallback, orgs, null);
		}

		public void init (object finishCallback, object orgs, object progressCallback)
		{
			this.progressCallback = progressCallback;	
			if (isAllAssetsLoaded || isDonnotResetAssets) {
				Utl.doCallback (finishCallback, orgs);
			} else {
				OnFinishSetCallbacks.add (gameObject.GetInstanceID ().ToString (), finishCallback, orgs);
				resetAssets ();
			}
		}

		float _progress = 0;
		//进度
		public float progress {
			get {
				return _progress;
			}
			set {
				_progress = value;
				Utl.doCallback (progressCallback, _progress);
			}
		}

		public void resetAssets ()
		{
			if (isAllAssetsLoaded)
				return;
			if (!isSkipModel) {
				//Set model
				setMesh ();
			} else {
				//TODO:set sound
				//set Material
				setMaterial ();
			}
		}

		public void setMaterial ()
		{
//			cleanRefOnly ();
			CLMaterialInfor clMat;
			if (materials.Count > 0) {
				clMat = materials [0];
				clMat.setMaterial ((Callback)onFinishSetMat, 0);
			} else {
				callbackOnFinish ();
			}
		}

		public void setMesh ()
		{
			CLMeshInfor clMesh;
			if (meshs.Count > 0) {
				clMesh = meshs [0];
				clMesh.setMesh ((Callback)onFinishSetMesh, 0);
			} else {
				callbackOnFinishMesh ();
			}
		}

		void onFinishSetMesh (params object[] paras)
		{
			int index = (int)paras [0];
			index++;
			progress = (float)index / (materials.Count + meshs.Count);
			if (index < meshs.Count) {
				CLMeshInfor clMat = meshs [index];
				clMat.setMesh ((Callback)onFinishSetMesh, index);
			} else {
				//Finished
				callbackOnFinishMesh ();
			}
		}

		void onFinishSetMat (params object[] paras)
		{
			int index = (int)paras [0];
			index++;
			progress = (float)(index + meshs.Count) / (materials.Count + meshs.Count);
			if (index < materials.Count) {
				CLMaterialInfor clMat = materials [index];
				clMat.setMaterial ((Callback)onFinishSetMat, index);
			} else {
				//Finished
				isAllAssetsLoaded = true;
				callbackOnFinish ();
			}
		}

		void callbackOnFinishMesh ()
		{
			setMaterial ();	
		}

		void callbackOnFinish ()
		{
			string key = gameObject.GetInstanceID ().ToString ();
			ArrayList callbackList = OnFinishSetCallbacks.getDelegates (key);
			int count = callbackList.Count;
			ArrayList cell = null;
			object cb = null;
			object orgs = null;
			for (int i = 0; i < count; i++) {
				cell = callbackList [i] as ArrayList;
				if (cell != null && cell.Count > 1) {
					cb = cell [0];
					orgs = cell [1];
					Utl.doCallback (cb, orgs);
				}
			}
			callbackList.Clear ();
			OnFinishSetCallbacks.removeDelegates (key);
		}

		public void OnDestroy ()
		{
			if (isFinishInited) {
				returnAssets ();
			}
		}

		public void returnAssets ()
		{
			if (isDonnotResetAssets)
				return;
			CLMaterialInfor clMat;
			for (int i = 0; i < materials.Count; i++) {
				clMat = materials [i];
				clMat.returnMaterial ();
			}
			CLMeshInfor clMesh;
			for (int i = 0; i < meshs.Count; i++) {
				clMesh = meshs [i];
				clMesh.returnMesh ();
			}
			isFinishInited = false;
			isAllAssetsLoaded = false;
		}


		[System.Serializable]
		public class CLMeshInfor
		{
			public MeshFilter meshFilter;
			public SkinnedMeshRenderer skinnedMesh;
			//			public string meshName;
			public Animator animator;
			public string modelName;
			public string meshName;

			object finishCallback;
			object callbackPrgs;

			public void returnMesh ()
			{
				CLModePool.returnObj (modelName);
			}

			public void setMesh (Callback onFinishCallback, object orgs)
			{
				finishCallback = onFinishCallback;
				callbackPrgs = orgs;
				#if UNITY_EDITOR
				if (!CLCfgBase.self.isEditMode || Application.isPlaying) {
					if (string.IsNullOrEmpty (modelName)) {
						Debug.LogWarning (" then model name is null===");
					} else {
						CLModePool.borrowObjAsyn (modelName, (Callback)onGetModel);
					}
				} else {
					string tmpPath = "Assets/" + CLPathCfg.self.basePath + "/upgradeRes4Dev/other/model/" + modelName.Replace (".", "/") + ".FBX";
					Debug.Log (tmpPath);
					GameObject model = AssetDatabase.LoadAssetAtPath (
						                   tmpPath, typeof(UnityEngine.Object)) as GameObject;
					onGetModel (modelName, model);
				}
				#else
				CLModePool.borrowObjAsyn (modelName, (Callback)onGetModel);
				#endif
			}

			void onGetModel (params object[] paras)
			{
				string name = paras [0].ToString ();
				GameObject obj = paras [1] as GameObject;
				if (obj == null) {
					return;
				}
				if (obj != null) {
					if (meshFilter != null) {

						MeshFilter[] mfs = obj.GetComponentsInChildren<MeshFilter>();
						if (mfs != null && mfs.Length > 0) {
							for (int i = 0; i < mfs.Length; i++) {
								if (mfs [i].sharedMesh.name == meshName) {
									meshFilter.sharedMesh = mfs [i].sharedMesh;

									#if UNITY_EDITOR
									if (!Application.isPlaying) {
										EditorUtility.SetDirty (meshFilter);
									}
									#endif
								}
							}
						}
					}
					if (skinnedMesh != null) {
						
						SkinnedMeshRenderer smr = obj.GetComponent<SkinnedMeshRenderer> ();
						if(smr == null) {
							smr = obj.GetComponentInChildren<SkinnedMeshRenderer> ();

							SkinnedMeshRenderer[] smrs = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
							if (smrs != null) {
								for (int i = 0; i < smrs.Length; i++) {
									if (smrs [i].sharedMesh.name == meshName) {
										skinnedMesh.sharedMesh = smr.sharedMesh;

										#if UNITY_EDITOR
										if (!Application.isPlaying) {
											EditorUtility.SetDirty (skinnedMesh);
										}
										#endif
									}
								}
							}
						} else {
							if (smr != null) {
								skinnedMesh.sharedMesh = smr.sharedMesh;
								#if UNITY_EDITOR
								if (!Application.isPlaying) {
									EditorUtility.SetDirty (skinnedMesh);
								}
								#endif
							}
						}
					}
					if (animator != null) {
						Animator _animator = obj.GetComponentInChildren<Animator> ();
						if (_animator != null) {
							animator.avatar = _animator.avatar;
							#if UNITY_EDITOR
							if (!Application.isPlaying) {
								EditorUtility.SetDirty (animator);
							}
							#endif
						}
					}
				}
				Utl.doCallback (finishCallback, callbackPrgs);
			}
		}

		[System.Serializable]
		public class CLMaterialInfor
		{
			public Renderer render;
			public int index = 0;
			public string materialName;
			//						public List<string> shaderPropNames = new List<string>();
			//						public List<string> textures = new List<string>();
			//						public List<string> texturesPath = new List<string> ();
			object finishCallback;
			object callbackPrgs;

			public void returnMaterial ()
			{
				CLMaterialPool.returnObj (materialName);
			}

			public void setMaterial (Callback onFinishCallback, object orgs)
			{
				finishCallback = onFinishCallback;
				callbackPrgs = orgs;
				#if UNITY_EDITOR
				if (!CLCfgBase.self.isEditMode || Application.isPlaying) {
					if (string.IsNullOrEmpty (materialName)) {
						Debug.LogWarning (" then materialName is null===" + render.transform.parent.parent.name);
					} else {
						CLMaterialPool.borrowObjAsyn (materialName, (Callback)onGetMat);
					}
				} else {
					string tmpPath = "Assets/" + CLPathCfg.self.basePath + "/upgradeRes4Dev/other/Materials/" + materialName.Replace (".", "/") + ".mat";
					Debug.Log (tmpPath);
					Material mat = AssetDatabase.LoadAssetAtPath (
						               tmpPath, typeof(UnityEngine.Object)) as Material;
					
					CLMaterialPool.resetTexRef (materialName, mat, null, null);
					onGetMat (materialName, mat);
				}
				#else
				CLMaterialPool.borrowObjAsyn(materialName, (Callback)onGetMat);
				#endif
			}

			void onGetMat (params object[] paras)
			{
                string name = paras [0].ToString ();
				Material mat = paras [1] as Material;
				if (index == 0) {
					render.sharedMaterial = mat;
				}
				Material[] mats = render.sharedMaterials;//new Material[render.sharedMaterials.Length];
				mats [index] = mat;
				render.sharedMaterials = mats;
				#if UNITY_EDITOR
				if (!Application.isPlaying) {
					EditorUtility.SetDirty (render);
				}
				#endif
				Utl.doCallback (finishCallback, callbackPrgs);
			}
		}
	}
}

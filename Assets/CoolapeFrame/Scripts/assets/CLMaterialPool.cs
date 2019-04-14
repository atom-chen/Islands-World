/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   材质球对象池
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Coolape
{
    public class CLMaterialPool : CLAssetsPoolBase<Material>
    {
        public static CLMaterialPool pool = new CLMaterialPool();

        public override string getAssetPath(string name)
        {
            string path = PStr.b().a(CLPathCfg.self.basePath).a("/")
                .a(CLPathCfg.upgradeRes).a("/other/Materials").e();
            return wrapPath(path, name);
        }

        public override Material _borrowObj(string name)
        {
            ArrayList propNames = null;
            ArrayList texNames = null;
            ArrayList texPaths = null;

            if (getMaterialTexCfg(name, ref propNames, ref texNames, ref texPaths))
            {
                if (texNames != null)
                {
                    for (int i = 0; i < texNames.Count; i++)
                    {
                        CLTexturePool.borrowObj(texNames[i].ToString());
                    }
                }
            }
            return base._borrowObj(name, true);
        }


        public override bool isAutoReleaseAssetBundle
        {
            get
            {
                return false;
            }
        }

        public static bool havePrefab(string name)
        {
            return pool._havePrefab(name);
        }

        public static void clean()
        {
            pool._clean();
        }

        public static void setPrefab(string name, object finishCallback)
        {
            setPrefab(name, finishCallback, null, null);
        }

        public static void setPrefab(string name, object finishCallback, object args)
        {
            pool._setPrefab(name, finishCallback, args, null);
        }

        public static void setPrefab(string name, object finishCallback, object args, object progressCB)
        {
            pool._setPrefab(name, finishCallback, args, progressCB);
        }

        public static Material borrowObj(string name)
        {
            return pool._borrowObj(name, true);
        }

        public static void borrowObjAsyn(string name, object onGetCallbak)
        {
            borrowObjAsyn(name, onGetCallbak, null);
        }

        public static void borrowObjAsyn(string name, object onGetCallbak, object orgs)
        {
            pool._borrowObjAsyn(name, onGetCallbak, orgs, null);
        }

        public static void borrowObjAsyn(string name, object onGetCallbak, object orgs, object progressCB)
        {
            pool._borrowObjAsyn(name, onGetCallbak, orgs, progressCB);
        }

        public static void returnObj(string name)
        {
            //return texture
            ArrayList propNames = null;
            ArrayList texNames = null;
            ArrayList texPaths = null;
            if (getMaterialTexCfg(name, ref propNames, ref texNames, ref texPaths))
            {
                if (texNames != null)
                {
                    for (int i = 0; i < texNames.Count; i++)
                    {
                        CLTexturePool.returnObj(texNames[i].ToString());
                    }
                }
            }
            // Then return material
            pool._returnObj(name, null);
        }

        public static void cleanTexRef(string name, Material mat)
        {
            ArrayList propNames = null;
            ArrayList texNames = null;
            ArrayList texPaths = null;
            if (CLMaterialPool.getMaterialTexCfg(name, ref propNames, ref texNames, ref texPaths))
            {
                if (propNames != null)
                {
                    for (int i = 0; i < propNames.Count; i++)
                    {
                        mat.SetTexture(propNames[i].ToString(), null);
                    }
                }
            }
        }

        public override void sepcProc4Assets(Material mat, object cb, object args, object progressCB)
        {
            if (mat != null)
            {
#if UNITY_EDITOR
                mat.shader = Shader.Find(mat.shader.name);
#endif
                resetTexRef(mat.name, mat, cb, args);
            } else {
                Debug.LogError("get mat is null.");
            }
        }

        public static void resetTexRef(string matName, Material mat, object cb, object args)
        {
            ArrayList propNames = null;
            ArrayList texNames = null;
            ArrayList texPaths = null;
            if (getMaterialTexCfg(matName, ref propNames, ref texNames, ref texPaths))
            {
                if (propNames != null)
                {
                    NewList list = null;
                    //取得texture
                    int count = propNames.Count;
                    //for (int i = 0; i < count; i++)
                    if(count > 0)
                    {
                        //int i = 0;
                        list = ObjPool.listPool.borrowObject();
                        list.Add(mat);
                        //list.Add(propNames[i]);
                        //list.Add(count);
                        list.Add(cb);
                        list.Add(args);
                        list.Add(propNames);
                        list.Add(texNames);
                        list.Add(texPaths);
                        list.Add(0);

                        doresetTexRef(list);
                    }
                }
                else
                {
                    Debug.LogError("propNames is null =====");
                    pool.finishSetPrefab(mat);
                    Utl.doCallback(cb, mat, args);
                }
            }
            else
            {
                Debug.LogError("get material tex failed");
                pool.finishSetPrefab(mat);
                Utl.doCallback(cb, mat, args);
            }
        }

        public static void doresetTexRef(NewList inputs) {
            ArrayList texNames = inputs[4] as ArrayList;
            ArrayList texPaths = inputs[5] as ArrayList;
            int i = (int)(inputs[6]);
#if UNITY_EDITOR
            if (!CLCfgBase.self.isEditMode || Application.isPlaying)
            {
                CLTexturePool.setPrefab(texNames[i].ToString(), (Callback)onGetTexture, inputs, null);
            }
            else
            {
                string tmpPath = "Assets/" + texPaths[i];
                Texture tex = AssetDatabase.LoadAssetAtPath(
                                  tmpPath, typeof(UnityEngine.Object)) as Texture;
                onGetTexture(tex, inputs);
            }
#else
            CLTexturePool.setPrefab(texNames [i].ToString (), (Callback)onGetTexture, inputs, null);
#endif
        }

        public static void onGetTexture(params object[] paras)
        {
            string name = "";
            try
            {
                Texture tex = paras[0] as Texture;
                NewList list = paras[1] as NewList;
                Material mat = list[0] as Material;
                int i = (int)(list[6]);
                ArrayList propNames = list[3] as ArrayList;
                string propName = propNames[i].ToString();

                //				name = paras [0].ToString ();
                if (tex == null)
                {
                    ArrayList texPaths = list[5] as ArrayList;
                    Debug.LogError("Get tex is null." + mat.name + "===" + texPaths[i]);
                }
                else
                {
                    name = tex.name;
                    // 设置material对应属性的texture
                    mat.SetTexture(propName, tex);
                }
                int count = propNames.Count;
                i++;
                if (i >= count)
                {
                    pool.finishSetPrefab(mat);
                    //finished
                    Callback cb = list[1] as Callback;
                    object agrs = list[2];
                    Utl.doCallback(cb, mat, agrs);
                    ObjPool.listPool.returnObject(list);
                } else {
                    list[6] = i;
                    doresetTexRef(list);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("name==========" + name + "==" + e);
            }
        }

        //==========================================================
        // material cfg proc
        //==========================================================
        static Hashtable _materialTexRefCfg = null;
        static string _materialTexRefCfgPath = null;
        public static string materialTexRefCfgPath
        {
            get
            {
                if (string.IsNullOrEmpty(_materialTexRefCfgPath))
                {
#if UNITY_EDITOR
                    _materialTexRefCfgPath = PStr.b().a(Application.dataPath).a("/").a(CLPathCfg.self.basePath).a("/upgradeRes4Dev/priority/cfg/materialTexRef.cfg").e();
#else
        _materialTexRefCfgPath = PStr.b().a(CLPathCfg.self.basePath).a("/upgradeRes/priority/cfg/materialTexRef.cfg").e();
#endif
                }
                return _materialTexRefCfgPath;
            }
        }

        public static Hashtable materialTexRefCfg {
			get {
				if (_materialTexRefCfg == null) {
					_materialTexRefCfg = readMaterialTexRefCfg ();
				}
				return _materialTexRefCfg;
			}
			set {
				_materialTexRefCfg = value;
			}
		}

		/// <summary>
		/// Gets the material cfg.取得material引用图片的配置
		/// </summary>
		/// <returns><c>true</c>, if material cfg was gotten, <c>false</c> otherwise.</returns>
		/// <param name="matName">Mat name.</param>
		/// <param name="propNames">Property names.</param>
		/// <param name="texNames">Tex names.</param>
		/// <param name="texPaths">Tex paths.</param>
		public static bool getMaterialTexCfg (string matName, ref ArrayList propNames, ref ArrayList texNames, ref ArrayList texPaths)
		{
			Hashtable cfg = MapEx.getMap (materialTexRefCfg, matName);
			bool ret = true;
			if (cfg == null) {
                Debug.LogError("Get MaterialTexCfg is null!" + matName);
				ret = false;
			} else {
				propNames = cfg ["pp"] as ArrayList;
				texNames = cfg ["tn"] as ArrayList;
				texPaths = cfg ["tp"] as ArrayList;
			}
			return ret;
		}

		public static Hashtable readMaterialTexRefCfg ()
        {
            Hashtable ret = null;
			#if UNITY_EDITOR
			byte[] buffer = File.Exists (materialTexRefCfgPath) ? File.ReadAllBytes (materialTexRefCfgPath) : null;
			#else
			byte[] buffer = FileEx.readNewAllBytes (materialTexRefCfgPath);
			#endif
			if (buffer != null) {
				MemoryStream ms = new MemoryStream ();
				ms.Write (buffer, 0, buffer.Length);
				ms.Position = 0;
				object obj = B2InputStream.readObject (ms);
				if (obj != null) {
					ret = obj as Hashtable;
				}
			}
			ret = ret == null ? new Hashtable () : ret;
			return ret;
		}
	}
}

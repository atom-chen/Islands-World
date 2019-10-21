/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  工具类
  *Others:  
  *History:
*********************************************************************************
*/ 

using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using XLua;

#if !UNITY_WEBPLAYER
using System.Net.NetworkInformation;
#endif

namespace Coolape
{
	public static class Utl
	{
		public static Vector3 kXAxis = new Vector3 (1.0f, 0.0f, 0.0f);
		public static Vector3 kZAxis = new Vector3 (0.0f, 0.0f, 1.0f);
		static string cacheUuid = "";

		public static string uuid {
			get {
				if (string.IsNullOrEmpty (cacheUuid)) {
					if (Application.platform == RuntimePlatform.Android) {
						cacheUuid = SystemInfo.deviceUniqueIdentifier;
					} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
						#if UNITY_IOS || UNITY_IPHONE || UNITY_STANDALONE_OSX
						string jsonStr = KeyChain.BindGetKeyChainUser ();
						Debug.Log("jsonStr===========" + jsonStr);
						if (string.IsNullOrEmpty (jsonStr)) {
							cacheUuid = SystemInfo.deviceUniqueIdentifier;
							KeyChain.BindSetKeyChainUser ("0", cacheUuid);
						} else {
							Hashtable m = JSON.DecodeMap(jsonStr);
							cacheUuid = MapEx.getString(m, "uuid");
							if(string.IsNullOrEmpty(cacheUuid)) {
								cacheUuid = SystemInfo.deviceUniqueIdentifier;
								KeyChain.BindSetKeyChainUser ("0", cacheUuid);
							}
						}
						#endif
					} else {
						cacheUuid = GetMacAddress ();
					}
				}
				return cacheUuid;
			}
		}

		public static string GetMacAddress ()
		{
			#if !UNITY_WEBPLAYER
			string macAdress = "";
			NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces ();

			foreach (NetworkInterface adapter in nics) {
				PhysicalAddress address = adapter.GetPhysicalAddress ();
				if (address.ToString () != "") {
					macAdress = address.ToString ();
					return macAdress;
				}
			}
			#endif
			return "00";
		}

		public static Hashtable vector2ToMap (Vector2 v2)
		{
			Hashtable r = new Hashtable ();
			r ["x"] = (double)(v2.x);
			r ["y"] = (double)(v2.y);
			return r;
		}

		public static Hashtable vector3ToMap (Vector3 v3)
		{
			Hashtable r = new Hashtable ();
			r ["x"] = (double)(v3.x);
			r ["y"] = (double)(v3.y);
			r ["z"] = (double)(v3.z);
			return r;
		}

		public static Hashtable vector4ToMap (Vector4 v4)
		{
			Hashtable r = new Hashtable ();
			r ["x"] = (double)(v4.x);
			r ["y"] = (double)(v4.y);
			r ["z"] = (double)(v4.z);
			r ["w"] = (double)(v4.w);
			return r;
		}

		public static Vector2 mapToVector2 (Hashtable map)
		{
			if (map == null) {
				return Vector2.zero;
			}
			return new Vector2 (
				(float)(MapEx.getDouble (map, "x")),
				(float)(MapEx.getDouble (map, "y")));
		}

		public static Vector3 mapToVector3 (Hashtable map)
		{
			if (map == null) {
				return Vector3.zero;
			}
			return new Vector3 (
				(float)(MapEx.getDouble (map, "x")),
				(float)(MapEx.getDouble (map, "y")),
				(float)(MapEx.getDouble (map, "z")));
		}

		public static Hashtable colorToMap (Color color)
		{
			Hashtable r = new Hashtable ();
			r ["r"] = (double)(color.r);
			r ["g"] = (double)(color.g);
			r ["b"] = (double)(color.b);
			r ["a"] = (double)(color.a);
			return r;
		}

		public static Color mapToColor (Hashtable map)
		{
			Color c = new Color (
				          (float)(MapEx.getDouble (map, "r")),
				          (float)(MapEx.getDouble (map, "g")),
				          (float)(MapEx.getDouble (map, "b")),
				          (float)(MapEx.getDouble (map, "a"))
			          );
			return c;
		}

		/// <summary>
		/// Filters the path.过滤路径
		/// </summary>
		/// <returns>
		/// The path.
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		public static string filterPath (string path)
		{
			string r = path;
			if (path.IndexOf ("Assets/") == 0) {
				r = StrEx.Mid (path, 7);
			}
			r = r.Replace ("\\", "/");
			r = r.Replace ("/upgradeRes4Dev", "/upgradeRes");
			r = r.Replace ("/upgradeRes4Publish/", "/upgradeRes/");
			return r;
		}

		/// <summary>
		/// Gets the animation curve.创建动画曲线
		/// </summary>
		/// <returns>
		/// The animation curve.
		/// </returns>
		/// <param name='list'>
		/// List.
		/// </param>
		/// <param name='postWrapMode'>
		/// Post wrap mode.
		/// </param>
		/// <param name='preWrapMode'>
		/// Pre wrap mode.
		/// </param>
		public static AnimationCurve getAnimationCurve (ArrayList list, WrapMode postWrapMode, WrapMode preWrapMode)
		{
			if (list == null || list.Count <= 0) {
				return null;
			}
			int len = list.Count;
			Keyframe[] ks = new Keyframe[len];
			for (int i = 0; i < len; i++) {
				Hashtable m = (Hashtable)list [i];
				float inTangent = (float)MapEx.getDouble (m, "inTangent");
				float outTangent = (float)MapEx.getDouble (m, "outTangent");
				float time = (float)MapEx.getDouble (m, "time");
				float value = (float)MapEx.getDouble (m, "value");
				ks [i] = new Keyframe (time, value, inTangent, outTangent);
			}
			AnimationCurve curve = new AnimationCurve (ks);
			curve.preWrapMode = preWrapMode;
			curve.postWrapMode = postWrapMode;
			return curve;
		}

		/// <summary>
		/// Rotates the towards.转向目标方向(支持提前量)
		/// </summary>
		/// <param name='dir'>
		/// Dir.
		/// </param>
		public static void rotateTowardsForecast (Transform trsf, Transform target, float forecastDis = 0)
		{
			Vector3 dir = Vector3.zero;
			if (forecastDis > 0) {
				dir = target.position + target.forward * forecastDis - trsf.position;
			} else {
				dir = target.position - trsf.position;
			}
			RotateTowards (trsf, dir);
		}

		/// <summary>
		/// Rotates the towards.转向目标方向(立即)
		/// </summary>
		/// <param name='dir'>
		/// Dir.
		/// </param>
		public static void RotateTowards (Transform trsf, Vector3 from, Vector3 to)
		{
			RotateTowards (trsf, to - from);
		}

		public static void RotateTowards (Transform trsf, Vector3 dir)
		{
			if (dir.magnitude < 0.001f) {
				return;
			}
			Quaternion rot = trsf.rotation;
			Quaternion toTarget = Quaternion.LookRotation (dir);
			trsf.rotation = toTarget;
		}

		/// <summary>
		/// Rotates the towards.转向目标方向(有转向过程)
		/// </summary>
		/// <param name='dir'>
		/// Dir.
		/// </param>
		public static void RotateTowards (Transform transform, Vector3 dir, float turningSpeed)
		{
			try {
				Quaternion rot = transform.rotation;
				if (dir.magnitude < 0.001f) {
					return;
				}
				Quaternion toTarget = Quaternion.LookRotation (dir);

				rot = Quaternion.Slerp (rot, toTarget, turningSpeed * Time.fixedDeltaTime);
				Vector3 euler = rot.eulerAngles;
				//euler.z = 0;
				//euler.x = 0;
				rot = Quaternion.Euler (euler);

				transform.rotation = rot;
			} catch (System.Exception e) {
				Debug.Log ("name==" + transform.name + "   " + e);
			}
		}

		public static Vector3 getAngle (Transform tr, Vector3 pos2)
		{
			return getAngle (tr.position, pos2);
		}

		public static Vector3 getAngle (Vector3 pos1, Vector3 pos2)
		{
			Vector3 dir = pos2 - pos1;
			return getAngle (dir);
		}

		public static Vector3 getAngle (Vector3 dir)
		{
			if (dir.magnitude < 0.001f) {
				return Vector3.zero;
			}
			Quaternion toTarget = Quaternion.LookRotation (dir);

			Vector3 euler = toTarget.eulerAngles;
			return euler;
//        return Quaternion.Euler(euler).eulerAngles;
		}

		/// <summary>
		/// Sets the body mat edit.重新设置一次shader，在editor模式下加载assetsbundle才需要调用这个方法
		/// </summary>
		/// <param name='tr'>
		/// Tr.
		/// </param>
		public static void setBodyMatEdit (Transform tr)
		{
			setBodyMatEdit (tr, null);
		}

		public static void setBodyMatEdit (Transform tr, Shader defaultShader)
		{
			if (tr == null) {
				return;
			}
			string shName = "";
			if (tr.GetComponent<Renderer> () != null && tr.GetComponent<Renderer> ().sharedMaterial != null) {
				shName = tr.GetComponent<Renderer> ().sharedMaterial.shader.name;
				if (defaultShader != null) {
					tr.GetComponent<Renderer> ().sharedMaterial.shader = defaultShader;
				} else {
					tr.GetComponent<Renderer> ().sharedMaterial.shader = Shader.Find (shName);
				}
			}
			SkinnedMeshRenderer smr = tr.GetComponent<SkinnedMeshRenderer> ();
			if (smr != null) {
				shName = smr.sharedMaterial.shader.name;
			
				if (defaultShader != null) {
					smr.sharedMaterial.shader = defaultShader;
				} else {
					smr.sharedMaterial.shader = Shader.Find (shName);
				}
			}
			MeshRenderer mr = tr.GetComponent<MeshRenderer> ();
			if (mr != null) {
				shName = mr.sharedMaterial.shader.name;
				if (defaultShader != null) {
					mr.sharedMaterial.shader = defaultShader;
				} else {
					mr.sharedMaterial.shader = Shader.Find (shName);
				}
				foreach (Material m in mr.sharedMaterials) {
					shName = m.shader.name;
					if (defaultShader != null) {
						m.shader = defaultShader;
					} else {
						m.shader = Shader.Find (shName);
					}
				}
			}
			TrailRenderer tailRender = tr.GetComponent<TrailRenderer> ();
			if (tailRender != null) {
				shName = tailRender.sharedMaterial.shader.name;
				if (defaultShader != null) {
					tailRender.sharedMaterial.shader = defaultShader;
				} else {
					tailRender.sharedMaterial.shader = Shader.Find (shName);
				}
			}
			for (int i = 0; i < tr.childCount; i++) {
				setBodyMatEdit (tr.GetChild (i));
			}
		}

		public static float distance (Transform tr1, Transform tr2)
		{
			return Vector3.Distance (tr1.position, tr2.position);
		}

		public static float distance4Loc (Transform tr1, Transform tr2)
		{
			return Vector3.Distance (tr1.localPosition, tr2.localPosition);
		}

		public static float distance (Vector2 v1, Vector2 v2)
		{
			return Vector2.Distance (v1, v2);
		}

		public static float distance (Vector3 v1, Vector3 v2)
		{
			return Vector3.Distance (v1, v2);
		}

		public static string LuaTableToString (LuaTable map)
		{
			if (map == null)
				return "map is null";
			StringBuilder outstr = new StringBuilder ();
			LuaTableToString (map, outstr);
			return outstr.ToString ();
		}

		public static void LuaTableToString (LuaTable map, StringBuilder outstr, int spacecount = 0)
		{
//			IEnumerable<object> list = map.GetKeys<object> ();
//			IEnumerator<object> e = list.GetEnumerator ();
			StringBuilder space = new StringBuilder ();
			for (int i = 0; i < spacecount; i++) {
				space.Append (" ");
			}
			outstr.Append ("\n" + space.ToString ()).Append ("{");
			foreach (object key in map.GetKeys<object>()) {
//				object key = cell.Key;
				object val = map.Get<object> (key);
				if (val == null) {
					continue;
				}
				outstr.Append (space.ToString ()).Append (key).Append ("(").Append (key.GetType ().ToString ()).Append (")").Append ("=");
				if (val is Hashtable) {
					MapToString ((Hashtable)val, outstr, spacecount++);
				} else if (val is ArrayList) {
					ArrayListToString ((ArrayList)val, outstr, spacecount++);
				} else if (val is LuaTable) {
					LuaTableToString ((LuaTable)val, outstr, spacecount++);
				} else if (val is byte[]) {
					outstr.Append (NumEx.bio2Int ((byte[])val)).Append ("(").Append (val.GetType ().ToString ()).Append (")").Append ("\n");
				} else {
					outstr.Append (val).Append ("(").Append (val.GetType ().ToString ()).Append (")").Append ("\n");
				}
			}
			outstr.Append ("}\n");
			//Debug.Log(outstr.ToString());
		}

		public static string MapToString (Hashtable map)
		{
			if (map == null)
				return "map is null";
			StringBuilder outstr = new StringBuilder ();
			MapToString (map, outstr);
			return outstr.ToString ();
		}

		public static void MapToString (Hashtable map, StringBuilder outstr, int spacecount = 0)
		{
//			ICollection keslist = map.Keys;
//			IEnumerator e = keslist.GetEnumerator();
			StringBuilder space = new StringBuilder ();
			for (int i = 0; i < spacecount; i++) {
				space.Append (" ");
			}
			outstr.Append ("\n" + space.ToString ()).Append ("{");
			foreach (DictionaryEntry cell in map) {
				object key = cell.Key;
				object val = cell.Value;
				if (val == null) {
					continue;
				}
				outstr.Append (space.ToString ()).Append (key).Append ("(").Append (key.GetType ().ToString ()).Append (")").Append ("=");
				if (val is Hashtable) {
					MapToString ((Hashtable)val, outstr, spacecount++);
				} else if (val is ArrayList) {
					ArrayListToString ((ArrayList)val, outstr, spacecount++);
				} else if (val is LuaTable) {
					LuaTableToString ((LuaTable)val, outstr, spacecount++);
				} else if (val is byte[]) {
					outstr.Append (NumEx.bio2Int ((byte[])val)).Append ("(").Append (val.GetType ().ToString ()).Append (")").Append ("\n");
				} else {
					outstr.Append (val).Append ("(").Append (val.GetType ().ToString ()).Append (")").Append ("\n");
				}
			}
			outstr.Append ("}\n");
			//Debug.Log(outstr.ToString());
		}

		public static string ArrayListToString2 (ArrayList list)
		{
			StringBuilder outstr = new StringBuilder ();
			ArrayListToString (list, outstr);
			return outstr.ToString ();
		}

		public static void ArrayListToString (ArrayList list, StringBuilder outstr, int spacecount = 0)
		{
			StringBuilder space = new StringBuilder ();
			for (int i = 0; i < spacecount; i++) {
				space.Append (" ");
			}
			outstr.Append ("\n" + space.ToString ()).Append ("[");
			foreach (object item in list) {
				if (item == null) {
					continue;
				}
				if (item is Hashtable) {
					MapToString ((Hashtable)item, outstr, spacecount++);
				} else if (item is ArrayList) {
					ArrayListToString ((ArrayList)item, outstr, spacecount++);
				} else if (item is byte[]) {
					outstr.Append (NumEx.bio2Int ((byte[])item)).Append (",");
				} else {
					outstr.Append (item).Append (",");
				}
			}
			outstr.Append ("]\n");

			//Debug.Log(outstr.ToString());
		}


		/// <summary>
		/// Draws the grid. 画网格
		/// </summary>
		/// <returns>
		/// The grid.
		/// </returns>
		/// <param name='origin'>
		/// Origin.
		/// </param>
		/// <param name='numRows'>
		/// Number rows.
		/// </param>
		/// <param name='numCols'>
		/// Number cols.
		/// </param>
		/// <param name='cellSize'>
		/// Cell size.
		/// </param>
		/// <param name='color'>
		/// Color.
		/// </param>
		public static ArrayList drawGrid (LineRenderer prefab, Vector3 origin, int numRows, int numCols, float cellSize, Color color, Transform gridRoot, float h)
		{
			ArrayList list = new ArrayList ();
#if UNITY_EDITOR
			if ((Application.platform == RuntimePlatform.OSXEditor ||
			    Application.platform == RuntimePlatform.WindowsEditor) &&
			    !Application.isPlaying) {
				return list;
			}
#endif

			float width = (numCols * cellSize);
			float height = (numRows * cellSize);

			// Draw the horizontal grid lines
			for (int i = 0; i < numRows + 1; i++) {
				Vector3 startPos = origin + i * cellSize * kZAxis + Vector3.up * h;
				Vector3 endPos = startPos + width * kXAxis + Vector3.up * h;
                LineRenderer lr = drawLine (prefab, startPos, endPos, color);
				list.Add (lr);
				lr.transform.parent = gridRoot;
			}

			// Draw the vertial grid lines
			for (int i = 0; i < numCols + 1; i++) {
				Vector3 startPos = origin + i * cellSize * kXAxis + Vector3.up * h;
				Vector3 endPos = startPos + height * kZAxis + Vector3.up * h;
                LineRenderer lr = drawLine (prefab, startPos, endPos, color);
				list.Add (lr);
				lr.transform.parent = gridRoot;
			}
			return list;
		}

		/// <summary>
		/// Draws the line.//画直线
		/// </summary>
		/// <returns>
		/// The line.
		/// </returns>
		/// <param name='startPos'>
		/// Start position.
		/// </param>
		/// <param name='endPos'>
		/// End position.
		/// </param>
		/// <param name='color'>
		/// Color.
		/// </param>
		public static LineRenderer drawLine (LineRenderer prefab, Vector3 startPos, Vector3 endPos, Color color)
		{
			LineRenderer line = Object.Instantiate (prefab) as LineRenderer;
			line.SetColors (color, color);
			line.SetPosition (0, startPos);
			line.SetPosition (1, endPos);
			line.gameObject.SetActive (true);
			return line;
		}

		/// <summary>
		/// Clones the res. 实例化Resoureces下的资源
		/// </summary>
		/// <returns>
		/// The res.
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		public static GameObject cloneRes (string path)
		{
			try {
				return Object.Instantiate (Resources.Load (path)) as GameObject;
			} catch (System.Exception e) {
				Debug.Log (e + "path==" + path);
				return null;
			}
		}

		public static GameObject cloneRes (GameObject prefab)
		{
			try {
				return Object.Instantiate (prefab) as GameObject;
			} catch (System.Exception e) {
				Debug.Log (e);
				return null;
			}
		}

		/// <summary>
		/// Loads the res.跟路径加载资源
		/// </summary>
		/// <returns>
		/// The res.
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		public static object loadRes (string path)
		{
			try {
				return Resources.Load (path, typeof(object)) as object;
			} catch (System.Exception e) {
				Debug.Log (e);
				return null;
			}
		}

		/// <summary>
		/// Loads the gobj.从指定路径加载gameObject
		/// </summary>
		/// <returns>
		/// The gobj.
		/// </returns>
		/// <param name='path'>
		/// Path.
		/// </param>
		public static GameObject loadGobj (string path)
		{
			try {
				return Resources.Load (path, typeof(GameObject)) as GameObject;
			} catch (System.Exception e) {
				Debug.Log (e);
				return null;
			}
		}

		public static Vector2 addVector2 (Vector2 v1, Vector2 v2)
		{
			return v1 + v2;
		}

		public static Vector3 addVector3 (Vector3 v1, Vector3 v2)
		{
			return v1 + v2;
		}

		public static Vector2 cutVector2 (Vector2 v1, Vector2 v2)
		{
			return v1 - v2;
		}

		public static Vector3 cutVector3 (Vector3 v1, Vector3 v2)
		{
			return v1 - v2;
		}

		public static Transform getChild (Transform root, params object[] args)
		{
			Transform tr = root;
			if (root == null || args == null)
				return null;
			int count = args.Length;
			int i = 0;
			while (true) {
				if (i >= count)
					break;
				if (tr == null) {
					Debug.LogError (args [i]);
					break;
				}
				tr = tr.Find (args [i].ToString ());
				i = i + 1;
			}
			return tr;
		}

		static string SDCardPath = "";

		public static string getSDCard ()
		{
			string sd = "";
			#if UNITY_ANDROID && !UNITY_EDITOR
			if (string.IsNullOrEmpty(SDCardPath)) {
				AndroidJavaClass ajc = new AndroidJavaClass("com.coolape.u3dPlugin.FilePathUtl");
				SDCardPath = ajc.CallStatic<string>("getSDKPath");
			}
			#endif
			sd = SDCardPath;
			return sd;
		}

		public static string chgToSDCard (string path)
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
			if (Directory.Exists("/sdcard/")) {
				path = path.Replace(CLPathCfg.persistentDataPath + "/", "/sdcard/");
			}
			#endif
			return path;
		}

		///   <summary>
		///   给一个字符串进行MD5加密
		///   </summary>
		///   <param   name="strText">待加密字符串</param>
		///   <returns>加密后的字符串</returns>
		public static string MD5Encrypt (string strText)
		{   
			byte[] bytes = Encoding.UTF8.GetBytes (strText);    //tbPass为输入密码的文本框
			return MD5Encrypt (bytes);
		}

		public static byte[] getUtf8bytes(string str) {
			return Encoding.UTF8.GetBytes (str);    //tbPass为输入密码的文本框
		}

		public static string MD5Encrypt (byte[] bytes)
		{
			MD5 md5 = new MD5CryptoServiceProvider ();
			byte[] output = md5.ComputeHash (bytes);
			return System.BitConverter.ToString (output).Replace ("-", "").ToLower ();  //tbMd5pass为输出加密文本
		}

		public static bool netIsActived ()
		{
			bool ret = true;
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.coolape.u3dPlugin.NetUtl");
        ret =  jc.CallStatic<bool>("isConnectNet");
#endif
			Debug.Log ("Net state =" + Application.internetReachability);
			if (Application.internetReachability == NetworkReachability.NotReachable) {
				ret = false;
			}
			return ret;
		}

		/// <summary>
		/// Gets the state of the net.
		/// </summary>
		/// <returns>The net state.
		///     None 无网络
		///     WiFi
		///     2G
		///     3G
		///     4G
		///     Unknown
		/// </returns>
		public static string getNetState ()
		{
			string ret = "Unkown";
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass jc = new AndroidJavaClass("com.coolape.u3dPlugin.NetUtl");
        ret =  jc.CallStatic<string>("getCurrentNetworkType");
			#endif
			return ret;
		}

		public static string urlAddTimes (string url)
		{
			if (url.StartsWith ("http://")) {
				if (url.Contains ("?")) {
					url = PStr.b ().a (url).a ("&t_sign_flag___=").a (DateEx.nowMS).e ();
				} else {
					url = PStr.b ().a (url).a ("?t_sign_flag___=").a (DateEx.nowMS).e ();
				}
#if CHL_NONE
				Debug.LogWarning (url);
#endif
			}
			return url;
		}

		/// <summary>
		/// Gets the sing in code android.取得签名值
		/// </summary>
		/// <returns>The sing in code android.</returns>
		public static  string getSingInCodeAndroid ()
		{
			try {
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
				AndroidJavaClass jcPackageManager = new AndroidJavaClass ("android.content.pm.PackageManager");
				AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
				AndroidJavaObject joPackageManager = jo.Call<AndroidJavaObject> ("getPackageManager");
				string joPackageName = jo.Call<string> ("getPackageName");
				int GET_SIGNATURES = jcPackageManager.GetStatic<int> ("GET_SIGNATURES");
				AndroidJavaObject packageInfo = joPackageManager.Call<AndroidJavaObject> ("getPackageInfo", joPackageName, GET_SIGNATURES);
				AndroidJavaObject[] signs = packageInfo.Get<AndroidJavaObject[]> ("signatures");
				if (signs.Length > 0) {
					AndroidJavaObject sign = signs [0];
					string signStr = sign.Call<string> ("toCharsString");
					if (!string.IsNullOrEmpty (signStr)) {
						return MD5Encrypt (signStr);
					}
				}
//        PackageInfo packageInfo = _self.getPackageManager().getPackageInfo(_self.getPackageName(), PackageManager.GET_SIGNATURES);
//        Signature[] signs = packageInfo.signatures;
//        Signature sign = signs[0];
//        Log.d("CommonTool","sign:    " + sign);
//        return sign.hashCode();

#endif
			} catch (System.Exception e) {
				Debug.LogError (e);
			}
			return "";
		}

		public static LayerMask getLayer (string layerName)
		{
			string[] list = layerName.Split (',');
			LayerMask ret = 0;
			for (int i = 0; i < list.Length; i++) {
				ret |= (1 << LayerMask.NameToLayer (list [i]));
			}
			return ret;
		}

        static RaycastHit hitInfor = new RaycastHit();
        public static RaycastHit getRaycastHitInfor (Camera camera, Vector3 inPos, LayerMask layer)
		{
            if (camera == null)
				return hitInfor;
			Ray ray = camera.ScreenPointToRay (inPos);
			if (Physics.Raycast (ray, out hitInfor, 1000, layer.value)) {
				return hitInfor;
			} else {
				return hitInfor;
			}
		}

        static object[] _doCallback (object callback, params object[] args)
		{
			try {
				if (callback == null)
					return null;
				object[] ret = null;
				if (callback is LuaFunction) {
					ret = ((LuaFunction)callback).Call (args);
				} else if (callback is Callback) {
					((Callback)callback) (args);
                }
				return ret;
			} catch (System.Exception e) {
				Debug.LogError (e);
				return null;
			}
		}

        static void parseLuafunc(LuaTable luatable, out LuaFunction func, out LuaTable instance) {
            if (luatable != null)
            {
                func = luatable.GetInPath<LuaFunction>("func");
                instance = luatable.GetInPath<LuaTable>("instance");
            } else {
                func = null;
                instance = null;
            }
        }
        public static object[] doCallback(object callback)
        {
            try
            {
                if (callback == null)
                    return null;
                object[] ret = null;
                if (callback is LuaTable)
                {
                    LuaFunction func = null;
                    LuaTable instance = null;
                    parseLuafunc(callback as LuaTable, out func, out instance);
                    ret = _doCallback(func, instance);
                }
                else
                {
                    ret = _doCallback(callback);
                }
                return ret;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }
        public static object[] doCallback(object callback, object paras1)
        {
            try
            {
                if (callback == null)
                    return null;
                object[] ret = null;
                if (callback is LuaTable)
                {
                    LuaFunction func = null;
                    LuaTable instance = null;
                    parseLuafunc(callback as LuaTable, out func, out instance);
                    ret = _doCallback(func, instance, paras1);
                }
                else
                {
                    ret = _doCallback(callback, paras1);
                }
                return ret;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public static object[] doCallback(object callback, object paras1, object paras2)
        {
            try
            {
                if (callback == null)
                    return null;
                object[] ret = null;
                if (callback is LuaTable)
                {
                    LuaFunction func = null;
                    LuaTable instance = null;
                    parseLuafunc(callback as LuaTable, out func, out instance);
                    ret = _doCallback(func, instance, paras1, paras2);
                }
                else
                {
                    ret = _doCallback(callback, paras1, paras2);
                }
                return ret;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public static object[] doCallback(object callback, object paras1, object paras2, object paras3)
        {
            try
            {
                if (callback == null)
                    return null;
                object[] ret = null;
                if (callback is LuaTable)
                {
                    LuaFunction func = null;
                    LuaTable instance = null;
                    parseLuafunc(callback as LuaTable, out func, out instance);
                    ret = _doCallback(func, instance, paras1, paras2, paras3);
                }
                else
                {
                    ret = _doCallback(callback, paras1, paras2, paras3);
                }
                return ret;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public static object[] doCallback(object callback, object paras1, object paras2, object paras3, object paras4)
        {
            try
            {
                if (callback == null)
                    return null;
                object[] ret = null;
                if (callback is LuaTable)
                {
                    LuaFunction func = null;
                    LuaTable instance = null;
                    parseLuafunc(callback as LuaTable, out func, out instance);
                    ret = _doCallback(func, instance, paras1, paras2, paras3, paras4);
                }
                else
                {
                    ret = _doCallback(callback, paras1, paras2, paras3, paras4);
                }
                return ret;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        public static object[] doCallback(object callback, object paras1, object paras2, object paras3, object paras4, object paras5)
        {
            try
            {
                if (callback == null)
                    return null;
                object[] ret = null;
                if (callback is LuaTable)
                {
                    LuaFunction func = null;
                    LuaTable instance = null;
                    parseLuafunc(callback as LuaTable, out func, out instance);
                    ret = _doCallback(func, instance, paras1, paras2, paras3, paras4, paras5);
                }
                else
                {
                    ret = _doCallback(callback, paras1, paras2, paras3, paras4, paras5);
                }
                return ret;
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// Files to map.取得文件转成map
        /// </summary>
        /// <returns>
        /// The to map.
        /// </returns>
        /// <param name='path'>
        /// Path.
        /// </param>
        public static Hashtable fileToMap (string path)
		{
			if (!File.Exists (path)) {
				return null;
			}
			byte[] buffer = File.ReadAllBytes (path);
			if (buffer != null) {
				MemoryStream ms = new MemoryStream ();
				ms.Write (buffer, 0, buffer.Length);
				ms.Position = 0;
				object obj = B2InputStream.readObject (ms);
				if (obj != null) {
					return (Hashtable)(obj);
				}
			}
			return null;
		}

		public static object fileToObj (string path)
		{
			if (!File.Exists (path)) {
				return null;
			}
			byte[] buffer = File.ReadAllBytes (path);
			if (buffer != null) {
				MemoryStream ms = new MemoryStream ();
				ms.Write (buffer, 0, buffer.Length);
				ms.Position = 0;
				object obj = B2InputStream.readObject (ms);
				if (obj != null) {
					return obj;
				}
			}
			return null;
		}

		public static void printe (object msg)
		{
			Debug.LogError (msg);
		}

		public static void printw (object msg)
		{
			Debug.LogWarning (msg);
		}

		public static bool IntersectRay (Bounds bounds, Ray ray)
		{
			if (bounds.Contains (ray.origin))
				return true;
			return bounds.IntersectRay (ray);
		}

		public static bool IntersectRay (Bounds bounds, Ray ray, float minDis, float maxDis)
		{
			if (bounds.Contains (ray.origin))
				return true;
			float dis = 0;
			bool ret = bounds.IntersectRay (ray, out dis);
			if (ret && dis >= minDis && dis <= maxDis) {
				ret = true;
			} else {
				ret = false;
			}
			return ret;
		}


		public class GameObjcetPool:AbstractObjectPool<GameObject>
		{
			public override GameObject createObject (string key)
			{
				return new GameObject ();
			}

			public override GameObject resetObject (GameObject t)
			{
				t.SetActive (false);
				return t;
			}
		}

		static GameObjcetPool goPool = new GameObjcetPool ();

		public static Vector3 RotateAround (Vector3 currPoint, Vector3 point, Vector3 axis, float angle)
		{
			GameObject go = goPool.borrowObject ();
			go.transform.position = currPoint;
			go.transform.RotateAround (point, axis, angle);

			Vector3 v3 = go.transform.position;
			goPool.returnObject (go);
			return v3;
		}

        [LuaCallCSharp]
        [ReflectionUse]
        public static byte[] read4MemoryStream (MemoryStream ms, int offset, int len)
		{
			if (ms == null || len <= 0) {
				return null;
			}

			byte[] ret = new byte[len];
			ms.Read (ret, offset, len);
			return ret;
		}
	}

	/*
	 * 有的Unity对象，在C#为null，在lua为啥不为nil呢？比如一个已经Destroy的GameObject
	 * 其实那C#对象并不为null，是UnityEngine.Object重载的==操作符，
	 * 当一个对象被Destroy，未初始化等情况，obj == null返回true，
	 * 但这C#对象并不为null，可以通过System.Object.ReferenceEquals(null, obj)来验证下。
	 * 对应这种情况，可以为UnityEngine.Object写一个扩展方法：
	 */
	[LuaCallCSharp]
	[ReflectionUse]
	public static class UnityEngineObjectExtention
	{
		public static bool IsNull(this UnityEngine.Object o) // 或者名字叫IsDestroyed等等
		{
			return o == null;
		}
	}
}

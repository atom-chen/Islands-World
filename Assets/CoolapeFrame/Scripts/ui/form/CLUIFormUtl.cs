using UnityEngine;
using System.Collections;
//using System;
using System.Collections.Generic;
using Coolape;
using System.IO;
//using System.Net;
using System.Text;

namespace Coolape
{
	public class CLUIFormUtl
	{
		/// <summary>
		/// 
		/*
	拨打电话的话，需要一个权限，就是android.permission.CALL_PHONE.
	所以，首先在AndroidMenifest文件里加上这个权限：
	<uses-permission android:name="android.permission.CALL_PHONE" />
		
	第一种方式：
	Intent intent = new Intent(Intent.ACTION_DIAL);
	Uri data = Uri.parse("tel:" + "135xxxxxxxx");
	intent.setData(data);
	startActivity(intent);
	这种方式的特点就是，去到了拨号界面，但是实际的拨号是由用户点击实现的。
	3
	第二种方式：
	Intent intent = new Intent(Intent.ACTION_CALL);
	Uri data = Uri.parse("tel:" + "135xxxxxxxx");
	intent.setData(data);
	startActivity(intent);
	这种方式的特点就是，直接拨打了你所输入的号码，所以这种方式对于用户没有直接的提示效果，Android推荐使用第一种方式，如果是第二种的话，建议在之前加一个提示，是否拨打号码，然后确定后再拨打。
	*/
		/*
		/// </summary>
		/// <param name="phoneNum">Phone number.</param>
		public static void callPhone (string phoneNum)
		{
			if (string.IsNullOrEmpty (phoneNum)) {
				Debug.LogWarning ("phoneNum is null");
				return;
			}
			#if UNITY_ANDROID && !UNITY_EDITOR
		try {
			AndroidJavaClass Intent = new AndroidJavaClass("android.content.Intent");
			string actionCall = Intent.GetStatic<string>("ACTION_CALL");
			AndroidJavaClass Uri = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject data = Uri.CallStatic<AndroidJavaObject>("parse", "tel:" + phoneNum);
			AndroidJavaObject intentObj = new AndroidJavaObject("android.content.Intent", actionCall, data);
			AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			unityActivity.Call("startActivity", intentObj);
		} catch (System.Exception e) {
			Debug.LogError(e);
		}
			#endif
		}

		/// <summary>
		/// Gets the phone number. 取得本机号码
		/// </summary>
		public static string getPhoneNumber ()
		{
			#if UNITY_ANDROID && !UNITY_EDITOR
		AndroidJavaClass Context = new AndroidJavaClass("android.content.Context");
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject tm = unityActivity.Call<AndroidJavaObject> ("getSystemService", Context.GetStatic<string> ("TELEPHONY_SERVICE"));
			
//		String deviceid = tm.getDeviceId();
//		String tel = tm.getLine1Number();
//		String  imei = tm.getSimSerialNumber();      
//		String imsi = tm.getSubscriberId();
		return tm.Call<string>("getLine1Number");
			#else
			return "";
			#endif
		}

		*/
		/// <summary>
		/// Identities the code valid.身份证号验证
		/// </summary>
		/// <returns>The code valid.</returns>
		/// <param name="code">Code.</param>
		public static string IdentityCodeValid (string code)
		{ 
			Dictionary<string, string> city = new Dictionary<string, string> ();
			city ["11"] = "北京";
			city ["12"] = "天津";
			city ["13"] = "河北";
			city ["14"] = "山西";
			city ["15"] = "内蒙古";
			city ["21"] = "辽宁";
			city ["22"] = "吉林";
			city ["23"] = "黑龙江 ";
			city ["31"] = "上海";
			city ["32"] = "江苏";
			city ["33"] = "浙江";
			city ["34"] = "安徽";
			city ["35"] = "福建";
			city ["36"] = "江西";
			city ["37"] = "山东";
			city ["41"] = "河南";
			city ["42"] = "湖北 ";
			city ["43"] = "湖南";
			city ["44"] = "广东";
			city ["45"] = "广西";
			city ["46"] = "海南";
			city ["50"] = "重庆";
			city ["51"] = "四川";
			city ["52"] = "贵州";
			city ["53"] = "云南";
			city ["54"] = "西藏 ";
			city ["61"] = "陕西";
			city ["62"] = "甘肃";
			city ["63"] = "青海";
			city ["64"] = "宁夏";
			city ["65"] = "新疆";
			city ["71"] = "台湾";
			city ["81"] = "香港";
			city ["82"] = "澳门";
			city ["91"] = "国外";

			string tip = "";
			bool pass = true;

			bool isMatch = System.Text.RegularExpressions.Regex.IsMatch (code, "^\\d{6}(18|19|20)?\\d{2}(0[1-9]|1[12])(0[1-9]|[12]\\d|3[01])\\d{3}(\\d|X)$");
			//		if(!code || !/^\d{6}(18|19|20)?\d{2}(0[1-9]|1[12])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/i.test(code)){
			if (!isMatch) {
				tip = "身份证号格式错误\n";
				pass = false;
			} else if (city [code.Substring (0, 2)] == null) {
				tip = "身份证号地址编码错误\n";
				pass = false;
			} else {
				//18位身份证需要验证最后一位校验位
				if (code.Length == 18) {
					//				code = code.Split('');
					//∑(ai×Wi)(mod 11)
					//加权因子
					int[] factor = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
					//校验位
					int[] parity = { 1, 0, 'X', 9, 8, 7, 6, 5, 4, 3, 2 };
					int sum = 0;
					int ai = 0;
					int wi = 0;
					for (var i = 0; i < 17; i++) {
						ai = int.Parse (code [i].ToString ());
						wi = factor [i];
						sum += ai * wi;
					}

					int last = parity [sum % 11];
					if (last != int.Parse (code [17].ToString ())) {
						tip = "身份证号校验位错误\n";
						pass = false;
					}
				}
			}

			return tip;
		}

		/// <summary>
		/// Shows the calender.日期选择页面
		/// </summary>
		/// <param name="paras">Paras.</param>
		public static void showCalender (params object[] paras)
		{
			if (paras != null && paras.Length >= 4) {
				ArrayList list = new ArrayList ();
				list.Add (paras [0]);//year);
				list.Add (paras [1]);//month);
				list.Add (paras [2]);//callback);
				list.Add (paras [3]);//isSetTime);

				CLPanelManager.getPanelAsy ("PanelCalender", (Callback)onGetCalenderPanel, list);
			} else {
				showCalender (System.DateTime.Now.Year, System.DateTime.Now.Month, paras [0], paras [1]);
			}
		}

		public static void onGetCalenderPanel (params object[] orgs)
		{
			CLPanelBase p = orgs [0] as CLPanelBase;
			ArrayList list = orgs [1] as ArrayList;
			if (p == null) {
				return;
			}
			p.setData (list);
			CLPanelManager.showPanel (p);
		}

		public static string GetChineseSpell (string strText)
		{ 
			int len = strText.Length; 
			string myStr = ""; 
			for (int i = 0; i < len; i++) { 
				myStr += getSpell (strText.Substring (i, 1)); 
			} 
			return myStr; 
		}


		/// <summary>
		/// 得到一个汉字的拼音第一个字母，如果是一个英文字母则直接返回大写字母
		/// </summary>
		/// <param name="CnChar">单个汉字</param>
		/// <returns>单个大写字母</returns>
		public static string GetCharSpellCode (string CnChar)
		{
			long iCnChar;
			byte[] ZW = System.Text.Encoding.Default.GetBytes (CnChar);
			//如果是字母，则直接返回
			if (ZW.Length == 1) {
				return CnChar.ToUpper ();
			} else {
				// get the array of byte from the single char
				int i1 = (short)(ZW [0]);
				int i2 = (short)(ZW [1]);
				iCnChar = i1 * 256 + i2;
			}
			// iCnChar match the constant
			if ((iCnChar >= 45217) && (iCnChar <= 45252)) {
				return "A";
			} else if ((iCnChar >= 45253) && (iCnChar <= 45760)) {
				return "B";
			} else if ((iCnChar >= 45761) && (iCnChar <= 46317)) {
				return "C";
			} else if ((iCnChar >= 46318) && (iCnChar <= 46825)) {
				return "D";
			} else if ((iCnChar >= 46826) && (iCnChar <= 47009)) {
				return "E";
			} else if ((iCnChar >= 47010) && (iCnChar <= 47296)) {
				return "F";
			} else if ((iCnChar >= 47297) && (iCnChar <= 47613)) {
				return "G";
			} else if ((iCnChar >= 47614) && (iCnChar <= 48118)) {
				return "H";
			} else if ((iCnChar >= 48119) && (iCnChar <= 49061)) {
				return "J";
			} else if ((iCnChar >= 49062) && (iCnChar <= 49323)) {
				return "K";
			} else if ((iCnChar >= 49324) && (iCnChar <= 49895)) {
				return "L";
			} else if ((iCnChar >= 49896) && (iCnChar <= 50370)) {
				return "M";
			} else if ((iCnChar >= 50371) && (iCnChar <= 50613)) {
				return "N";
			} else if ((iCnChar >= 50614) && (iCnChar <= 50621)) {
				return "O";
			} else if ((iCnChar >= 50622) && (iCnChar <= 50905)) {
				return "P";
			} else if ((iCnChar >= 50906) && (iCnChar <= 51386)) {
				return "Q";
			} else if ((iCnChar >= 51387) && (iCnChar <= 51445)) {
				return "R";
			} else if ((iCnChar >= 51446) && (iCnChar <= 52217)) {
				return "S";
			} else if ((iCnChar >= 52218) && (iCnChar <= 52697)) {
				return "T";
			} else if ((iCnChar >= 52698) && (iCnChar <= 52979)) {
				return "W";
			} else if ((iCnChar >= 52980) && (iCnChar <= 53640)) {
				return "X";
			} else if ((iCnChar >= 53689) && (iCnChar <= 54480)) {
				return "Y";
			} else if ((iCnChar >= 54481) && (iCnChar <= 55289)) {
				return "Z";
			} else {
				return ("?");
			}
		}

		public static string getSpell (string cnChar)
		{ 
			byte[] arrCN = Encoding.Unicode.GetBytes (cnChar); 
			if (arrCN.Length > 1) { 
				int area = (short)arrCN [0]; 
				int pos = (short)arrCN [1]; 
				int code = (area << 8) + pos; 
				int[] areacode = {45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622,

					50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481
				};
				for (int i = 0; i < 26; i++) { 
					int max = 55290; 
					if (i != 25)
						max = areacode [i + 1]; 
					if (areacode [i] <= code && code < max) { 
						return Encoding.Default.GetString (new byte[]{ (byte)(65 + i) }); 
					} 
				} 
				return "*"; 
			} else
				return cnChar; 
		}
		#if USE_LOCATION_SERVER
		public static void StopGPS ()
		{
			Input.location.Stop ();
		}

		public static void getGPSLocation (object callback)
		{
			getGPSLocation (500.0f, callback);
		}

		public static void getGPSLocation (float desired, object callback)
		{
			CLMainBase.self.StartCoroutine (StartGPS (desired, callback));
		}

		/// <summary>
		/// Starts the GP.
		/// </summary>
		/// <returns>The GP.</returns>
		/// <param name="callback">Callback.</param>
		/// callback(errMsg, LocationInfo localInfor);
		/// LocationInfo
		/// 	属性如下：
		/// 		（1） altitude -- 海拔高度 
		/// 		（2） horizontalAccuracy -- 水平精度 
		/// 		（3） latitude -- 纬度 
		/// 		（4） longitude -- 经度 
		/// 		（5） timestamp -- 最近一次定位的时间戳，从1970开始 
		/// 		（6） verticalAccuracy -- 垂直精度 
		public static IEnumerator StartGPS (float desired, object callback)
		{
			// Input.location 用于访问设备的位置属性（手持设备）, 静态的LocationService位置
			// LocationService.isEnabledByUser 用户设置里的定位服务是否启用
			string errMsg = "";
			if (!Input.location.isEnabledByUser) {
				errMsg = "isEnabledByUser value is:" + Input.location.isEnabledByUser.ToString () + " Please turn on the GPS"; 
			} else {
				// LocationService.Start() 启动位置服务的更新,最后一个位置坐标会被使用
				/*void Start(float desiredAccuracyInMeters = 10f, float updateDistanceInMeters = 10f); 
		参数详解：
			desiredAccuracyInMeters  服务所需的精度，以米为单位。如果使用较高的值，比如500，那么通常不需要打开GPS芯片（比如可以利用信号基站进行三角定位），从而节省电池电量。像5-10这样的值，可以被用来获得最佳的精度。默认值是10米。
			updateDistanceInMeters  最小距离（以米为单位）的设备必须横向移动前Input.location属性被更新。较高的值，如500意味着更少的开销。默认值是10米。
		*/
				if (Input.location.status == LocationServiceStatus.Failed ||
				   Input.location.status == LocationServiceStatus.Stopped) {
					Input.location.Start (desired);

					int maxWait = 20;
					while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
						// 暂停协同程序的执行(1秒)
						yield return new WaitForSeconds (1);
						maxWait--;
					}

					if (maxWait < 1) {
						errMsg = "Init GPS service time out";
					} else if (Input.location.status == LocationServiceStatus.Failed) {
						errMsg = "Unable to determine device location";
					} else {
						errMsg = "";
						Debug.Log ("--------N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude);
//					yield return new WaitForSeconds(1);
					}
				}
				Debug.Log ("======N:" + Input.location.lastData.latitude + " E:" + Input.location.lastData.longitude);
//			yield return new WaitForSeconds(1);
			}

			Utl.doCallback (callback, errMsg, Input.location.lastData);
			//		StopGPS ();
		}
		#endif
	}
}
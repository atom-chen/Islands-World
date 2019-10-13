
/*

changed from HaYaShi ToShiTaKa

*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Text;
using XLua;
using System.Linq;

static public class XLuaIDEAapiMaker
{
	#region member
	private static string m_apiDir = Application.dataPath + "/../UnityAPI";

	private static Dictionary<string, int> m_nameCounter = new Dictionary<string, int> ();

	private static List<ConstructorInfo> m_constructs = new List<ConstructorInfo> ();
	private static List<MethodInfo> m_methods = new List<MethodInfo> ();
	private static List<PropertyInfo> m_propertys = new List<PropertyInfo> ();
	private static List<FieldInfo> m_fields = new List<FieldInfo> ();
	private static string m_argsText;

	public static bool IsMemberFilter (MemberInfo mi)
	{
		return false;
//		return ToLuaExport.memberFilter.Contains(mi.ReflectedType.Name + "." + mi.Name);
	}

	#endregion

	/// <summary>
	/// 获取类型的文件名
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	static string GetTypeFileName (Type type)
	{
		return type.ToString ().Replace ("+", ".").Replace (".", "").Replace ("`", "").Replace ("&", "").Replace ("[", "").Replace ("]", "").Replace (",", "");
	}

	//    /// <summary>
	//    /// 获取类型标签名
	//    /// </summary>
	//    /// <param name="type"></param>
	//    /// <returns></returns>
	static string GetTypeTagName (Type type)
	{
		return type.ToString ().Replace ("+", ".").Replace ("`", "").Replace ("&", "").Replace ("[", "").Replace ("]", "").Replace (",", "");
	}

	#region export api

	[MenuItem ("XLua/IDEA生成自动提示的文件")]
	public static void ExportLuaApi ()
	{
		List<Type> classList = new List<Type> ();

		IEnumerable<FieldInfo> LuaCallCSharps = (from type in XLua.Utils.GetAllTypes ()
			from method in type.GetFields (BindingFlags.Static | BindingFlags.Public)
			where method.IsDefined (typeof(LuaCallCSharpAttribute), false)
			select method);


		
//		object[] ccla = test.GetCustomAttributes(typeof(LuaCallCSharpAttribute), false);
//		if (ccla.Length == 1 && (((ccla[0] as LuaCallCSharpAttribute).Flag & GenFlag.GCOptimize) != 0))
//		{
//			AddToList(GCOptimizeList, get_cfg);
//		}
		foreach (var memeber in LuaCallCSharps) {
			object LuaCallCSharpList = memeber.GetValue (null);
			if (LuaCallCSharpList is IEnumerable<Type>) {
				classList.AddRange (LuaCallCSharpList as IEnumerable<Type>);
			}
		}

//		return;
//		classList = XluaGenCodeConfig.LuaCallCSharp;

//		Dictionary<Type, ToLuaMenu.BindType> s_apiTypeIdx = new Dictionary<Type, ToLuaMenu.BindType>();
//		//收集要生成的类
//		List<ToLuaMenu.BindType> btList = new List<ToLuaMenu.BindType>();
//		ToLuaExport.allTypes.Clear();
//		ToLuaExport.allTypes.AddRange(ToLuaMenu.baseType);
//		ToLuaExport.allTypes.AddRange(CustomSettings.staticClassTypes);
//		for (int i = 0; i < ToLuaExport.allTypes.Count; i++)
//		{
//			btList.Add(new ToLuaMenu.BindType(ToLuaExport.allTypes[i]));
//		}
//		foreach(var bt in CustomSettings.customTypeList)
//		{
//			if (ToLuaExport.allTypes.Contains(bt.type)) continue;
//			ToLuaExport.allTypes.Add(bt.type);
//			btList.Add(bt);
//		}
//		ToLuaMenu.BindType[] allTypes = GenBindTypes(btList.ToArray(), false);
//		foreach(var bt in allTypes)//做最后的检查，进一步排除一些类
//		{
//			if (bt.type.IsInterface && bt.type != typeof(System.Collections.IEnumerator))
//				continue;
//			s_apiTypeIdx[bt.type] = bt; 
//			if (classList.Contains (bt.type)) {
//				Debug.LogWarning ("type:"+bt.type.FullName+" has add");
//			} else {
//				
//				classList.Add (bt.type);
//			}
//		}

		//一些类需要手动加
//		{
//			ToLuaMenu.BindType bt = new ToLuaMenu.BindType(typeof(Array));
//			s_apiTypeIdx[bt.type] = bt;
//			GetClassApi("System.Collections.IEnumerable").AddMethod("GetEnumerator", "()", "System.Collections.IEnumerator", "System.Collections.IEnumerator");
//		}

		string unityAPI = "";
		// add class here
		BindingFlags bindType = BindingFlags.DeclaredOnly |
		                        BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public;
		MethodInfo[] methods;
		FieldInfo[] fields;
		PropertyInfo[] properties;
		List<ConstructorInfo> constructors;
		ParameterInfo[] paramInfos;
		int delta;
		string path = m_apiDir;
		if (path == "") {
			return;
		}
		if (Directory.Exists (path)) {
			Directory.Delete (path, true);
		}
		Directory.CreateDirectory (path);

		foreach (Type t in classList) {
			FileStream fs = new FileStream (path + "/" + t.FullName.Replace ('.', '_') + "Wrap.lua", FileMode.Create);
			var utf8WithoutBom = new System.Text.UTF8Encoding (false);
			StreamWriter sw = new StreamWriter (fs, utf8WithoutBom);

			if (t.BaseType != null && t.BaseType != typeof(object)) {
				sw.WriteLine (string.Format ("---@class {0} : {1}", t.FullName, t.BaseType.FullName));
			} else {
				sw.WriteLine (string.Format ("---@class {0}", t.FullName));
			}


			#region field
			fields = t.GetFields (bindType);
			foreach (var field in fields) {
				if (IsObsolete (field)) {
					continue;
				}
				WriteField (sw, GetTypeTagName (field.FieldType), field.Name);
			}

			properties = t.GetProperties (bindType);
			foreach (var property in properties) {
				if (IsObsolete (property)) {
					continue;
				}
				WriteField (sw, GetTypeTagName (property.PropertyType), property.Name);
			}

			//sw.Write ("\n");
			#endregion

			#region constructor
			constructors = new List<ConstructorInfo> (t.GetConstructors (bindType));
			constructors.Sort ((left, right) => {
				return left.GetParameters ().Length - right.GetParameters ().Length;
			});
			bool isDefineTable = false;
			if (constructors.Count > 0) {
				sw.WriteLine ("local m = { }");
				WriteCtorComment (sw, constructors);
				paramInfos = constructors [constructors.Count - 1].GetParameters ();
				delta = paramInfos.Length - constructors [0].GetParameters ().Length;
				if (constructors [0].IsPublic) {
					WriteFun (sw, paramInfos, delta, t, "New", true);
				}
				isDefineTable = true;
			}

			#endregion

			#region method
			methods = t.GetMethods (bindType);
			MethodInfo method;

			Dictionary<string, List<MethodInfo>> methodDict = new Dictionary<string, List<MethodInfo>> ();
			if (methods.Length != 0 && !isDefineTable) {
				sw.WriteLine ("local m = { }");
			}

			for (int i = 0; i < methods.Length; i++) {
				method = methods [i];
				string methodName = method.Name;
				if (IsObsolete (method)) {
					continue;
				}
				if (method.IsGenericMethod) {
					continue;
				}
				if (!method.IsPublic)
					continue;
				if (methodName.StartsWith ("get_") || methodName.StartsWith ("set_"))
					continue;

				List<MethodInfo> list;
				if (!methodDict.TryGetValue (methodName, out list)) {
					list = new List<MethodInfo> ();
					methodDict.Add (methodName, list);
				}
				list.Add (method);
			}

			var itr = methodDict.GetEnumerator ();
			while (itr.MoveNext ()) {
				List<MethodInfo> list = itr.Current.Value;
//				RemoveRewriteFunHasTypeAndString(list);
				list.Sort ((left, right) => {
					return left.GetParameters ().Length - right.GetParameters ().Length;
				});
				WriteFunComment (sw, list);
				paramInfos = list [list.Count - 1].GetParameters ();
				delta = paramInfos.Length - list [0].GetParameters ().Length;

				WriteFun (sw, paramInfos, delta, list [0].ReturnType, list [0].Name, list [0].IsStatic);
			}
			itr.Dispose ();

			if (methods.Length != 0 || isDefineTable) {
				sw.WriteLine (t.FullName + " = m");
				if (t.FullName.Contains (".")) {
					unityAPI += t.FullName.Replace ('.', '_') + " = " + t.FullName + "\r\n";
				}
				sw.WriteLine ("return m");
			}
			#endregion

			//清空缓冲区
			sw.Flush ();
			//关闭流
			sw.Close ();
			fs.Close ();
		}

		File.WriteAllText (path + "/unity_api.lua", unityAPI); 
		if (Directory.Exists (Application.dataPath + "/lua/")) {
			File.WriteAllText (Application.dataPath + "/lua/" + "/unity_api.lua", unityAPI);
		}

		Debug.Log ("转换完成");
	}

	public static bool IsObsolete (MemberInfo mb)
	{
		object[] attrs = mb.GetCustomAttributes (true);

		for (int j = 0; j < attrs.Length; j++) {
			Type t = attrs [j].GetType ();

			if (t == typeof(System.ObsoleteAttribute)) { // || t.ToString() == "UnityEngine.WrapperlessIcall")
				return true;
			}
		}

		if (IsMemberFilter (mb)) {
			return true;
		}

		return false;
	}

	static void WriteField (StreamWriter sw, string returnType, string fieldName)
	{
		sw.WriteLine (string.Format ("---@field public {0} {1}", fieldName, returnType));
	}

	static void WriteFunComment (StreamWriter sw, List<MethodInfo> list)
	{
		for (int i = 0, imax = list.Count; i < imax; i++) {
			WriteOneComment (sw, list [i]);
		}
	}

	static void WriteCtorComment (StreamWriter sw, List<ConstructorInfo> list)
	{
		for (int i = 0, imax = list.Count; i < imax; i++) {
			WriteOneComment (sw, list [i]);
		}
	}

	static void WriteOneComment (StreamWriter sw, MethodBase method)
	{
		ParameterInfo[] paramInfos;
		string argsStr = "";
		paramInfos = method.GetParameters ();
		for (int i = 0, imax = paramInfos.Length; i < imax; i++) {
			if (i != 0) {
				argsStr += ", ";
			}
			argsStr += paramInfos [i].ParameterType.Name + " " + RepalceLuaKeyWord (paramInfos [i].Name);
//			sw.WriteLine ("---@param " + paramInfos [i].ParameterType.Name + " " + RepalceLuaKeyWord (paramInfos [i].Name));
		}
		if (method is MethodInfo) {
			sw.WriteLine ("---public {0} {1}({2})", ((MethodInfo)method).ReturnType.Name, method.Name, argsStr);
		} else if (method is ConstructorInfo) {
			sw.WriteLine ("---public {0} {1}({2})", method.ReflectedType.Name, method.Name, argsStr);
		}
	}

	static void WriteFun (StreamWriter sw, ParameterInfo[] paramInfos, int delta, Type methodReturnType, string methodName, bool isStatic)
	{
		string typeStr = ConvertToLuaType (methodReturnType);
		if (methodReturnType != typeof(void)) {
			sw.WriteLine (string.Format ("---@return {0}", typeStr));
		}

		string argsStr = "";
		for (int i = 0, imax = paramInfos.Length; i < imax; i++) {
			if (i != 0) {
				argsStr += ", ";
			}
//			argsStr += RepalceLuaKeyWord(paramInfos[i].Name);
			argsStr += paramInfos [i].Name;
			if (i < delta) {
				sw.WriteLine ("---@param " + paramInfos [i].ParameterType.Name + " " + RepalceLuaKeyWord (paramInfos [i].Name));
			} else {
				sw.WriteLine ("---@param optional " + paramInfos [i].ParameterType.Name + " " + RepalceLuaKeyWord (paramInfos [i].Name));
			}
		}
//		for (int i = 1, imax = delta; i <= imax; i++) {
//			int index = paramInfos.Length - 1 - delta + i;
//			sw.WriteLine(string.Format("---@param optional {0} {1}", RepalceLuaKeyWord(paramInfos[index].Name), ConvertToLuaType(paramInfos[index].ParameterType)));
//		}
		if (isStatic) {
			sw.WriteLine (string.Format ("function m.{0}({1}) end", methodName, argsStr));
		} else {
			sw.WriteLine (string.Format ("function m:{0}({1}) end", methodName, argsStr));
		}
	}

	static string ConvertToLuaType (Type methodReturnType)
	{
		string result = "";

		if (methodReturnType != typeof(void)) {

			if (methodReturnType == typeof(bool)) {
				result = "bool";
			} else if (methodReturnType == typeof(long) || methodReturnType == typeof(ulong)) {
				result = "long";
			} else if (methodReturnType.IsPrimitive || methodReturnType.IsEnum) {
				result = "number";
			} else if (methodReturnType == typeof(LuaFunction)) {
				result = "function";
			} else if (methodReturnType == typeof(Type)) {
				result = "string";
			} else if (methodReturnType.IsArray) {
				result = "table";
			} else if (methodReturnType.IsGenericType
			         && methodReturnType.GetGenericTypeDefinition () == typeof(IDictionary<,>)) {
				result = "table";
			}
//			else if (methodReturnType.IsGenericType && methodReturnType.IsSubclassOf(typeof(LuaValueBase))) {
//				result = "table";
//			}
			else {
				result = methodReturnType.Name;
			}
		}
		return result;
	}

	static string RepalceLuaKeyWord (string name)
	{
		if (name == "table") {
			name = "tb";
		} else if (name == "function") {
			name = "func";
		} else if (name == "type") {
			name = "t";
		} else if (name == "end") {
			name = "ed";
		} else if (name == "local") {
			name = "loc";
		} else if (name == "and") {
			name = "ad";
		} else if (name == "or") {
			name = "orz";
		} else if (name == "not") {
			name = "no";
		}
		return name;
	}

	#endregion

	//
	//	static List<ToLuaMenu.BindType> allTypes;
	//	static ToLuaMenu.BindType[] GenBindTypes(ToLuaMenu.BindType[] list, bool beDropBaseType = true)
	//	{
	//		allTypes = new List<ToLuaMenu.BindType>(list);
	//
	//		for (int i = 0; i < list.Length; i++)
	//		{
	//			for (int j = i + 1; j < list.Length; j++)
	//			{
	//				if (list[i].type == list[j].type)
	//					throw new NotSupportedException("Repeat BindType:" + list[i].type);
	//			}
	//
	//			if (ToLuaMenu.dropType.IndexOf(list[i].type) >= 0)
	//			{
	//				Debug.LogWarning(list[i].type.FullName + " in dropType table, not need to export");
	//				allTypes.Remove(list[i]);
	//				continue;
	//			}
	//			else if (beDropBaseType && ToLuaMenu.baseType.IndexOf(list[i].type) >= 0)
	//			{
	//				Debug.LogWarning(list[i].type.FullName + " is Base Type, not need to export");
	//				allTypes.Remove(list[i]);
	//				continue;
	//			}
	//			else if (list[i].type.IsEnum)
	//			{
	//				continue;
	//			}
	//
	//			AutoAddBaseType(list[i], beDropBaseType);
	//		}
	//
	//		return allTypes.ToArray();
	//	}
	//
	//	static void AutoAddBaseType(ToLuaMenu.BindType bt, bool beDropBaseType)
	//	{
	//		Type t = bt.baseType;
	//
	//		if (t == null)
	//		{
	//			return;
	//		}
	//
	//		if (t.IsInterface)
	//		{
	//			Debugger.LogWarning("{0} has a base type {1} is Interface, use SetBaseType to jump it", bt.name, t.FullName);
	//			bt.baseType = t.BaseType;
	//		}
	//		else if (ToLuaMenu.dropType.IndexOf(t) >= 0)
	//		{
	//			Debugger.LogWarning("{0} has a base type {1} is a drop type", bt.name, t.FullName);
	//			bt.baseType = t.BaseType;
	//		}
	//		else if (!beDropBaseType || ToLuaMenu.baseType.IndexOf(t) < 0)
	//		{
	//			int index = allTypes.FindIndex((iter) => { return iter.type == t; });
	//
	//			if (index < 0)
	//			{
	//				#if JUMP_NODEFINED_ABSTRACT
	//				if (t.IsAbstract && !t.IsSealed)
	//				{
	//				Debugger.LogWarning("not defined bindtype for {0}, it is abstract class, jump it, child class is {1}", t.FullName, bt.name);
	//				bt.baseType = t.BaseType;
	//				}
	//				else
	//				{
	//				Debugger.LogWarning("not defined bindtype for {0}, autogen it, child class is {1}", t.FullName, bt.name);
	//				bt = new BindType(t);
	//				allTypes.Add(bt);
	//				}
	//				#else
	//				Debugger.LogWarning("not defined bindtype for {0}, autogen it, child class is {1}", t.FullName, bt.name);
	//				bt = new ToLuaMenu.BindType(t);
	//				allTypes.Add(bt);
	//				#endif
	//			}
	//			else
	//			{
	//				return;
	//			}
	//		}
	//		else
	//		{
	//			return;
	//		}
	//		AutoAddBaseType(bt, beDropBaseType);
	//	}
}
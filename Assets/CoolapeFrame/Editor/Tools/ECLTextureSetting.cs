using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ECLTextureSetting
{

	//The values for the chosen platform are returned in the "out" parameters.
	//The options for the platform string are "Standalone", "Web", "iPhone", "Android", "WebGL", "Windows Store Apps", "PSP2", "PS4", "XboxOne", "Nintendo 3DS", "WiiU" and "tvOS".
	public static string platform {
		get {
			#if UNITY_ANDROID
			return "Android";
			#elif UNITY_IPHONE || UNITY_IOS
			return "iPhone";
			#endif
			return "Standalone";
		}
	}

	public static bool isPot (Texture tex)
	{
		int w = tex.width;
		int h = tex.height;
		if (w != h)
			return false;
		if ((w & w - 1) == 0)
			return true;
		return false;
	}

	public static bool multipleOf4 (Texture tex)
	{
		int w = tex.width;
		int h = tex.height;
		if (w % 4 == 0 && h % 4 == 0) {
			return true;
		}
		return false;
	}

	public static bool alphaIsTransparency (TextureImporter ti)
	{
		return ti.DoesSourceTextureHaveAlpha ();
	}

	public static bool setTexture (string path)
	{
		Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D> (path);
		if (tex == null)
			return false;
		TextureImporter ti = TextureImporter.GetAtPath (path) as TextureImporter;
		if (ti == null)
			return  false;
		TextureImporterPlatformSettings tips = ti.GetPlatformTextureSettings (platform);

//		Debug.Log (tips.format);
		switch (tips.format) {
		case TextureImporterFormat.ETC_RGB4:
			if (isPot (tex)) {
				return false;
			} else {
				return doSetTexture (path, ti, tips, tex);
			}
		case TextureImporterFormat.ETC2_RGB4:
		case TextureImporterFormat.ETC2_RGBA8:
		case TextureImporterFormat.ETC2_RGB4_PUNCHTHROUGH_ALPHA:
			if (isPot (tex) || multipleOf4 (tex)) {
				return false;
			} else {
				return doSetTexture (path, ti, tips, tex);
			}
		case TextureImporterFormat.PVRTC_RGB2:
		case TextureImporterFormat.PVRTC_RGB4:
		case TextureImporterFormat.PVRTC_RGBA2:
		case TextureImporterFormat.PVRTC_RGBA4:
			if (isPot (tex)) {
				return false;
			} else {
				return doSetTexture (path, ti, tips, tex);
			}
		case TextureImporterFormat.ASTC_RGBA_10x10:
		case TextureImporterFormat.ASTC_RGBA_12x12:
		case TextureImporterFormat.ASTC_RGBA_4x4:
		case TextureImporterFormat.ASTC_RGBA_5x5:
		case TextureImporterFormat.ASTC_RGBA_6x6:
		case TextureImporterFormat.ASTC_RGBA_8x8:
		case TextureImporterFormat.ASTC_RGB_10x10:
		case TextureImporterFormat.ASTC_RGB_12x12:
		case TextureImporterFormat.ASTC_RGB_4x4:
		case TextureImporterFormat.ASTC_RGB_5x5:
		case TextureImporterFormat.ASTC_RGB_6x6:
		case TextureImporterFormat.ASTC_RGB_8x8:
			return false;
		case TextureImporterFormat.RGB16:
		case TextureImporterFormat.RGBA16:
		case TextureImporterFormat.RGBAHalf:
			return false;
		case TextureImporterFormat.RGBA32:
		case TextureImporterFormat.RGB24:
			return doSetTexture (path, ti, tips, tex);
		case  TextureImporterFormat.Automatic:
			#if UNITY_IPHONE || UNITY_IOS
			if(isPot(tex)) {
				return false;
			} else {
				return doSetTexture (path, ti, tips, tex);
			}
			#elif UNITY_ANDROID
			if (isPot (tex) || multipleOf4 (tex)) {
				return false;
			} else {
				return doSetTexture (path, ti, tips, tex);
			}
			#endif
		default:
			Debug.LogError ("some case not cased===" + path + "===" + tips.format.ToString ());
			return false;
		}

		return false;
	}

	public static bool doSetTexture (string path, TextureImporter ti, TextureImporterPlatformSettings tips, Texture2D tex)
	{
		tips.overridden = true;
		Debug.Log ("doSetTexture==" + path);
		if (isPot (tex)) {
			if (alphaIsTransparency (ti)) {
				#if UNITY_IPHONE || UNITY_IOS
				tips.format = TextureImporterFormat.PVRTC_RGBA4;
				#else
				tips.format = TextureImporterFormat.ETC2_RGBA8;
				#endif
			} else {
				#if UNITY_IPHONE || UNITY_IOS
				tips.format = TextureImporterFormat.PVRTC_RGB4;
				#else
				tips.format = TextureImporterFormat.ETC_RGB4;
				#endif
			}
		} else if (multipleOf4 (tex)) {
			if (alphaIsTransparency (ti)) {
				#if UNITY_IPHONE || UNITY_IOS
				tips.format = TextureImporterFormat.ASTC_RGBA_4x4;
				#else
				tips.format = TextureImporterFormat.ETC2_RGBA8;
				#endif
			} else {
				#if UNITY_IPHONE || UNITY_IOS
				tips.format = TextureImporterFormat.ASTC_RGB_4x4;
				#else
				tips.format = TextureImporterFormat.ETC2_RGB4;
				#endif
			}
		} else {
			if (alphaIsTransparency (ti)) {
				#if UNITY_IPHONE || UNITY_IOS
				tips.format = TextureImporterFormat.ASTC_RGBA_4x4;
				#else
				tips.format = TextureImporterFormat.RGBA16;
				#endif
			} else {
				#if UNITY_IPHONE || UNITY_IOS
				tips.format = TextureImporterFormat.ASTC_RGB_4x4;
				#else
				tips.format = TextureImporterFormat.RGB16;
				#endif
			}
		}
		ti.SetPlatformTextureSettings (tips);
		EditorUtility.SetDirty (ti);
		AssetDatabase.WriteImportSettingsIfDirty (path);
		AssetDatabase.ImportAsset (path);
		return true;
	}
}

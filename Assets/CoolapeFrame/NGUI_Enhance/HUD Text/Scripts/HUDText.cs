//--------------------------------------------
//            NGUI: HUD Text
// Copyright © 2012 Tasharen Entertainment
//--------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// HUD text creates temporary on-screen text entries that are perfect for damage, effects, and messages.
/// </summary>

[AddComponentMenu("NGUI/Examples/HUD Text")]
public class HUDText : MonoBehaviour
{
	protected class Entry
	{
		public float time;			// Timestamp of when this entry was added
		public float stay = 0f;		// How long the text will appear to stay stationary on the screen
		public float offset = 0f;	// How far the object has moved based on time
		public float val = 0f;		// Optional value (used for damage)
		public UILabel label;		// Label on the game object
		public float scaleTime = 0; // add by chenbin

		public float movementStart { get { return time + stay; } }
	}

	/// <summary>
	/// Sorting comparison function.
	/// </summary>

	static int Comparison (Entry a, Entry b)
	{
		if (a.movementStart < b.movementStart) return -1;
		if (a.movementStart > b.movementStart) return 1;
		return 0;
	}

	/// <summary>
	/// Font that will be used to create labels.
	/// </summary>
	[HideInInspector]
	public string fontName;	//add by chenbin
	[HideInInspector]
	public UIFont font;

	/// <summary>
	/// Effect applied to the text.
	/// </summary>

	public UILabel.Effect effect = UILabel.Effect.None;

	public bool inactiveWhenFinish = false;
	public bool gradient = false;
	public FontStyle fontStyle = FontStyle.Normal;
	public int spacingX = 0;
	public bool needAddValue = true;
	public bool needQueue = true;
	public float scaleOffset = 1;
	public float speed = 1;
	public float hightOffset = 0;
	
	public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(1f, 1f) });
	
	/// <summary>
	/// Curve used to move entries with time.
	/// </summary>

	public AnimationCurve offsetCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(3f, 40f) });

	/// <summary>
	/// Curve used to fade out entries with time.
	/// </summary>

	public AnimationCurve alphaCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1f, 1f), new Keyframe(3f, 0.001f) });

	List<Entry> mList = new List<Entry>();
	List<Entry> mUnused = new List<Entry>();

	int counter = 0;

	/// <summary>
	/// Whether some HUD text is visible.
	/// </summary>

	public bool isVisible { get { return mList.Count != 0; } }

	/// <summary>
	/// Create a new entry, reusing an old entry if necessary.
	/// </summary>

	Entry Create ()
	{
		// See if an unused entry can be reused
		if (mUnused.Count > 0)
		{
			Entry ent = mUnused[mUnused.Count - 1];
			mUnused.RemoveAt(mUnused.Count - 1);
			ent.time = Time.realtimeSinceStartup;
			ent.label.depth = NGUITools.CalculateNextDepth(HUDRoot.go)+ 10;
			//ent.label.gameObject.active = true;
			ent.offset = 0f;
			ent.scaleTime = 0f;
			ent.label.applyGradient = gradient;
			ent.label.enabled = false;
			ent.label.gameObject.SetActive(true);
			mList.Add(ent);
			return ent;
		}
		
		// New entry
		Entry ne = new Entry();
		ne.time = Time.realtimeSinceStartup;
		ne.label = NGUITools.AddWidget<UILabel>(gameObject);
		ne.label.name = counter.ToString();
		ne.label.effectStyle = effect;
		ne.label.applyGradient = gradient;
		ne.label.fontStyle = fontStyle;
		ne.label.spacingX = spacingX;
		ne.label.bitmapFont = font;
		if (font != null) {
			ne.label.fontSize = font.defaultSize;
		} else {
			ne.label.fontSize = 30;
		}

		// Make it small so that it's invisible to start with
		ne.label.cachedTransform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
		ne.label.overflowMethod = UILabel.Overflow.ResizeHeight;
		ne.label.width = (int)(Screen.width*UIRoot.GetPixelSizeAdjustment(ne.label.gameObject)*0.5f);
		ne.scaleTime = 0f;
		mList.Add(ne);
		++counter;
		return ne;
	}

	/// <summary>
	/// Delete the specified entry, adding it to the unused list.
	/// </summary>

	void Delete (Entry ent)
	{
		ent.label.enabled = false;
		if (inactiveWhenFinish) {
			ent.label.gameObject.SetActive (false);
		} else {
			ent.label.transform.localPosition = new Vector3 (0,10000, 0);
		}
		mList.Remove(ent);
		mUnused.Add(ent);
	}

	public void init(int num = 1) {
		for(int i=0; i<num; i++) {
			Entry ne = Create();
			mList.Add(ne);
		}
		int count  = mList.Count;
		for(int i=0; i<count; i++) {
			mUnused.Add(mList[i]);
		}
		mList.Clear();
	}

	/// <summary>
	/// Add a new scrolling text entry.
	/// </summary>

	public UILabel Add (object obj, Color c, float stayDuration, float scaleOffset = 1)
	{
		if (!enabled) return null;

		float time = Time.realtimeSinceStartup;
		bool isNumeric = false;
		float val = 0f;
		this.scaleOffset = scaleOffset;

		if (obj is float)
		{
			isNumeric = true;
			val = (float)obj;
		}
		else if (obj is int)
		{
			isNumeric = true;
			val = (int)obj;
		} else if(obj is double) {
			isNumeric = true;
			val = float.Parse(obj.ToString());
		}

		if (isNumeric && needAddValue)
		{
			if (val == 0f) return null;

			for (int i = mList.Count; i > 0; )
			{
				Entry ent = mList[--i];
				if (ent.time + 1f < time) continue;

				if (ent.val != 0f)
				{
					if (ent.val < 0f && val < 0f)
					{
						ent.val += val;
						ent.label.text = Mathf.RoundToInt(ent.val).ToString();
						ent.label.fontSize = Mathf.CeilToInt(font.defaultSize*scaleOffset);
						return ent.label;
					}
					else if (ent.val > 0f && val > 0f)
					{
						ent.val += val;
//						ent.label.text = Toolkit.PStr.begin().a ("+").a (Mathf.RoundToInt(ent.val)).end(); // add by chenbin
						ent.label.text = Mathf.RoundToInt(ent.val).ToString();
						ent.label.fontSize = Mathf.CeilToInt(font.defaultSize*scaleOffset);
						return ent.label;
					}
				}
			}
		}

		// Create a new entry
		Entry ne = Create();
		ne.stay = stayDuration;
		ne.label.color = c;
		if (font != null) {
			ne.label.fontSize = Mathf.CeilToInt (font.defaultSize * scaleOffset);
		} else {
			ne.label.fontSize = 30;
		}
		ne.val = val;

		if (isNumeric) ne.label.text = (val < 0f ? Mathf.RoundToInt(ne.val).ToString() : "+" + Mathf.RoundToInt(ne.val));
		else {
			ne.label.text = obj.ToString();
		}
		ne.scaleTime = ne.scaleTime >= 1? 0 : ne.scaleTime;	//add by chenbin 
		
		// Sort the list
		mList.Sort(Comparison);
		return ne.label;
	}

	/// <summary>
	/// Disable all labels when this script gets disabled.
	/// </summary>
	void OnDisable ()
	{
		Entry lent = null;
		int count = mList.Count;
		for (int i = count-1; i >= 0; i--)
		{
			lent = mList[i];
			if (lent.label != null) {
				Delete(lent);
			}
		}
	}

	/// <summary>
	/// Update the position of all labels, as well as update their size and alpha.
	/// </summary>

	float time = 0;
	Keyframe[] offsets;
	Keyframe[] alphas;
	float alphaEnd;
	float offsetEnd;
	float totalEnd;
	Entry cellEnt;
	float currentTime;
	float offset = 0f;
	float tmpSize;
	
//	[HideInInspector]
	public bool ignoreTimeScale = true;

	void Update ()
	{
		time = Time.realtimeSinceStartup;

		offsets = offsetCurve.keys;
		alphas = alphaCurve.keys;

		offsetEnd = offsets[offsets.Length - 1].time;
		alphaEnd = alphas[alphas.Length - 1].time;
		totalEnd = Mathf.Max(offsetEnd, alphaEnd);

		// Adjust alpha and delete old entries
		for (int i = mList.Count; i > 0; )
		{
			cellEnt = mList[--i];
			if(cellEnt.stay < 0)  {
				//NGUI 2.X
//				ent.label.cachedTransform.localScale = new Vector3(font.size, font.size, 1f);  //modify by chenbin
				//NGUI 3.0
				cellEnt.label.cachedTransform.localScale = Vector3.one; // add by chenbin
				
				cellEnt.label.enabled = true;
				continue;
			}
			currentTime = time - cellEnt.movementStart;
			cellEnt.offset = offsetCurve.Evaluate(currentTime);
			cellEnt.label.alpha = alphaCurve.Evaluate(currentTime);

			// Fade the label in by adjusting its size
//			float size = font.size * Mathf.Clamp01((time - ent.time) * 4f); // del by chenbin
//			float size = Mathf.Clamp01((time - ent.time) * 4f); //add by chenbin
			cellEnt.scaleTime += (ignoreTimeScale ? RealTime.deltaTime : Time.deltaTime)*speed;//Time.deltaTime;
			cellEnt.scaleTime = cellEnt.scaleTime > 1 ? 1: cellEnt.scaleTime;
			tmpSize = scaleCurve.Evaluate(cellEnt.scaleTime);
			cellEnt.label.cachedTransform.localScale = new Vector3(tmpSize, tmpSize, 1f);

			// Delete the entry when needed
			if (currentTime > totalEnd) Delete(cellEnt);
			else cellEnt.label.enabled = true;
		}

		offset = 0f;
		// Move the entries
		for (int i = mList.Count; i > 0; )
		{
			cellEnt = mList[--i];
			if(cellEnt.stay < 0) continue;
			offset = Mathf.Max(offset, cellEnt.offset);
			cellEnt.label.cachedTransform.localPosition = new Vector3(0f, offset, 0f);

			if(needQueue) {
				offset += Mathf.Round(cellEnt.label.cachedTransform.localScale.y * cellEnt.label.height + hightOffset); //modify by chenbin
			}
		}
	}
}
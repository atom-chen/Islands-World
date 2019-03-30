using UnityEngine;
using System.Collections;
using Coolape;

public class SFourWayArrow : CLBaseLua
{
	public static SFourWayArrow self;
	public SFourWayArrowCell arrow1;//上
	public SFourWayArrowCell arrow2;//右
	public SFourWayArrowCell arrow3;//下
	public SFourWayArrowCell arrow4;//左 
    public CLBehaviour4Lua building;
	public int cellSize = 1;
	
	public SFourWayArrow ()
	{
		self = this;
	}

	Material _material;
	public Material material{
		get {
			if (_material == null) {
				_material = arrow1.GetComponent<MeshRenderer> ().sharedMaterial;
			}
			return _material;
		}
	}
	public void init() {
		setLua ();
	}

    public static void show (CLBehaviour4Lua parent, int size)
	{
		self.building = parent;
		self._show (parent.gameObject, size);
		setMatToon ();
	}
	
	public static void  hide ()
	{
		self._hide ();
	}

    //public static void setMatOutLine ()
    //{
    //	self.material.shader = Shader.Find ("Outlined/Silhouetted Diffuse");
    //	self.material.SetColor ("_Color", Color.white);
    //	self.material.SetColor ("_OutlineColor", ColorEx.getColor (0, 255, 0, 200));
    //}

    public static void setMatToon()
    {
        setMatToon(Color.white);
    }

    public static void setMatToon (Color color)
	{
		self.material.shader = Shader.Find ("Toon/Basic");
        self.material.SetColor ("_Color", color);
	}
	
	GameObject mParent = null;
	bool isShowing = false;

	public void _show (GameObject parent, int size)
	{
		mParent = parent;
		isShowing = true;
		transform.position = parent.transform.position + new Vector3 (0, 0.3f, 0);
		Vector3 arrowPos = Vector3.zero;
		int half = size / 2;
		
		if (size % 2 == 0) {
			arrowPos = Vector3.forward * (half * cellSize + 0.5f);
			arrow1.transform.localPosition = arrowPos;
			arrow1.tweenPos.from = arrow1.transform.localPosition;
			arrow1.tweenPos.to = arrow1.transform.localPosition + Vector3.forward * 0.3f;
			
			arrowPos = Vector3.right * (half * cellSize + 0.5f);
			arrow2.transform.localPosition = arrowPos;
			arrow2.tweenPos.from = arrow2.transform.localPosition;
			arrow2.tweenPos.to = arrow2.transform.localPosition + Vector3.right * 0.3f;
			
			arrowPos = Vector3.back * ((half) * cellSize + 0.5f);
			arrow3.transform.localPosition = arrowPos;
			arrow3.tweenPos.from = arrow3.transform.localPosition;
			arrow3.tweenPos.to = arrow3.transform.localPosition + Vector3.back * 0.3f;
				
			arrowPos = Vector3.left * ((half) * cellSize + 0.5f);
			arrow4.transform.localPosition = arrowPos;
			arrow4.tweenPos.from = arrow4.transform.localPosition;
			arrow4.tweenPos.to = arrow4.transform.localPosition + Vector3.left * 0.3f;
		} else {
			arrowPos = Vector3.forward * ((half + 0.5f) * cellSize + 0.5f);
			arrow1.transform.localPosition = arrowPos;
			arrow1.tweenPos.from = arrow1.transform.localPosition;
			arrow1.tweenPos.to = arrow1.transform.localPosition + Vector3.forward * 0.3f;
			
			arrowPos = Vector3.right * ((half + 0.5f) * cellSize + 0.5f);
			arrow2.transform.localPosition = arrowPos;
			arrow2.tweenPos.from = arrow2.transform.localPosition;
			arrow2.tweenPos.to = arrow2.transform.localPosition + Vector3.right * 0.3f;
			
			arrowPos = Vector3.back * ((half + 0.5f) * cellSize + 0.5f);
			arrow3.transform.localPosition = arrowPos;
			arrow3.tweenPos.from = arrow3.transform.localPosition;
			arrow3.tweenPos.to = arrow3.transform.localPosition + Vector3.back * 0.3f;
				
			arrowPos = Vector3.left * ((half + 0.5f) * cellSize + 0.5f);
			arrow4.transform.localPosition = arrowPos;
			arrow4.tweenPos.from = arrow4.transform.localPosition;
			arrow4.tweenPos.to = arrow4.transform.localPosition + Vector3.left * 0.3f;
		}
		arrow1.init (size);
		arrow2.init (size);
		arrow3.init (size);
		arrow4.init (size);
		NGUITools.SetActive (gameObject, true);
	}
	
	public void _hide ()
	{
		isShowing = false;
		NGUITools.SetActive (gameObject, false);
	}
	
	public void Update ()
	{
		if (isShowing) {
			transform.position = mParent.transform.position + new Vector3 (0, 0.3f, 0);
		}
	}
	
	public void OnClick ()
	{
		if (building != null) {
			building.OnClick ();
		}
	}
	
	public void OnPress (bool isPressed)
	{
		if (building != null) {
			building.OnPress (isPressed);
		}
	}
	
	public void OnDrag (Vector2 delta)
	{
		if (building != null) {
			building.OnDrag (delta);
		}
	}
}

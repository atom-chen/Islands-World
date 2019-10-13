/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:   角色、怪物、战斗单元的基类
  *Others:  
  *History:
*********************************************************************************
*/ 
using UnityEngine;
using System.Collections;
using System;

namespace Coolape
{
	public abstract class  CLUnit : CLBehaviour4Lua
	{
		public static float SCANRange = 30;
		//视野
		public int instanceID = 0;
		//id
		[HideInInspector]
		public int type;
		//类型
		//	[HideInInspector]
		public int id;
		//id
		[HideInInspector]
		byte[] _lev;
		//等级
		public int lev {
			get {
				return NumEx.bio2Int(_lev);
			}
			set {
				_lev = NumEx.int2Bio(value);
			}
		}

		public CLUnit mTarget;
		//目标
		public CLUnit mAttacker;
		//攻击我的对象
//		[HideInInspector]
//		public SBSliderBar _sliderLifeBar;
		//血条
	
//		public SBSliderBar lifeBar {
//			get {
//				if (_sliderLifeBar == null) {
//					createLifeBar();
//				}
//				return _sliderLifeBar;
//			}
//			set {
//				_sliderLifeBar = value;
//			}
//		}

//		public void createLifeBar()
//		{
//			try {
//				_sliderLifeBar = SBSliderBar.instance(hudAnchor, Vector3.zero);
//			} catch (Exception e) {
//				Debug.LogError(name + ":" + e.ToString());
//			}
//		}

//		public void hiddenLifeBar()
//		{
//			if (_sliderLifeBar != null) {
//				lifeBar.hide();
//			}
//			_sliderLifeBar = null;
//		}

		public bool isDead = false;
		//是否死亡
	
		public Transform hudAnchor;
		//HUD的锚点
		public CLRoleState state = CLRoleState.idel;
		//状态
		[HideInInspector]
		public bool isOffense;
		//是否进攻方
		public bool isDefense {
			get {
				return !isOffense;
			}
		}

		public bool isCopyBody = false;
		//是否分身

		public override void clean()
		{
			mAttacker = null;
//			hiddenLifeBar();
			isDead = true;
			isCopyBody = false;
			CancelInvoke();
			StopAllCoroutines();
			state = CLRoleState.idel;
			base.clean();
		}

		Collider bc;

		public Collider collider { //取得boxcollider
			get {
				if (bc == null) {
					bc = GetComponent<Collider>();
				}
				return bc;
			}
		}

		public Vector3 size = Vector3.one;

		public float minSize {
			get {
				if (collider == null)
					return 0;
				Vector3 v3 = size;
				//			float ret = v3.x < v3.y ? v3.x : (v3.y < v3.z ? v3.y : v3.z);
				float ret = v3.x < v3.z ? v3.x : v3.z;
				return ret;
			}
		}


		#if UNITY_EDITOR
		Matrix4x4 boundsMatrix;

		public virtual void OnDrawGizmos()
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireCube(transform.position, size);
			Gizmos.color = Color.white;
		}
		#endif

		Material _materials;

		public Material materials {		//Old materials
			get {
				if (_materials == null) {
					_materials = getBodyMat(mbody);
				}
				return _materials;
			}
		}

		public Transform mbody;
		//主体
		bool isOutLineMode = false;
		public static Hashtable matMap = new Hashtable();

		/// <summary>
		/// Sets the mat out line.
		/// </summary>
		public void setMatOutLine()
		{
			setMatOutLineWithColor(Color.white, ColorEx.getColor(255, 0, 0, 150));
		}

		/// <summary>
		/// Sets the mat ice effect.冰冻效果
		/// </summary>
		public void setMatIceEffect()
		{
//		setMatOutLineWithColor (Utl.newColor (0, 255, 255), Utl.newColor (0, 255, 255));
			setMatToonWithColor(ColorEx.getColor(0, 255, 255));
		}

		/// <summary>
		/// Sets the mat violent.狂暴效果
		/// </summary>
		public void setMatViolent()
		{
//		setMatOutLineWithColor (Color.red, Color.red);
			setMatToonWithColor(Color.red);
		}

		public void setMatOutLineWithColor(Color mainColor, Color outLineColor)
		{
			if (isOutLineMode)
				return;
			isOutLineMode = true;
			if (materials == null)
				return;
			Material mat = null;
			object obj = matMap [materials.mainTexture.name];
			if (obj == null) {
				mat = new Material(Shader.Find("Outlined/Silhouetted Diffuse"));
				mat.mainTexture = materials.mainTexture;
				matMap [materials.mainTexture.name] = mat;
			} else {
				mat = obj as Material;
			}
			mat.SetColor("_Color", mainColor);
			mat.SetColor("_OutlineColor", outLineColor);
			setBodyMat(mbody, mat);
		}

		public void setMatToonWithColor(Color mainColor)
		{
//		if (isOutLineMode)
//			return;
			isOutLineMode = true;
			if (materials == null)
				return;
			Material mat = null;
			object obj = matMap [materials.mainTexture.name];
			if (obj == null) {
				mat = new Material(Shader.Find("Toon/Basic"));
				mat.mainTexture = materials.mainTexture;
				matMap [materials.mainTexture.name] = mat;
			} else {
				mat = obj as Material;
			}
			mat.SetColor("_Color", mainColor);
			setBodyMat(mbody, mat);
		}

		public Material getBodyMat(Transform tr)
		{
			if (tr == null)
				return null;

			Renderer rd = tr.GetComponent<Renderer>();
			if (rd != null && rd.sharedMaterial != null) {
				return rd.sharedMaterial;
			} else {
				SkinnedMeshRenderer smr = tr.GetComponent<SkinnedMeshRenderer>();
				if (smr != null) {
					return smr.sharedMaterial;
				}
			}
			Transform trch = null;
			for (int i = 0; i < tr.childCount; i++) {
				trch = tr.GetChild(i);
				rd = trch.GetComponent<Renderer>();
				if (rd != null && rd.sharedMaterial != null) {
					return rd.sharedMaterial;
				} else {
					SkinnedMeshRenderer smr = trch.GetComponent<SkinnedMeshRenderer>();
					if (smr != null) {
						return smr.sharedMaterial;
					}
				}
			}
		
			for (int i = 0; i < tr.childCount; i++) {
				Material m = getBodyMat(tr.GetChild(i));
				if (m != null)
					return m;
			}
			return null;
		}

		public void setBodyMat(Transform tr, Material mat)
		{
			if (tr == null)
				return;
			Renderer rd = tr.GetComponent<Renderer>();
			if (rd != null && rd.sharedMaterial != null &&
			    rd.sharedMaterial.mainTexture == mat.mainTexture) {
				rd.sharedMaterial = mat;
			} else {
				SkinnedMeshRenderer smr = tr.GetComponent<SkinnedMeshRenderer>();
				if (smr != null && smr.sharedMaterial != null && smr.sharedMaterial.mainTexture == mat.mainTexture) {
					smr.sharedMaterial = mat;
				}
			}
			for (int i = 0; i < tr.childCount; i++) {
				setBodyMat(tr.GetChild(i), mat);
			}
		}

		public void setMatToon()
		{
//		if (!isOutLineMode)
//			return;
			isOutLineMode = false;
			if (materials != null) {
				setBodyMat(mbody, materials);
			}
		}

		public override void pause()
		{
			isPause = true;
			enabled = false;
			StopAllCoroutines();
			CancelInvoke();
			base.pause();
		}

		public override void regain()
		{
			isPause = false;
			enabled = true;
			base.regain();
		}

		public override void Start()
		{
		
#if UNITY_EDITOR
			//因为是通过assetebundle加载的，在真机上不需要处理，只有在pc上需要重设置shader
//			Utl.setBodyMatEdit(mbody);
#endif
			base.Start();
		}

		//==========================================
		//=== 伪随机
		//==========================================
		public float RandomFactor = 0;

		public float initRandomFactor()
		{
			RandomFactor = NumEx.NextInt(0, 1001) / 1000.0f;
			return RandomFactor;
		}

		public int fakeRandom(int min, int max)
		{
			int diff = (max - min);
			int point = Mathf.FloorToInt(diff * RandomFactor);
			return min + point;
		}

        public float RandomFactor2 = 0;

        public float initRandomFactor2()
        {
            RandomFactor2 = NumEx.NextInt(0, 1001) / 1000.0f;
            return RandomFactor2;
        }

        public int fakeRandom2(int min, int max)
        {
            int diff = (max - min);
            int point = Mathf.FloorToInt(diff * RandomFactor2);
            return min + point;
        }
        public float RandomFactor3 = 0;

        public float initRandomFactor3()
        {
            RandomFactor3 = NumEx.NextInt(0, 1001) / 1000.0f;
            return RandomFactor3;
        }

        public int fakeRandom3(int min, int max)
        {
            int diff = (max - min);
            int point = Mathf.FloorToInt(diff * RandomFactor3);
            return min + point;
        }
        //====================================================
        //====================================================
        public virtual void init(int id, int star, int lev, bool isOffense, object other)
		{
			this.id = id;
			this.lev = lev;
			this.isOffense = isOffense;
			isDead = false;
//		setMatToon ();
			instanceID = gameObject.GetInstanceID();
		}

		public abstract CLUnit doSearchTarget();

		public abstract void onBeTarget(CLUnit attacker);

		public abstract void onRelaseTarget(CLUnit attacker);

		public abstract void doAttack();

		public abstract void onHurtHP(int hurt, object skillAttr);

		public abstract bool onHurt(int hurt, object skillAttr, CLUnit attacker);

		public abstract void onHurtFinish(object skillAttr, CLUnit attacker);

		public abstract void onDead();

		public abstract void moveTo(Vector3 toPos);

		public abstract void moveToTarget(Transform target);
	
	}

	public enum CLRoleState
	{
		//    -- 空闲
		idel,
		//   -- 到处走动
		walkAround,
		//   -- 归到队伍位置中
		formation,
		//等待攻击
		waitAttack,
		//    -- 攻击
		attack,
		//   -- 寻敌
		searchTarget,
		//	--击退
		beakBack,
	}
}

using UnityEngine;
using System.Collections.Generic;

//[ExecuteInEditMode]
[AddComponentMenu("Strategy/Trade Route")]
public class MyTradeRoute : MonoBehaviour
{
	
		public List<MyTradeRoute> list = new List<MyTradeRoute> ();
		static public float globalAlpha = 1f;
	
		// The two connected towns
		[HideInInspector]
		public Vector3
				town0 = Vector3.zero;
		bool hadSetTown0 = false;
		[HideInInspector]
		public Vector3
				town1 = Vector3.zero;
		bool hadSetTown1 = false;
	
		// Controls whether this trade route is visible
		public float targetAlpha = 1.0f;
		public Color mColor = Color.white;
		public float tradeSize = 0.05f;
		// Texture used to draw the path
		public Texture2D texture = null;
	
		// Shared material
		private static Material mMat = null;
	
		// Spline created with the points above
		private SplineV 		mOriginal = new SplineV ();
		private SplineV			mNormalized = new SplineV ();
		public bool 			mRebuild = false;
		private Mesh 			mMesh = null;
		private MeshFilter		mFilter = null;
		private MeshRenderer	mRen = null;
		private float			mAlpha = 0f;
		private float			mLength = 0f;
		private Vector3			mTooltipPos;
		public bool mIsHadTrade = false;
	
		/// <summary>
		/// List of traded items
		/// </summary>

		//public List<Item> items = new List<Item>();
	
		/// <summary>
		/// List of all ships assigned to this trade route
		/// </summary>
	
		//public List<AvailableShips.Owned> ships = new List<AvailableShips.Owned>();
		//public List<MyTradeShip> ships = new List<MyTradeShip>();
	
		[HideInInspector]
		[SerializeField]
		public List<Vector3>
				KeyTradeRouteNodes;
//	[HideInInspector]
		public bool isEditMode = false;
		public bool isHeightActive = false;
		public bool isNotRender = false;
		/// <summary>
		/// Read-only access to connected towns
		/// </summary>

		public SplineV path { get { return mOriginal; } }
	
		/// <summary>
		/// Gets the normalized spline path.
		/// </summary>
	
		public SplineV normalizedPath { get { return mNormalized; } }
	
		/// <summary>
		/// Gets a value indicating whether this <see cref="TradeRoute"/> is valid.
		/// </summary>
	
		public bool isValid { get { return (hadSetTown0) && (hadSetTown1); } }
	
		/// <summary>
		/// Gets the length of the trade route.
		/// </summary>
	
		public float length { get { return isValid ? mLength : 0f; } }
	
		/// <summary>
		/// Sample the trade route spline at the specified time.
		/// </summary>
	
		public Vector3 Sample (float time)
		{
				return mOriginal.Sample (time, SplineV.SampleType.Spline);
		}
	
		/// <summary>
		/// Connect the specified town.
		/// </summary>
	
		public bool Connect (Vector3 town)
		{
				if (!hadSetTown0) {
						hadSetTown0 = true;
						town0 = town;
						Add (town);
				} else if (!hadSetTown1 && town0 != town) {
						hadSetTown1 = true;
						Add (town);
						town1 = town;
				}
				return hadSetTown1;
		}
	
		/// <summary>
		/// Adds a new point to the trade route.
		/// </summary>
	
		public void Add (Vector3 v)
		{
				//if (town0 != null && town1 == null)
				//{
				if (!isHeightActive) {
						v.y = 0.05f;
				}
			
				if (mOriginal.isValid) {
						mOriginal.AddKey (mOriginal.endTime + (v - mOriginal.end).magnitude, v);
				} else {
						mOriginal.AddKey (0.0f, v);
				}
				mRebuild = true;
				//}
		}
	
		/// <summary>
		/// Removes the last added trade route point.
		/// </summary>
	
		public bool UndoAdd ()
		{
				if (mOriginal.isValid) {
						mOriginal.list.RemoveAt (mOriginal.list.Count - 1);
						mRebuild = true;
						return true;
				}
				return false;
		}
	
		/// <summary>
		/// Copies the trade route path from the specified trade route.
		/// </summary>
	
		public void CopyPath (SplineV sp)
		{
				mRebuild = true;
				mOriginal.Clear ();
				foreach (SplineV.CtrlPoint cp in sp.list)
						mOriginal.AddKey (cp.mTime, cp.mVal);
		}
	
		/// <summary>
		/// Returns the town connected to the specified town if it's in this trade route.
		/// </summary>
	
		public bool GetConnectedTown (Vector3 town)
		{
				if (town0 == town)
						return true;
				if (town1 == town)
						return true;
				return false;
		}
	
		/// <summary>
		/// Adds this trade route to the list.
		/// </summary>
		bool isInitTrade = true;

		public void OnEnable ()
		{
				if (isInitTrade) {
						isInitTrade = false;
						list.Insert (0, this);
						resetKeyTradeRouteNodes ();
				}
//		name = "Trade Route " + GetInstanceID ();
		}
	
		public void resetKeyTradeRouteNodes ()
		{
				for (int i = 0; i < KeyTradeRouteNodes.Count; i++) {
						if ((i == 0) || (i == KeyTradeRouteNodes.Count - 1)) {
								Connect (KeyTradeRouteNodes [i]);
						} else {
								Add (KeyTradeRouteNodes [i]);
						}
				}
		}
	
		public void appendKeyTradeRouteNodes (Vector3 node)
		{
				Add (node);
		}
	
		public void endSetKeyTradeRouteNodes ()
		{
		
		}
	
		/// <summary>
		/// Removes this trade route from the list.
		/// </summary>
	
		void OnDisable ()
		{
//		list.Remove (this);
		
				if (mFilter != null) {
						Object.Destroy (mFilter);
						mFilter = null;
				}
		
				if (mMesh != null) {
						Object.Destroy (mMesh);
						mMesh = null;
				}
		
				mRebuild = true;
				//if (Config.Instance != null) Config.Instance.onGUI.Remove(DrawGUI);
		}
	
		/// <summary>
		/// Update this instance.
		/// </summary>
	
		public void Update ()
		{
#if UNITY_EDITOR
		if(isEditMode && !Application.isPlaying) {
			if(KeyTradeRouteNodes != null) {
				for(int i = 0; i < KeyTradeRouteNodes.Count -1; i++) {
					Debug.DrawLine(KeyTradeRouteNodes[i] , KeyTradeRouteNodes[i+1]);
				}
			}
		} else {
#endif
				if (!Application.isPlaying)
						return;
				bool wasVisible = targetAlpha > 0.001f;
				float factor = Mathf.Min (1f, Time.deltaTime * 5f);
				//mAlpha = Mathf.Lerp (mAlpha, targetAlpha * globalAlpha, factor);
				if (targetAlpha > 0.001f) {
						if (mRebuild) {
								if (mOriginal.isValid) {
										mRebuild = false;
										if (isNotRender) {
												Vector3 centerPos = Vector3.zero;
												foreach (SplineV.CtrlPoint cp in mOriginal.list)
														centerPos += cp.mVal;
												centerPos *= 1.0f / mOriginal.list.Count;
												centerPos.y = 3f;
												mLength = mOriginal.GetMagnitude ();
												int divisions = Mathf.RoundToInt (mLength);
												CreateMesh (mOriginal, -centerPos, tradeSize, divisions);
						
												// Create the normalized path that will be used for sampling
												mNormalized = SplineV.Normalize (mOriginal, divisions);
												mLength = mNormalized.GetMagnitude ();
												return;
										}
										if (mMesh == null) {
												mMesh = new Mesh ();
												mMesh.name = "Trade Route " + GetInstanceID ();
										}
						
										if (mFilter == null) {
												mFilter = gameObject.AddComponent<MeshFilter> ();
												mFilter.mesh = mMesh;
										}
					
										if (mMat == null) {
//							Shader shader = Shader.Find ("Transparent/Diffuse");
												Shader shader = Shader.Find ("Mobile/Transparent/Vertex Color");
							
												//						Shader shader = Shader.Find ("Toon/Basic");
												mMat = new Material (shader);
												mMat.name = "Trade Route" + mMat.GetInstanceID ();
												//mMat.color = mColor;
												mMat.mainTexture = texture;
										}
					
										if (mRen == null) {
												mRen = gameObject.AddComponent<MeshRenderer> ();
												mRen.material = mMat;
												mRen.castShadows = false;
												mRen.receiveShadows = false;
										}
										// Find the center of the spline
										Vector3 center = Vector3.zero;
										foreach (SplineV.CtrlPoint cp in mOriginal.list)
												center += cp.mVal;
										center *= 1.0f / mOriginal.list.Count;
										center.y = 3f;
										// Reposition the trade route
										//transform.position = center;
										transform.localPosition = center;
					
										// Re-create the mesh
										mLength = mOriginal.GetMagnitude ();
										int subdivisions = Mathf.RoundToInt (mLength);
										CreateMesh (mOriginal, -center, tradeSize, subdivisions);
						
										// Create the normalized path that will be used for sampling
										mNormalized = SplineV.Normalize (mOriginal, subdivisions);
										mLength = mNormalized.GetMagnitude ();
								} else if (mMesh != null) {
										mMesh.Clear ();
										if (mRen != null)
												mRen.enabled = false;
								}
								// Update the material color
								if (mRen != null) {
										mRen.enabled = true;
										Color newColor = mColor;
//					if (isValid) {
//						newColor.a = 0.5f;
//					} else {
//						newColor.a = 0.25f;
//					}
										//newColor.a = targetAlpha;
										mRen.material.color = newColor;
								}
						}
				} else if (wasVisible) {
						mAlpha = 0f;
						mRen.enabled = false;
				}
#if UNITY_EDITOR
		}
#endif
		}
	
		/// <summary>
		/// Creates a trade route mesh.
		/// </summary>
	
		void CreateMesh (SplineV initial, Vector3 offset, float width, int subdivisions)
		{
				if (mMesh != null) {
						mMesh.Clear ();
				}
				if (initial.list.Count < 2)
						return;
		
				SplineV spline = SplineV.Normalize (initial, subdivisions * 4);
		
				float start = spline.startTime;
				float length = spline.endTime - start;
		
				// We will need the spline's center for tooltip purposes
				mTooltipPos = spline.Sample (start + length * 0.5f, SplineV.SampleType.Spline);
		
				List<Vector3> v = new List<Vector3> ();
				List<Vector3> n = new List<Vector3> ();
				List<Vector2> uv = new List<Vector2> ();
				List<int> faces = new List<int> ();
		
				++subdivisions;
		
				for (int i = 0; i < subdivisions; ++i) {
						float f0 = (float)(i - 1) / subdivisions;
						float f1 = (float)(i) / subdivisions;
						float f2 = (float)(i + 1) / subdivisions;
						float f3 = (float)(i + 2) / subdivisions;
			
						Vector3 s0 = spline.Sample (start + f0 * length, SplineV.SampleType.Linear);
						Vector3 s1 = spline.Sample (start + f1 * length, SplineV.SampleType.Linear);
						Vector3 s2 = spline.Sample (start + f2 * length, SplineV.SampleType.Linear);
						Vector3 s3 = spline.Sample (start + f3 * length, SplineV.SampleType.Linear);
						Vector3 dir0 = (s2 - s0).normalized;
						Vector3 dir1 = (s3 - s1).normalized;
						//Debug.Log(s0 +"    " + s1 + "    " + s2 + "    " + s3);
			
						// Cross(dir, up)
						Vector3 tan0 = new Vector3 (-dir0.z, 0f, dir0.x);
						Vector3 tan1 = new Vector3 (-dir1.z, 0f, dir1.x);
			
						tan0 *= width;
						tan1 *= width;
						Vector3 v0 = s1 - tan0;
						Vector3 v1 = s2 - tan1;
						Vector3 v2 = s2 + tan1;
						Vector3 v3 = s1 + tan0;
			
						v.Add (offset + v1);
						n.Add (Vector3.up);
						uv.Add (new Vector2 (1.0f, f2));
			
						v.Add (offset + v0);
						n.Add (Vector3.up);
						uv.Add (new Vector2 (1.0f, f1));
			
						v.Add (offset + v3);
						n.Add (Vector3.up);
						uv.Add (new Vector2 (0.0f, f1));
			
						v.Add (offset + v2);
						n.Add (Vector3.up);
						uv.Add (new Vector2 (0.0f, f2));
				}
				for (int i = 0; i < v.Count; i += 4) {
						faces.Add (i);
						faces.Add (i + 1);
						faces.Add (i + 2);
						faces.Add (i + 2);
						faces.Add (i + 3);
						faces.Add (i);
				}
		
				// Assign the mesh data
				if (mMesh != null) {
						mMesh.vertices = v.ToArray ();
						mMesh.normals = n.ToArray ();
						mMesh.uv = uv.ToArray ();
						mMesh.triangles = faces.ToArray ();
				}
		}
	
		/// <summary>
		/// Finds the next trade route connected to the specified town.
		/// </summary>
	
		public MyTradeRoute FindNext (MyTradeRoute tradeRoute, Vector3 town, bool reverse)
		{
				bool found = false;
				MyTradeRoute first = null;
				MyTradeRoute last = null;
		
				foreach (MyTradeRoute tr in list) {
						if (tr == tradeRoute) {
								// Now that we've found the current node, if we're going in reverse, we can use the last node
								if (reverse && last != null)
										return last;

								// Remember that we've found the current node
								found = true;
						} else if (tr.GetConnectedTown (town)) {
								// If the current node has already been found and we're going in order, we're done
								if (found && !reverse)
										return tr;

								// Remember this node
								if (first == null)
										first = tr;
								last = tr;
						}
				}
		
				// If we were going in reverse, just return the last available node
				if (reverse)
						return (last == null) ? tradeRoute : last;

				// Going in order? Just return the first node.
				return (first == null) ? tradeRoute : first;
		}
	
		public MyTradeRoute getNext (MyTradeRoute tradeRoute, bool reverse = false)
		{
				for (int i = 0; i < list.Count; i++) {
						MyTradeRoute tr = list [i];
						if (tradeRoute == tr) {
								if (!reverse) {
										if (i < list.Count - 1) {
												return list [i + 1];
										} else {
												return list [i];
										}
								} else {
										if (i > 0) {
												return list [i - 1];
										} else {
												return list [i];
										}
								}
						}
				}
				return null;
		}
}
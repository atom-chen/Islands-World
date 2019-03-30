using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Strategy/Trade Ship")]
public delegate void FinishActionDelegate ();

public class MyTradeShip : MonoBehaviour
{	
	public enum Style
	{
		Once,
		Loop,
		PingPong,
	}
	public GameObject tail;
	public Style style = Style.Once;
	public float 			arriveDistance = 0f;
	public float			maxSpeed = 0f;
	public float 			mAcceleration = 0f;
	public float			speed = 0f;
	public float 			finishPercent = 0f;	//已经行驶完成的百分比
	float 						mfinishPercent = 0f;	//已经行驶完成的百分比
	public float			distance = 0f;
	MyTradeRoute 		tradeRoute = null;
	public MyTradeRoute 		tradeRouteMain = null;
	Transform	mTrans;
	Vector2 	mOffset;
	Vector3 	mTargetPos;
	Quaternion 	mTargetRot;
	float 		mStartTime = 0f;
	float 		mNextUpkeep = 0f;
	Vector3 mLastTown;
	bool hadLastTown = false;
	int	mCurrentRevenue = 0;
	int	mCurrentUpkeep = 0;
	int mLastRevenue = 0;
	int mLastUpkeep = 0;
	float mNextWeek = 0f;
	FinishActionDelegate finishCallback;
	
	void Start ()
	{
		mStartTime = Time.time + 1f;
		mNextUpkeep = Time.time + 0.5f;
		mNextWeek = Time.time + 60f;
		mfinishPercent = finishPercent;
		init ();
	}
	
	/// <summary>
	/// Position the ship at the beginning of the trade route.
	/// </summary>
	
	void OnEnable ()
	{
		//init();
	}
	
	void init ()
	{
		mTrans = transform;
		if (tradeRouteMain != null) {
			tradeRouteMain.Update ();
			tradeRoute = tradeRouteMain;
			initPos ();
			hadLastTown = false;
			canMoveShip = true;
			mTargetRot = mTrans.localRotation;
			mTargetPos = mTrans.localPosition;
			mOffset.x = Random.Range (0.0f, 10.0f);
			mOffset.y = Random.Range (0.0f, 10.0f);
		}
	}

	void initPos ()
	{
		if (tradeRouteMain == null || tradeRouteMain.length == 0)
			return;
		float length = tradeRouteMain.length;
		distance = length * mfinishPercent;
		Vector3 nextPos;
		float time = Interpolation.Linear (0f, length, finishPercent);
		mTargetPos = tradeRouteMain.normalizedPath.Sample (time, SplineV.SampleType.Linear);
		nextPos = tradeRouteMain.normalizedPath.Sample (time + 1f, SplineV.SampleType.Linear);
		DockShip (tradeRoute.town0);
		Vector3 diff = nextPos - mTargetPos;
		if (diff.magnitude > 0.01f)
			mTargetRot = Quaternion.LookRotation (diff);
		
		mTrans.localPosition = mTargetPos;
		mTrans.localRotation = mTargetRot;
	}
	/// <summary>
	/// Update this instance.
	/// </summary>
	
	public void tradePlay ()
	{
		init ();
	}

	public void tradePlay (float percent)
	{
		mfinishPercent = percent;
		init ();
	}
	
	public void tradePlay (FinishActionDelegate callback)
	{
		finishCallback = callback;
		tradePlay ();
	}
	
	bool canMoveShip = false;

	void Update ()
	{
		// If this is a brand new ship, dock it at the first town
		
		// Calculate the bobble rotation
		Vector3 rot = new Vector3 (Mathf.Sin (mOffset.x + Time.time * 0.7326f) * 0.75f, 0f,
			Mathf.Sin (mOffset.y + Time.time * 1.2265f) * 1.5f);
		Quaternion bobble = Quaternion.Euler (rot);
		
		if (tradeRoute == null) {
			mTrans.rotation = bobble;
			if (tail != null)
				tail.SetActiveRecursively (false);
		} else {
			if (!hadLastTown)
				initPos ();
			//DockShip (tradeRoute.town0);
			
			// If it's time to start moving the ship, do that
			if (mStartTime < Time.time && canMoveShip) {
				if (tail != null)
					tail.SetActiveRecursively (true);

				// Ships should start with the speed of 0 and accelerate gradually
				//float acceleration = 0.05f * prefab.acceleration * Time.deltaTime;
				float acceleration = mAcceleration * Time.deltaTime;
				
				// Adjust the traveling speed
				speed = Mathf.Min (speed + acceleration, maxSpeed);
				//speed = speed + acceleration;
				
				// Distance the ship has traveled since it left the dock
				distance += speed * Time.deltaTime;
				
				// Sampling factor in 0-1 range
				float length = tradeRoute.length;
				float factor = Mathf.Clamp01 (distance / length);
				mfinishPercent = factor;
				Vector3 nextPos = Vector3.zero;
				bool hadNextPos = false;
				//Debug.Log("mLastTown == tradeRoute.town0==" + mLastTown.ToString() + " == " +tradeRoute.town0.ToString());
				if (mLastTown == tradeRoute.town0) {
					// Traveling from Town0 to Town1
					float time = Interpolation.Linear (0f, length, factor);
					mTargetPos = tradeRoute.normalizedPath.Sample (time, SplineV.SampleType.Linear);
					nextPos = tradeRoute.normalizedPath.Sample (time + 1f, SplineV.SampleType.Linear);
					hadNextPos = true;
					if (factor == 1f || Vector3.Distance (transform.localPosition, tradeRoute.town1) <= arriveDistance) { 
						DockShip (tradeRoute.town1);
						speed = 0f;
						distance = 0f;
						tradeRoute = tradeRouteMain.getNext (tradeRoute);
					}
				} else {
					if (style == Style.PingPong) {
						// Traveling from Town1 to Town0
						float time = Interpolation.Linear (length, 0f, factor);
						mTargetPos = tradeRoute.normalizedPath.Sample (time, SplineV.SampleType.Linear);
						nextPos = tradeRoute.normalizedPath.Sample (time - 1f, SplineV.SampleType.Linear);
						hadNextPos = true;
						if (factor == 1f) {
							DockShip (tradeRoute.town0);
							speed = 0f;
							distance = 0f;
							tradeRoute = tradeRouteMain.getNext (tradeRoute, true);
						}
					} else if (style == Style.Loop) {
						tradeRoute = tradeRouteMain;
						DockShip (tradeRouteMain.town0);
						speed = 0f;
						distance = 0f;
					} else {
						canMoveShip = false;
						if (tail != null)
							tail.SetActiveRecursively (false);
						
						if (finishCallback != null) {
							finishCallback ();
						}
					}
				}
				
				// Calculate the rotation
				if (hadNextPos) {
					Vector3 diff = nextPos - mTargetPos;
					if (diff.magnitude > 0.01f) {
						mTargetRot = Quaternion.LookRotation (diff);
					}
				}
			} else {
				//speed = 0f;
				//distance = 0f;
			}

			// Update the position
			{
				float factor = Time.deltaTime * 5.0f;
				//mTrans.position = Vector3.Lerp (mTrans.position, mTargetPos, factor);
				mTrans.localPosition = Vector3.Lerp (mTrans.localPosition, mTargetPos, factor);
				//mTrans.rotation = Quaternion.Slerp (mTrans.rotation, bobble * mTargetRot, factor);
				mTrans.localRotation = Quaternion.Slerp (mTrans.localRotation, bobble * mTargetRot, factor);
			}
		}
	}
	
	/// <summary>
	/// Dock the ship at the specified town.
	/// </summary>
	
	void DockShip (Vector3 town)
	{
		mStartTime = Time.time + 1f;
		mLastTown = town;
		//Debug.Log("mLastTown=================" + mLastTown);
		hadLastTown = true;
	}
	
}
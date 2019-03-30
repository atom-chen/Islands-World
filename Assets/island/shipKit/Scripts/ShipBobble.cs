using UnityEngine;

[AddComponentMenu("Exploration/Ship Bobble")]
public class ShipBobble : MonoBehaviour
{
	//public ShipController control;
	public float mSpeed = 1;

	Transform mTrans;
	Vector3 mOffset;
	Vector2 mTime;

	void Start ()
	{
		mTrans = transform;
		mOffset.x = Random.Range(0.0f, 10.0f);
		mOffset.y = Random.Range(0.0f, 10.0f);
	}

    float strength = 0;
    float delta = 0;
    Vector3 rot = Vector3.zero;
    void Update ()
	{
        //float strength = mSpeed + ((control != null) ? control.speed : 0f);
        strength = mSpeed;
        delta = Time.deltaTime * strength;

		mTime.x += delta * 0.7326f;
		mTime.y += delta * 1.2265f;

		// Calculate the bobble rotation
		rot = new Vector3(
			Mathf.Sin(mOffset.x + mTime.x) * 0.75f * strength, 
			0f,
			Mathf.Sin(mOffset.y + mTime.y) * 1.5f  * strength);
		mTrans.localRotation = Quaternion.Euler(rot);
	}
}
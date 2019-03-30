using UnityEngine;
using System.Collections;

public class Boat : MonoBehaviour {
	float orgY = 0;
	float waveSpeed = 1f;
	float moveDis = 0.2f;
	// Use this for initialization
	void Start () {
		orgY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(transform.position.x,orgY+moveDis*Mathf.Sin(Time.time*waveSpeed),transform.position.z);
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,transform.localEulerAngles.y,3*Mathf.Sin(Time.time*(waveSpeed)+1.5f));
	}
}

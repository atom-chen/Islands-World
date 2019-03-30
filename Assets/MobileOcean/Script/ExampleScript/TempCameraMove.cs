using UnityEngine;
using System.Collections;
public class TempCameraMove : MonoBehaviour {
	public float speed = 200;
	public float rotateSpeed = 40;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		transform.Translate(Vector3.forward*speed*Time.deltaTime,Space.Self);
		if(Input.GetKey("w")){
			transform.Translate(Vector3.forward*speed*Time.deltaTime,Space.Self);
		}
		
		if(Input.GetKey("s")){
			transform.Translate(Vector3.back*speed*Time.deltaTime,Space.Self);
		}
		
		if(Input.GetKey("a")){
			transform.Translate(Vector3.left*speed*Time.deltaTime,Space.Self);
		}
		
		if(Input.GetKey("d")){
			transform.Translate(Vector3.right*speed*Time.deltaTime,Space.Self);
		}
		
		
		if(Input.GetKey(KeyCode.UpArrow)){
			transform.Rotate(Vector3.left*rotateSpeed*Time.deltaTime,Space.Self);
		}
		
		if(Input.GetKey(KeyCode.DownArrow)){
			transform.Rotate(Vector3.right*rotateSpeed*Time.deltaTime,Space.Self);
		}
		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.Rotate(Vector3.down*rotateSpeed*Time.deltaTime,Space.World);
			
		}
		if(Input.GetKey(KeyCode.RightArrow)){
			transform.Rotate(Vector3.up*rotateSpeed*Time.deltaTime,Space.World);
		}
		
	}
}

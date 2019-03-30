using UnityEngine;
using System.Collections;
public class ReflectionCameraControl : MonoBehaviour {

	Camera _camera;
	public Camera camera{
		get {
			if (_camera == null) {
				_camera = GetComponent<Camera> ();
			}
			return _camera;
		}
		set{
			_camera = value;
		}
	}

	void OnPreRender(){
		GL.SetRevertBackfacing (true);
	}
	
	void OnPostRender() {
		camera.targetTexture = MirrorReflection.m_ReflectionTexture;
		GL.SetRevertBackfacing (false);
	}

	void OnDestroy(){
		GL.SetRevertBackfacing (false);
	}
}

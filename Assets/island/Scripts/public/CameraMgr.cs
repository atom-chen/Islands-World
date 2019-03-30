using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using XLua;
using Coolape;

[LuaCallCSharp]
public class CameraMgr :CLBaseLua
{
	public static CameraMgr self;

	public CameraMgr ()
	{
		self = this;
	}

	public Camera maincamera;
	public Camera subcamera;
	public PostProcessVolume postprocessing;
	public PostProcessVolume subpostprocessing;

	public float fieldOfView {
		get {
			return maincamera.fieldOfView;
		}
		set {
			maincamera.fieldOfView = value;
			if (subcamera != null) {
				subcamera.fieldOfView = value;
			}
		}
	}

	public PostProcessProfile postProcessingProfile {
		get {
			return postprocessing.profile;
		}
		set {
			postprocessing.profile = value;
		}
	}

	public PostProcessProfile postProcessingProfileSub {
		get {
			return subpostprocessing.profile;
		}
		set {
			subpostprocessing.profile = value;
		}
	}


    private static Plane[] frustumPlanes = new Plane[6];
    public static bool isInCameraView(Camera cam, Bounds bounds)
    {
        if (cam == null || bounds == null)
            return false;
        GeometryUtility.CalculateFrustumPlanes(cam, frustumPlanes);
        if (GeometryUtility.TestPlanesAABB(frustumPlanes, bounds))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [ContextMenu("Execute")]
    public void showCameraView()
    {
        // Calculate the planes from the main camera's view frustum
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        // Create a "Plane" GameObject aligned to each of the calculated planes
        for (int i = 0; i < 6; ++i)
        {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Plane);
            p.name = "Plane " + i.ToString();
            p.transform.position = -planes[i].normal * planes[i].distance;
            p.transform.rotation = Quaternion.FromToRotation(Vector3.up, planes[i].normal);
        }
    }

}

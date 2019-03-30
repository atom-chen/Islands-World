using UnityEngine;

[RequireComponent(typeof(ImprovedTrail))]
[AddComponentMenu("Exploration/Ship Trail")]
public class ShipTrail : MonoBehaviour
{
	public float speed;
    ImprovedTrail _Trail;

    public ImprovedTrail mTrail
    {
        get
        {
            if (_Trail == null)
                _Trail = GetComponent<ImprovedTrail>();
            return _Trail;
        }
    }

	void Update ()
	{
        mTrail.alpha = speed;
	}

    public void resetPosition()
    {
        mTrail.resetPoints();
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyDirectionArrow : MonoBehaviour {
    public bool isSharedMaterial = true;
    public LineRenderer line;
	public Transform arrow;
//	public Renderer arrowRender;
	Vector3 startPos;
	Vector3 endPos;
	float dis = 0f;
	float dottedSpacing = 1f;

    Material _lineMaterail;
    public Material lineMaterail
    {
        get
        {
            if(_lineMaterail == null)
            {
                if(isSharedMaterial)
                {
                    _lineMaterail = line.sharedMaterial;
                }
                else
                {
                    _lineMaterail = line.material;
                }
            }
            return _lineMaterail;
        }
    }
    public void setMaterailScale(float dis)
    {
        //lineMaterail.mainTextureScale = new Vector2(dis, 1);
    }

    /// <summary>
    /// Init the specified startWidth, endWidth, startColor, endColor and dottedSpacing.
    /// </summary>
    /// <param name="startWidth">Start width.</param>
    /// <param name="endWidth">End width.</param>
    /// <param name="startColor">Start color.</param>
    /// <param name="endColor">End color.</param>
    /// <param name="dottedSpacing">Dotted spacing. 虚线间距</param>
    public void init(float startWidth, float endWidth, Color startColor, Color endColor, float dottedSpacing) {
		line.positionCount = 2;
        line.startWidth = startWidth;
        line.endWidth = endWidth;
        line.startColor = startColor;
        line.endColor = endColor;

		this.dottedSpacing = dottedSpacing;
		if (this.dottedSpacing <= 0.0001f) {
			this.dottedSpacing = 1;
		}
		line.useWorldSpace = true;
	}

	public void SetPosition(Vector3 startPos, Vector3 endPos) {
		this.startPos = startPos;
		this.endPos = endPos;
		line.SetPosition (0, startPos);
		line.SetPosition (1, endPos);
		arrow.position = endPos;
		dis = Vector3.Distance (startPos, endPos)/dottedSpacing;
        setMaterailScale(dis);

        //		Utl.RotateTowards (arrow, startPos, endPos);
    }
	
	public void SetEndPosition( Vector3 endPos) {
		this.endPos = endPos;
		line.SetPosition (1, endPos);
		arrow.position = endPos;
		dis = Vector3.Distance (startPos, endPos)/dottedSpacing;
        setMaterailScale(dis);
        //		Utl.RotateTowards (arrow, startPos, endPos);
    }

	public void SetStartPosition(Vector3 startPos) {
		this.startPos = startPos;
		line.SetPosition (0, startPos);
		dis = Vector3.Distance (startPos, endPos)/dottedSpacing;
        setMaterailScale(dis);
        //		Utl.RotateTowards (arrow, startPos, endPos);
    }

	public void SetPositions(List<Vector3> path, Vector3 startPos, int startIndex) {
		if (path == null)
			return;
        line.positionCount = path.Count + 1 - startIndex;
		line.SetPosition (0, startPos);
		int index = 1;
		Vector3 pos1 = startPos;
		Vector3 pos2 = Vector3.zero;
		dis = 0;
		for(int i = startIndex; i < path.Count; i++) {
			pos2 = path [i];
			line.SetPosition (index, pos2);
			index++;
			dis += Vector3.Distance (pos1, pos2)/dottedSpacing;
			pos1 = pos2;
		}
        setMaterailScale(dis);
    }
}

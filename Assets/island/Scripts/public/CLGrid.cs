using UnityEngine;
using System.Collections;
using Coolape;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CLGrid :UIEventListener
{
//	public static CLGrid self;
	public int numRows = 1;
	public int numCols = 1;
	public int numGroundRows = 1;
	public int numGroundCols = 1;
	public float cellSize = 1;

	public const float OffsetX = 1;
//0.8660254f;
	public const float OffsetY = 1;
//0.75f;
	public const float RowOffsetX = 0;
//0.4330127f;
	public const float OffsetZ = 1;
    //0.84f;
    public float gridLineHight = 0;
	public bool showGrid = true;
	public bool showGridRange = true;
    public GridBase grid = new GridBase ();
	bool isShowingGrid = false;

	public static string lineName = "line";
	ArrayList lineList4Rect = new ArrayList ();
	ArrayList lineList4Grid;

	LineRenderer linePrefab;

//	public CLGrid ()
//	{
//		self = this;
//	}

	// Use this for initialization
	public void Start ()
	{
		grid.Awake (transform.position, numRows, numCols, cellSize, false);
		if (showGrid) {
			show ();
		}
	}

	public void show ()
	{
		if (isShowingGrid) {
//			reShow ();
			return;
		}
		isShowingGrid = true;
        CLThingsPool.borrowObjAsyn (lineName, (Callback)onSetPrefab, null);
	}

	public void onSetPrefab (params object[] paras)
	{
		GameObject go = paras [1] as GameObject;
		linePrefab = go.GetComponent<LineRenderer> ();

		lineList4Grid = Utl.drawGrid (linePrefab, transform.position, numRows, numCols, cellSize, 
			ColorEx.getColor (255, 255, 255, 20), transform, gridLineHight);
		linePrefab.gameObject.SetActive (false);
	}

	public void showRect ()
	{
        if (lineList4Rect.Count > 0)
        {
            for (int i = 0; i < lineList4Rect.Count; i++)
            {
                NGUITools.SetActive(((LineRenderer)(lineList4Rect[i])).gameObject, true);
            }
            return;
        }
        float width = (numGroundCols * cellSize);
		float height = (numGroundRows * cellSize);
		Vector3 origin = transform.position - new Vector3 ((numGroundRows - numRows), 0, (numGroundCols - numCols));
		
		Vector3 startPos = origin + 0 * cellSize * Utl.kZAxis;// + Vector3.up * h;
		Vector3 endPos = startPos + width * Utl.kXAxis;
		drawLine (startPos, endPos, Color.red);
		startPos = origin + numGroundRows * cellSize * Utl.kZAxis;// + Vector3.up * h;
		endPos = startPos + width * Utl.kXAxis;
		drawLine (startPos, endPos, Color.red);
		
		startPos = origin + 0 * cellSize * Utl.kXAxis;// + Vector3.up * h;
		endPos = startPos + height * Utl.kZAxis;
		drawLine (startPos, endPos, Color.red);
		startPos = origin + numGroundCols * cellSize * Utl.kXAxis;// + Vector3.up * h;
		endPos = startPos + height * Utl.kZAxis;
		drawLine (startPos, endPos, Color.red);
	}

    public void hideRect()
    {
        if (lineList4Rect == null)
        {
            return;
        }
        for (int i = 0; i < lineList4Rect.Count; i++)
        {
            NGUITools.SetActive(((LineRenderer)(lineList4Rect[i])).gameObject, false);
        }
    }

    public void drawLine (Vector3 startPos, Vector3 endPos, Color color)
	{
		ArrayList list = ListEx.builder ().Add (startPos).Add (endPos).Add (color).ToList ();
		CLThingsPool.borrowObjAsyn (lineName, (Callback)doDrawLine, list);
	}

	public void doDrawLine (params object[] orgs)
	{
		string name = orgs [0].ToString();
		GameObject go = orgs [1] as GameObject;
		ArrayList list = orgs [2] as ArrayList;
		Vector3 startPos = (Vector3)(list [0]);
		Vector3 endPos = (Vector3)( list [1]);
		Color color = (Color)(list [2]);
		LineRenderer lr = go.GetComponent<LineRenderer> ();
		lr.transform.parent = transform;
		lr.SetPosition (0, startPos);
		lr.SetPosition (1, endPos);
		lr.SetColors (Color.red, Color.red);
		NGUITools.SetActive (lr.gameObject, true);
        lineList4Rect.Add(lr);

        list.Clear ();
		list = null;
	}

	public void reShow ()
	{
		if (lineList4Grid == null) {
			show ();
			return;
		}
		for (int i = 0; i < lineList4Grid.Count; i++) {
			NGUITools.SetActive (((LineRenderer)(lineList4Grid [i])).gameObject, true);
		}
	}

	public void clean ()
	{
		if (lineList4Grid != null) {
			for (int i = 0; i < lineList4Grid.Count; i++) {
				GameObject.DestroyImmediate (((LineRenderer)(lineList4Grid [i])).gameObject, true);
			}
		}
        lineList4Grid = null;

        for (int i = 0; i < lineList4Rect.Count; i++)
        {
            GameObject.DestroyImmediate(((LineRenderer)(lineList4Rect[i])).gameObject, true);
        }
        lineList4Rect.Clear();
		Transform tr = null;
		while (transform.childCount > 0) {
			tr = transform.GetChild (0);
			CLThingsPool.returnObj (lineName, tr.gameObject);
			NGUITools.SetActive (tr.gameObject, false);
		}
		tr = null;
		if (linePrefab != null) {
			CLThingsPool.returnObj (lineName, linePrefab.gameObject);
			NGUITools.SetActive (linePrefab.gameObject, false);
			linePrefab = null;	
		}
		isShowingGrid = false;
	}

	public void hide ()
	{
		if (lineList4Grid == null)
			return;
		for (int i = 0; i < lineList4Grid.Count; i++) {
			NGUITools.SetActive (((LineRenderer)(lineList4Grid [i])).gameObject, false);
		}
	}

	/// <summary>
	/// Gets the grid position.根据坐标取得在地图格子中的坐标
	/// </summary>
	/// <returns>
	/// The map position.
	/// </returns>
	/// <param name='pos'>
	/// Position.
	/// </param>
	public Vector3 getGridPos (Vector3 pos)
	{
		int flagX = 1, flagY = 1, flagZ = 1;
		int x = 0, y = 0;
		if (pos.x > 0) {
			flagX = 1;
		} else {
			flagX = -1;
		}
		
		if (pos.z > 0) {
			flagY = 1;
		} else {
			flagY = -1;
		}
		if (pos.y > 0) {
			flagZ = 1;
		} else {
			flagZ = -1;
		}
		
		y = (int)((pos.z + flagY * OffsetY / 2) / OffsetY);
		int off = y % 2;
		bool rowIndexIsUneven = (off == 1 || off == -1);
		if (rowIndexIsUneven) {
			x = (int)((pos.x - RowOffsetX + flagX * OffsetX / 2) / OffsetX);
		} else {
			x = (int)((pos.x + flagX * OffsetX / 2) / OffsetX);
		}
		int z = (int)((pos.y + flagZ * OffsetZ / 2) / OffsetZ);
		return new Vector3 (x, y, z);
	}

	public static Vector3 getPos (int x, int y, int z)
	{
		Vector3 pos = new Vector3 (x * OffsetX, z * OffsetZ, y * OffsetY);
		int off = y % 2;
		bool rowIndexIsUneven = (off == 1 || off == -1);
		if (rowIndexIsUneven)
			pos.x += RowOffsetX;
		return pos;
	}

	/// <summary>
	/// Gets the own grids.根据中心点及占用格子size,取得占用格子index数
	/// </summary>
	/// <returns>
	/// The own grids.
	/// </returns>
	/// <param name='center'>
	/// Center.
	/// </param>
	/// <param name='size'>
	/// Size.
	/// </param>
	public List<int> getOwnGrids (int center, int size)
	{
        return grid.getCells(center, size);
	}
	
	#if UNITY_EDITOR
	void OnDrawGizmos ()
	{
		if (showGrid) {
			float width = (numCols * cellSize);
			float height = (numRows * cellSize);
			Vector3 origin = transform.position;
			// Draw the horizontal grid lines
			for (int i = 0; i < numRows + 1; i++) {
				Vector3 startPos = origin + i * cellSize * Utl.kZAxis;// + Vector3.up * h;
				Vector3 endPos = startPos + width * Utl.kXAxis;
				//			LineRenderer lr = drawLine(startPos, endPos, Color.white);
				Gizmos.DrawLine (startPos, endPos);
			}
			
			// Draw the vertial grid lines
			for (int i = 0; i < numCols + 1; i++) {
				Vector3 startPos = origin + i * cellSize * Utl.kXAxis;// + Vector3.up * h;
				Vector3 endPos = startPos + height * Utl.kZAxis;
				Gizmos.DrawLine (startPos, endPos);
			}
		}
		if (showGridRange) {
			Gizmos.color = Color.red;
			float width = (numGroundCols * cellSize);
			float height = (numGroundRows * cellSize);
			Vector3 origin = transform.position - new Vector3 ((numGroundRows - numRows), 0, (numGroundCols - numCols));
			
			Vector3 startPos = origin;
			Vector3 endPos = startPos + width * Utl.kXAxis;
			Gizmos.DrawLine (startPos, endPos);
			startPos = origin + numGroundRows * cellSize * Utl.kZAxis;// + Vector3.up * h;
			endPos = startPos + width * Utl.kXAxis;
			Gizmos.DrawLine (startPos, endPos);
			
			
			startPos = origin;
			endPos = startPos + height * Utl.kZAxis;
			Gizmos.DrawLine (startPos, endPos);
			startPos = origin + numGroundCols * cellSize * Utl.kXAxis;// + Vector3.up * h;
			endPos = startPos + height * Utl.kZAxis;
			Gizmos.DrawLine (startPos, endPos);
			Gizmos.color = Color.white;
		}
	}
	#endif
}

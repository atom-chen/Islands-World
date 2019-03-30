using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coolape;

public class AITargetTest : MonoBehaviour
{
    public CLAStarPathSearch aStar;
    public int radius = 10;

    // Use this for initialization
    void Start()
    {
        Invoke("resetPosition", NumEx.NextInt(10, 50) / 10);
    }

    public void resetPosition()
    {
        if (aStar != null)
        {
            int index = NumEx.NextInt(0, aStar.grid.NumberOfCells);
            transform.position = aStar.grid.GetCellCenter(index);
        } else {
            transform.position = new Vector3(NumEx.NextInt(-radius * 10, radius*10)/10.0f, 0, NumEx.NextInt(-radius * 10, radius * 10) / 10.0f);
        }
        Invoke("resetPosition", NumEx.NextInt(20, 80) / 10);
    }
}

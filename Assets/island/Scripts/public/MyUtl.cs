using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyUtl
{
    public static float calculateAngle(Vector2 fromPos, Vector2 toPos)
    {
        float Angle = 0.0f;
        float a = Vector2.Distance(fromPos, new Vector2(fromPos.x, toPos.y));
        float b = Vector2.Distance(new Vector2(fromPos.x, toPos.y), toPos);
        float c = Vector2.Distance(toPos, fromPos);

        float cosb = (a * a + c * c - b * b) / (2 * a * c);
        float AB = Mathf.Acos(cosb);
        float B = AB * 180 / Mathf.PI;
        Angle = B;
        if (a * c <= 0.000001f)
        {
            Angle = 0;
        }

        if (Mathf.Abs(toPos.x - fromPos.x) < 0.01f && Mathf.Abs(toPos.y - fromPos.y) < 0.01f)
        {
            Angle = -1;
        }
        else if (Mathf.Abs(toPos.x - fromPos.x) < 0.01f && Mathf.Abs(toPos.y - fromPos.y) > 0.01f && toPos.y > fromPos.y)
        {
            Angle = 0;
        }
        else if (Mathf.Abs(toPos.x - fromPos.x) < 0.01f && Mathf.Abs(toPos.y - fromPos.y) > 0.01f && toPos.y < fromPos.y)
        {
            Angle = 180;
        }
        else if (Mathf.Abs(toPos.x - fromPos.x) > 0.01f && toPos.x > fromPos.x && Mathf.Abs(toPos.y - fromPos.y) < 0.01f)
        {
            Angle = 90;
        }
        else if (Mathf.Abs(toPos.x - fromPos.x) > 0.01f && toPos.x < fromPos.x && Mathf.Abs(toPos.y - fromPos.y) < 0.01f)
        {
            Angle = 90 + 180;
        }
        else if (toPos.x > fromPos.x && toPos.y > fromPos.y)
        {
            Angle = B;
        }
        else if (toPos.x > fromPos.x && toPos.y < fromPos.y)
        {
            Angle = 180 - B;
        }
        else if (toPos.x < fromPos.x && toPos.y > fromPos.y)
        {
            Angle = 360 - B;
        }
        else if (toPos.x < fromPos.x && toPos.y < fromPos.y)
        {
            Angle = 180 + B;
        }
        return Angle;
    }
}

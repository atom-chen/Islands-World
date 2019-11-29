using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coolape;

public class MyBoundsPool : AbstractObjectPool<Bounds>
{
    public static MyBoundsPool pool = new MyBoundsPool();
    public override Bounds createObject(string key = null)
    {
        return new Bounds();
    }

    public override Bounds resetObject(Bounds t)
    {
        return t;
    }

    public static Bounds borrow()
    {
        return pool.borrowObject();
    }

    public static Bounds borrow(Vector3 center, Vector3 size)
    {
        Bounds b = pool.borrowObject();
        b.center = center;
        b.size = size;
        return b;
    }

    public static void returnObj(Bounds b)
    {
        pool.returnObject(b);
    }
}

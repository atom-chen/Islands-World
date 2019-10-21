using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coolape;
using XLua;

public class MyBullet : CLBulletBase
{
    public CLBaseLua lua;
    public void init()
    {
        if (lua == null)
        {
            lua = GetComponent<CLBaseLua>();
        }
        lua.setLua();
        _lresetTarget = lua.getLuaFunction("resetTarget");
    }

    LuaFunction _lresetTarget;
    LuaFunction lresetTarget
    {
        get
        {
            if(_lresetTarget == null)
            {
                init();
            }
            return _lresetTarget;
        }
    }

    public override void resetTarget()
    {
        object[] objs = Utl.doCallback(lresetTarget, this);
        if(objs != null && objs.Length > 0)
        {
            target = objs[0] as CLUnit;
        }
    }
}

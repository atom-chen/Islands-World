using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coolape;
using XLua;

public class MyUnit : CLUnit
{	
	LuaFunction lfinit;
	public override void initGetLuaFunc ()
	{
		base.initGetLuaFunc ();
		if (luaTable != null) {
			lfinit = getLuaFunction ("init");
		}
	}

	public override void init (int id, int star, int lev, bool isOffense, object other)
	{
		setLua ();
		base.init (id, star, lev, isOffense, other);
		if (lfinit != null) {
            call(lfinit, this, id, star, lev, isOffense, other);
		}
	}
	public override void doAttack ()
	{
		throw new System.NotImplementedException ();
	}

	public override CLUnit doSearchTarget ()
	{
		throw new System.NotImplementedException ();
	}

	public override void moveTo (Vector3 toPos)
	{
		throw new System.NotImplementedException ();
	}

	public override void moveToTarget (Transform target)
	{
		throw new System.NotImplementedException ();
	}

	public override void onBeTarget (CLUnit attacker)
	{
		throw new System.NotImplementedException ();
	}

	public override bool onHurt (int hurt, object skillAttr, CLUnit attacker)
	{
		throw new System.NotImplementedException ();
	}

	public override void onHurtFinish (object skillAttr, CLUnit attacker)
	{
		throw new System.NotImplementedException ();
	}

	public override void onHurtHP (int hurt, object skillAttr)
	{
		throw new System.NotImplementedException ();
	}

	public override void onRelaseTarget (CLUnit attacker)
	{
		throw new System.NotImplementedException ();
	}
	public override void onDead ()
	{
		throw new System.NotImplementedException ();
	}
}

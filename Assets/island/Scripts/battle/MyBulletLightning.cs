using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coolape;
using DigitalRuby.LightningBolt;

public class MyBulletLightning : CLBulletBase
{
    public LightningBoltScript lightningBolt;
    public CLSmoothFollow lightningBall;
    public void OnEnable()
    {
        init(null);
    }

    public void init(GameObject startPoint)
    {
        if(startPoint == null)
        {
            startPoint = gameObject;
        }
        lightningBolt.StartObject = startPoint;
    }

    public override void resetTarget()
    {
    }

    public override void FixedUpdate()
    {
        //stop base.FixedUpdate running
    }

    public override void doFire(CLUnit attacker, CLUnit target, Vector3 orgPos, Vector3 dir, object attr, object data, object callbak)
    {
        lightningBolt.EndObject = target.gameObject;
        lightningBall.target = target.transform;
        lightningBall.LateUpdate();
        base.doFire(attacker, target, orgPos, dir, attr, data, callbak);
        gameObject.SetActive(true);
        onFinishFire(false);
        Invoke("stop", 0.4f);
    }

    public override void stop()
    {
        lightningBall.target = null;
        lightningBolt.StartObject = null;
        lightningBolt.EndObject = null;
        base.stop();
    }
    public void UpdateFromMaterialChange(GameObject go)
    {
        lightningBolt.UpdateFromMaterialChange();
    }
}

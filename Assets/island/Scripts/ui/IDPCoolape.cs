using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coolape;

public class IDPCoolape : CLPanelLua
{
    public override void show()
    {
        base.show();
    }

    public new void uiEventDelegate(GameObject go)
    {
        if (go.name == "logo")
        {
            SoundEx.playSound("logo");
        }
    }
}

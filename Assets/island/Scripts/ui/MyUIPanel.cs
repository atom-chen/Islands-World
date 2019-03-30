using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coolape;
using XLua;

public class MyUIPanel : CLPanelLua
{
    public string frameName;
    [HideInInspector]
    public CLCellLua frameObj;
    LuaFunction lfonShowFrame;

    public override void init()
    {
        base.init();
        lfonShowFrame = getLuaFunction("onShowFrame");
    }

    public void showFrame()
    {
        if (string.IsNullOrEmpty(frameName)) return;
        CLUIOtherObjPool.borrowObjAsyn(frameName, (Callback)onBorrowFrame);
    }

    void onBorrowFrame(params object[] objs)
    {
        GameObject frame = objs[1] as GameObject;
        if (frame != null)
        {
            if (frameObj != null)
            {
                CLUIOtherObjPool.returnObj(frame);
                NGUITools.SetActive(frame, false);
                return;
            }
            frameObj = frame.GetComponent<CLCellLua>();
            frameObj.transform.parent = transform;
            frameObj.transform.localScale = Vector3.one;
            frameObj.transform.localPosition = Vector3.zero;
            frameObj.transform.localEulerAngles = Vector3.zero;
            NGUITools.SetActive(frameObj.gameObject, true);
            if (frameObj.luaTable == null)
            {
                frameObj.setLua();
            }

            if (lfonShowFrame != null)
            {
                lfonShowFrame.Call(this);
            }
        }
    }

    public void releaseFrame()
    {
        if (frameObj != null)
        {
            CLUIOtherObjPool.returnObj(frameObj.gameObject);
            NGUITools.SetActive(frameObj.gameObject, false);
            frameObj = null;
        }
    }

    public override void show()
    {
        base.show();
        showFrame();
    }

    public override void hide()
    {
        base.hide();
    }

    public override void finishMoveOut()
    {
        releaseFrame();
        base.finishMoveOut();
    }
}

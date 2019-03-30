using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coolape;

namespace Coolape
{
    public class CLUIJumpLabel : UILabel
    {
        public bool isPlayJump = false;

        public string orgStr = "";
        public float speed = 0.25f;

        int index = 0;
        float timeCount = 0;

        new public string text
        {
            get
            {
                return base.text;
            }
            set
            {
                base.text = value;
                orgStr = base.processedText;
                isPlayJump = true;
                index = 0;
                timeCount = Time.time;
                base.text = PStr.b().a("[sub]").a(text).a("[/sub]").e();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!isPlayJump || string.IsNullOrEmpty(orgStr)) return;
            if (timeCount <= Time.time)
            {
                jump();
            }
        }

        public void jump()
        {
            int len = orgStr.Length;
            if (len < 2) {
                return;
            }
            index++;
            if (index >= len)
            {
                index = 0;
            }
            string left = StrEx.Left(orgStr, index);
            string mid = StrEx.Mid(orgStr, index, 1);
            string right = StrEx.Right(orgStr, len - index - 1);
            string str = PStr.b()
                             .a("[sub]")
                             .a(left)
                             .a("[/sup]")
                             .a("[sup]").a(mid).a("[/sup]")
                             .a("[sub]")
                             .a(right)
                             .a("[/sup]")
                             .e();
            base.text = str;
            timeCount = Time.time + speed;
        }

    }
}

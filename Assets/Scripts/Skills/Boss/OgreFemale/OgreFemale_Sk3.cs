using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk3 : Skill {

    IBattleHandler[] friendlyNum;
    int startNum = 0;
    private bool Sk3Acess = false;
    public float ofT3ime = 0;

    public override void RunTime()
    {
        ofT3ime += Time.deltaTime;
        base.RunTime();
    }
    
    private void Sk3On()
    {
        if(Sk3Acess==true)
        {
            if (!TimeSystem.GetTimeSystem().CheckTimer(this))
            {
                TimeSystem.GetTimeSystem().AddTimer(this);
                ofT3ime = 0;
            }

            if(ofT3ime <= 2)
            {
                //캐스팅
            }
            if(ofT3ime >= 2 && ofT3ime <= 6)
            {
                for(int i=0; i<=friendlyNum.Length-1; i++)
                {
                    Character c = friendlyNum[i] as Character;
                    bool hitCheck = EllipseScanner(3.5f, 1.3f, this.gameObject.transform.position, c.gameObject.transform.position);
                    if(hitCheck==true)
                    {
                        Debug.Log("stun "+c.transform.name);
                        //기절
                    }
                }
            }
            if(ofT3ime >= 6 && ofT3ime <= 7.5)
            {
                // 마무리 동작
            }
            if(ofT3ime > 7.5)
            {
                Sk3Acess = false;
                ofT3ime = 0;
                if (TimeSystem.GetTimeSystem().CheckTimer(this))
                {
                    TimeSystem.GetTimeSystem().DeleteTimer(this);
                }
            }
        }
    }


    private void Sk3Targgetting()
    {
        int targetingNum = 0;

        for(int i=0; i<=friendlyNum.Length-1; i++)
        {
            Character c = friendlyNum[i] as Character;
            bool targetOn = EllipseScanner(4, 1.5f, this.gameObject.transform.position, c.gameObject.transform.position);

            if(targetOn == true)
            {
                targetingNum++;
            }
        }
        if(targetingNum>=2)
        {
            Debug.Log("e skill on");
            Sk3Acess = true;
        }
    }

    private bool EllipseScanner(float a, float b, Vector3 center,Vector3 targetPosition)
    {
        float dx = targetPosition.x - center.x;
        float dy = targetPosition.y - center.y;

        float l1 = Mathf.Sqrt((dx + Mathf.Sqrt(a * a - b * b)) * (dx + Mathf.Sqrt(a * a - b * b)) + (dy * dy));
        float l2 = Mathf.Sqrt((dx - Mathf.Sqrt(a * a - b * b)) * (dx - Mathf.Sqrt(a * a - b * b)) + (dy * dy));

        if (l1 + l2 <= 2 * a)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Activate(IBattleHandler target)
    {
        if (startNum == 0)
        {
            friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
            Debug.Log(friendlyNum.Length);
            startNum = 1;
        }

        if (Sk3Acess==false)
        {
            Sk3Targgetting();
        }
        Sk3On();
    }
}

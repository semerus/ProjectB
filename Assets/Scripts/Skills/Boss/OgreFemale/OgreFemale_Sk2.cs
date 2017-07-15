using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk2 : Skill {

    IBattleHandler[] friendlyNum;
    int startNum = 0;
    float max = 0;
    float of2Time = 0;
    bool jumpTargettingOn = false;
    int jumpNum = 0;
    Vector3 adjustpoint;
    Vector3 jumpdistance;
    float jumptime = 1;
    Character maxC;
    IBattleHandler maxNum = null;
    OgreFemale_AutoAttack atk;

    // Use this for initialization
    void Start () {
        adjustpoint = this.gameObject.transform.position+ Vector3.down;
        //cooldown = 10;
        //timer_cooldown = 0;
    }

    public override void RunTime()
    {
        of2Time += Time.deltaTime;
        base.RunTime();
    }

    public void JumpAttack()
    {
        if(jumpTargettingOn==true)
        {
            if (!TimeSystem.GetTimeSystem().CheckTimer(this))
            {
                TimeSystem.GetTimeSystem().AddTimer(this);
                of2Time = 0;
            }

            if (of2Time < 1 && jumpTargettingOn == true)
            {
                caster.Move(jumpdistance, jumptime);
            }
            if(of2Time>=1 && of2Time>1.99f)
            {
                // 준비동작
            }
            if (of2Time <= 2.0 && of2Time >= 1.99)
            {
                Debug.Log("auto attack sk2 on");
                for (int i = 0; i < friendlyNum.Length; i++)
                {
                    Character c = friendlyNum[i] as Character;
                    bool hitCheck = EllipseScanner(2, 1.4f, maxC.gameObject.transform.position, c.gameObject.transform.position);
                    if (hitCheck == true)
                    {
                        if (OgreFemale_Sk5.rageOn == true)
                        {
                            IBattleHandler ch = c as IBattleHandler;
                            caster.AttackTarget(c, 100);
                        }
                        else
                        {
                            Debug.Log("Auto Attack => " + c.gameObject.transform.name);
                            IBattleHandler ch = c as IBattleHandler;
                            caster.AttackTarget(c, 50);
                        }
                    }
                }
            }
            if(of2Time>2 && of2Time<=2.5)
            {
                //마무리 동작
            }
            if(of2Time>2.5)
            {
                jumpTargettingOn = false;
                if (TimeSystem.GetTimeSystem().CheckTimer(this))
                {
                    TimeSystem.GetTimeSystem().DeleteTimer(this);
                }
            }
        }
    }

    public void JumpAttackTargetting()
    {
        max = 0;
        for (int i = 0; i < friendlyNum.Length; i++)
        {
            Debug.Log(i);
            jumpNum = 0;

            Character c = friendlyNum[i] as Character;
            float x = this.gameObject.transform.position.x - c.transform.position.x;
            float y = this.gameObject.transform.position.y - c.transform.position.y;
            float distance = x * x + y * y;

            if (distance >= max)
            {
                max = distance;
                maxNum = friendlyNum[i];
            }
        }
        maxC = maxNum as Character;
        Debug.Log(maxC.transform.name);

        if (this.gameObject.transform.position.x >= maxC.transform.position.x)
        {
            adjustpoint = maxC.transform.position;
            adjustpoint.x = maxC.transform.position.x + 1.25f;
            jumpdistance = adjustpoint - this.gameObject.transform.position;
            jumpTargettingOn = true;
        }
        else
        {
            adjustpoint = maxC.transform.position;
            adjustpoint.x = maxC.transform.position.x - 1.25f;
            jumpdistance = adjustpoint - this.gameObject.transform.position;
            jumpTargettingOn = true;
        }

        jumpTargettingOn = true;
    }

    public override void Activate(IBattleHandler target)
    {
        if (startNum == 0)
        {
            friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
            Debug.Log(friendlyNum.Length);
            startNum = 1;
        }

        if (jumpTargettingOn == false)
        {
            JumpAttackTargetting();
        }
        JumpAttack();
    }

    private bool EllipseScanner(float a, float b, Vector3 center, Vector3 targetPosition)
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
}

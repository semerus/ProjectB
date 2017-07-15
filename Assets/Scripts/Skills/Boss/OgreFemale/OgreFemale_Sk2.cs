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
            Debug.Log("asdasdasdasds");
            if (!TimeSystem.GetTimeSystem().CheckTimer(this))
            {
                TimeSystem.GetTimeSystem().AddTimer(this);
                of2Time = 0;
            }

            of2Time += Time.deltaTime;
            if (of2Time < 1 && jumpTargettingOn == true)
            {
                caster.Move(jumpdistance, jumptime);
            }
            if (of2Time >= 1 && jumpNum == 0)
            {
                jumpNum = 1;
                jumpTargettingOn = false;
                Debug.Log("nice");

                if (TimeSystem.GetTimeSystem().CheckTimer(this))
                {
                    TimeSystem.GetTimeSystem().DeleteTimer(this);
                }
            }
        }
    }

    public void JumpAttackTargetting()
    {
        //state = SkillState.OnCoolDown;

        for (int i = 0; i < friendlyNum.Length; i++)
        {
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
}

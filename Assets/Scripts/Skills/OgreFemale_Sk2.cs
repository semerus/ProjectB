using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk2 : Skill {

    IBattleHandler[] friendlyNum;
    float[] distance;
    float max = 0;
    bool jumpTargettingOn = false;
    int jumpNum = 0;
    Vector3 adjustpoint;
    Vector3 jumpdistance;
    float jumptime = 1;
    Character maxC;
    IBattleHandler maxNum = null;

    // Use this for initialization
    void Start () {
        friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
        float[] distance = new float[friendlyNum.Length];
        adjustpoint = this.gameObject.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        JumpAttack();
    }

    public void JumpAttack()
    {
        if(jumpTargettingOn==true)
        {
            if (this.gameObject.transform.position != adjustpoint && jumpTargettingOn == true)
            {
                this.gameObject.transform.position += Time.deltaTime * jumpdistance / jumptime;
            }

            if (this.gameObject.transform.position == adjustpoint && jumpNum == 0)
            {
                //기절 - maxC
                // AutoAttak();
                jumpNum = 1;
                jumpTargettingOn = false;
            }
        }
    }

    public void JumpAttackTargetting()
    {
        state = SkillState.OnCoolDown;

        for (int i = 0; i < friendlyNum.Length; i++)
        {
            jumpNum = 0;

            Character c = friendlyNum[i] as Character;
            float x = this.gameObject.transform.position.x - c.transform.position.x;
            float y = this.gameObject.transform.position.y - c.transform.position.y;
            distance[i] = x * x + y * y;

            if (distance[i] >= max)
            {
                max = distance[i];
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

    public override void Activate(IBattleHandler target) //발동조건
    {
        if (jumpTargettingOn == false)
        {
            JumpAttackTargetting();
        }
    }
}

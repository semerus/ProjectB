using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk2 : Skill {

    IBattleHandler[] friendlyNum;
    int startNum = 0;
    float max = 0;
    float ctime = 0;
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
    }
	
	// Update is called once per frame
	void Update () {
        if (startNum == 0)
        {
            friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
            Debug.Log(friendlyNum.Length);
            startNum = 1;
        }

        JumpAttack();
        if(Input.GetKeyDown("w"))
        {
            Debug.Log("w");
            Activate(null);
        }

    }

    public void JumpAttack()
    {
        if(jumpTargettingOn==true)
        {
            ctime += Time.deltaTime;

            if (ctime<1 && jumpTargettingOn == true)
            {
                this.gameObject.transform.position += Time.deltaTime * jumpdistance / jumptime;
            }

            if (ctime>=1 && jumpNum == 0)
            {
                //기절 - maxC
                // AutoAttak();
                jumpNum = 1;
                jumpTargettingOn = false;
                Debug.Log("nice");
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
            float distance = x * x + y * y;

            if (distance >= max)
            {
                max = distance;
                maxNum = friendlyNum[i];
            }
        }
        maxC = maxNum as Character;
        Debug.Log(maxC.gameObject.transform.name);

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

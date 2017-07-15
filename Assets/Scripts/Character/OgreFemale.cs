using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale : Enemy,ITimeHandler {

    public OgreFemale_AutoAttack atk1;
    public OgreFemale_Sk2 atk2;
    public OgreFemale_Sk3 atk3;
    public OgreFemale_Sk4 atk4;

    float ctime = 0;

    void Start()
    {
        speed_x = 1f;
        speed_y = 1f;
        atk1.SetSkill(this);
        atk2.SetSkill(this);
        atk3.SetSkill(this);
        atk4.SetSkill(this);
        TimeSystem.GetTimeSystem().AddTimer(this);
        ctime = 0;
        atk4.OnClick();
    }

    public void RunTime()
    {
        ctime += Time.deltaTime;
        InstructEnemyAI();
    }

    protected override void InstructEnemyAI()
    {
        if (ctime <= 10)
        {
            atk1.OnClick();
        }
        if (ctime >= 10 && ctime <=20)
        {
            StopMove();
            atk2.OnClick();
        }
        if (ctime >= 20 && ctime <= 30)
        {
            atk3.OnClick();
        }
        if(ctime>30)
        {
            ctime = 0;
        }
        base.InstructEnemyAI();
    }
    
    public override void ReceiveHeal (int heal)
	{
		base.ReceiveHeal (heal);
		Debug.Log ("Received heal : " + heal);
	}

}

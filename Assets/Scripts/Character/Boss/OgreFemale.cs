﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale : Enemy,ITimeHandler {

    public OgreFemale_AutoAttack atk1;
    public OgreFemale_Sk2 atk2;
    public OgreFemale_Sk3 atk3;
    public OgreFemale_Sk4 atk4;
    IBattleHandler[] friendlyNum;

    void OnSkillEnd(object sender, EventArgs e)
    {

    }
    
    protected override void Start()
    {
		base.Start ();

		maxHp = 3000;
		hp = 3000;

        speed_x = 1f;
        speed_y = 1f;
        atk1.SetSkill(this);
        atk2.SetSkill(this);
        atk3.SetSkill(this);
        atk4.SetSkill(this);
        TimeSystem.GetTimeSystem().AddTimer(this);
        atk1.EndSkill += new EventHandler<SkillEventArgs>(OnSkillEnd);
        atk2.EndSkill += new EventHandler<SkillEventArgs>(OnSkillEnd);
        atk3.EndSkill += new EventHandler<SkillEventArgs>(OnSkillEnd);
        atk4.EndSkill += new EventHandler<SkillEventArgs>(OnSkillEnd);
        friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
        atk2.OnCast();
    }

    public void RunTime()
    {
        InstructEnemyAI();
    }

    protected override void InstructEnemyAI()
    {
        if ((status & CharacterStatus.NotOrderableMask) > 0)
            return;

        if (atk3.CheckSkillState(SkillStatus.ReadyMask) && Sk3())
        {
            StopMove();
            atk3.OnCast();
        }
        else if (atk2.CheckSkillState(SkillStatus.ReadyMask))
        {
            StopMove();
            atk2.OnCast();
        }
        else if (atk1.CheckSkillState(SkillStatus.ReadyMask))
        {
            atk1.OnCast();
        }
    }
    
    public override void ReceiveHeal (int heal)
	{
		base.ReceiveHeal (heal);
		Debug.Log ("Received heal : " + heal);
	}

    private bool Sk3()
    {
        int targetingNum = 0;

        for (int i = 0; i <= friendlyNum.Length - 1; i++)
        {
            Character c = friendlyNum[i] as Character;
            bool targetOn = EllipseScanner(4, 1.5f, this.gameObject.transform.position, c.gameObject.transform.position);

            if (targetOn == true)
            {
                targetingNum++;
            }
        }

        if (targetingNum >= 2)
        {
            Debug.Log("e on");
            return true;
        }
        else
        {
            return false;
        }
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

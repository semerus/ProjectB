using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Hero
{
    public int[] abillity = new int[3];
    public Character atkTarget = null;

    private void Awake()
    {
<<<<<<< HEAD
        abillity[0] = 1;
        abillity[1] = 2;
        abillity[2] = 1;
        id = 2;
        team = Team.Friendly;
        status = CharacterStatus.Idle;
        maxHp = 2000;
        hp = maxHp;
        speed_x = 2.57f;
        speed_y = 1.4f;

=======
		/*
>>>>>>> 96a441a56d03b4f6eda8cbf73eb63b00e7d93ad2
        autoAttack = gameObject.AddComponent<Wizard_AutoAttack>();
        //autoAttack.SetSkill(this);

        passiveSkill = gameObject.AddComponent<Wizard_Passive>();
        //passiveSkill.SetSkill(this);
        passiveSkill.OnCast();

		activeSkills = new HeroActive[3];
        switch(abillity[0])
        {
            case 1:
                activeSkills[0] = gameObject.AddComponent<Wizard_Snowball_Abillity1>();
                break;

            case 2:
                activeSkills[0] = gameObject.AddComponent<Wizard_Snowball_Abillity2>();
                break;
        }

        switch (abillity[1])
        {
            case 1:
                activeSkills[1] = gameObject.AddComponent<Wizard_Freeze_Abillity1>();
                break;

            case 2:
                activeSkills[1] = gameObject.AddComponent<Wizard_Freeze_Abillity2>();
                break;
        }

        switch (abillity[2])
        {
            case 1:
                activeSkills[2] = gameObject.AddComponent<Wizard_Blizzard_Abillity1>();
                break;

            case 2:
                activeSkills[2] = gameObject.AddComponent<Wizard_Blizzard_Abillity2>();
                break;
        }

        foreach (Skill eachSkill in activeSkills)
        {
            //eachSkill.SetSkill(this);
        }
        */
    }

	/*
    public override void RunTime()
    {
        base.RunTime();
        if (!activeSkills[0].CheckSkillStatus(SkillStatus.ProcessMask) && !activeSkills[1].CheckSkillStatus(SkillStatus.ProcessMask) && !activeSkills[2].CheckSkillStatus(SkillStatus.ProcessMask))
        { 
            if (Input.GetKeyDown("b") && activeSkills[0].CheckSkillStatus(SkillStatus.ReadyMask) && Target!=null)
            {
                activeSkills[0].OnCast();
            }
            if (Input.GetKeyDown("n") && activeSkills[1].CheckSkillStatus(SkillStatus.ReadyMask))
            {
                activeSkills[1].OnCast();
            }
            if (Input.GetKeyDown("m") && activeSkills[2].CheckSkillStatus(SkillStatus.ReadyMask))
            {
                activeSkills[2].OnCast();
            }
            if (action == CharacterAction.Idle)
            {
                if (autoAttack.CheckSkillStatus(SkillStatus.ReadyMask))
                {
                    autoAttack.OnCast();
                }
            }
        }
    }
<<<<<<< HEAD

    public override void ReceiveDamage(IBattleHandler attacker, int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            KillCharacter();
        }
        Debug.Log(transform.name + "Received Damage: " + damage);
        UpdateHpUI();
    }
=======
    */
>>>>>>> 96a441a56d03b4f6eda8cbf73eb63b00e7d93ad2
}

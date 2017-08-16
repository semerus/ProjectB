using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Hero
{
    private void Awake()
    {
		/*
        autoAttack = gameObject.AddComponent<Wizard_AutoAttack>();
        //autoAttack.SetSkill(this);

        passiveSkill = gameObject.AddComponent<Wizard_Passive>();
        //passiveSkill.SetSkill(this);
        passiveSkill.OnCast();

		activeSkills = new HeroActive[3];
        activeSkills[0] = gameObject.AddComponent<Wizard_Snowball>();
        activeSkills[1] = gameObject.AddComponent<Wizard_Freeze>();
        activeSkills[2] = gameObject.AddComponent<Wizard_Blizzard>();
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
                if (this.target != null)
                {
                    if (this.target.Team == Team.Hostile && autoAttack.CheckSkillStatus(SkillStatus.ReadyMask))
                    {
                        autoAttack.OnCast();
                    }
                }
            }
        }
    }
    */
}

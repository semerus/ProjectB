using UnityEngine;

public class Fighter : Hero {

	void Awake() {
		// temporary value given
		id = 1;
		team = Team.Friendly;
        status = CharacterStatus.Idle;
        maxHp = 300;
        hp = maxHp;
		speed_x = 2.57f;
		speed_y = 1.4f;
        
        // for Skill debugging
        autoAttack = gameObject.AddComponent<Fighter_Attack>();
        autoAttack.SetSkill(this);

        passiveSkill = gameObject.AddComponent<Fighter_Passive_LifeSteal_AutoAttack>();
        passiveSkill.SetSkill(this);

		activeSkills = new HeroActive[3];
        activeSkills[0] = gameObject.AddComponent<Fighter_MeowPunch_FierceScratch>();
        activeSkills[1] = gameObject.AddComponent<Fighter_CounterStance>();
        activeSkills[2] = gameObject.AddComponent<Fighter_ThousandFists>();
        foreach(Skill eachSkill in activeSkills)
        {
            eachSkill.SetSkill(this);
        }
        
    }

	/*
    public override void AutoAttack(IBattleHandler target)
    {
        this.target = target;
        autoAttack.Activate(target);
    }
    */

    public override void ReceiveDamage(IBattleHandler attacker, int damage)
    {
        if(activeSkills[1].Status == SkillStatus.ChannelingOn)
        {
            (activeSkills[1] as Fighter_CounterStance).ReflectDamage(attacker);

            Debug.Log("CounterAttack Activate");
            UpdateHpUI();
        }
        else
        {
            int receivedDamage = Calculator.ReceiveDamage(this, damage);

            hp -= receivedDamage;
            if (hp <= 0)
            {
                hp = 0;
                KillCharacter();
            }
            UpdateHpUI();
        }

    }
		
	public override void RunTime ()
	{
		base.RunTime ();
		// for debugging skill
		if (Input.GetKeyDown (KeyCode.A))
			activeSkills [0].OnCast ();
		else if (Input.GetKeyDown (KeyCode.S))
			activeSkills [1].OnCast ();
		else if (Input.GetKeyDown (KeyCode.D))
			activeSkills [2].OnCast ();

        //switch(action)
        //{
        //    case CharacterAction.Idle:
        //    case CharacterAction.Moving:
        //        if (this.target != null)
        //        {
        //            if (this.target.Team == Team.Hostile)
        //                AutoAttack(target);
        //        }
        //        break;

        //    default:
        //        break;
        //}
	}
}

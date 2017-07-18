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

        passiveSkill = gameObject.AddComponent<Fighter_Passive>();
        passiveSkill.SetSkill(this);
        passiveSkill.Activate(this);

        activeSkills = new Skill[3];
        activeSkills[0] = gameObject.AddComponent<Fighter_MeowPunch_FierceScratch>();
        activeSkills[1] = gameObject.AddComponent<Fighter_CounterStance>();
        activeSkills[2] = gameObject.AddComponent<Fighter_ThousandFists>();
        foreach(Skill eachSkill in activeSkills)
        {
            eachSkill.SetSkill(this);
        }
        
    }
    
    public override void AutoAttack(IBattleHandler target)
    {
        this.target = target;
        autoAttack.Activate(target);
    }

    public override void ReceiveDamage(IBattleHandler attacker, int damage)
    {
        if(activeSkills[1].State == SkillStatus.ChannelingOn)
        {
            (activeSkills[1] as Fighter_CounterStance).ReflectDamage(attacker);

            Debug.Log("CounterAttack Activate");
            UpdateHpUI();
        }
        else
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

    }

    protected override void Update()
    {
        // for debugging skill
        if (Input.GetKeyDown(KeyCode.A))
            activeSkills[0].Activate(target);
        else if (Input.GetKeyDown(KeyCode.S))
            activeSkills[1].Activate(target);
        else if (Input.GetKeyDown(KeyCode.D))
            activeSkills[2].Activate(target);

        if (status == CharacterStatus.Idle)
        {
            if (this.target != null)
            {
                if (this.target.Team == Team.Hostile)
                    AutoAttack(target);
            }
        }
        else if ((status & CharacterStatus.IsMovingMask) > 0)
        {
            Move(moveTarget);
        }
        else if ((status & CharacterStatus.IsChannelingMask) > 0)
        {
            // Do Channelling Things
            // 0) AutoAttack

            // 1) Hit

            // 2) Meow Attack
            
            // 3) ThousandFist
            if (activeSkills[2].State == SkillStatus.ChannelingOn)
            {
                (activeSkills[2] as Fighter_ThousandFists).TryAttack(target);
            }
        }

    }
}

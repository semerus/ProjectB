using UnityEngine;

public class Fighter : Hero {

	void Awake() {
		// temporary value given
		id = 1;
		team = Team.Friendly;
        status = CharacterStatus.Idle;
        maxHp = 1000;
		hp = 1000;
		speed_x = 2.57f;
		speed_y = 1.4f;

		// temporary spawning -> should be moved to BattleManager
		Spawn();

        // for Skill debugging
        autoAttack = gameObject.AddComponent<Fighter_Attack>();
        autoAttack.SetSkill(this);

        passiveSkill = gameObject.AddComponent<Fighter_Passive>();
        passiveSkill.SetSkill(this);
        passiveSkill.Activate(this);

        activeSkills = new Skill[3];
        activeSkills[0] = gameObject.AddComponent<Fighter_MeowPunch>();
        activeSkills[1] = gameObject.AddComponent<Fighter_CounterStance>();
        activeSkills[2] = gameObject.AddComponent<Fighter_WarCry>();
        foreach(Skill eachSkill in activeSkills)
        {
            eachSkill.SetSkill(this);
        }
        
    }

    public virtual void SetSkill(Skill[] skills)
    {
    }

    public override void AutoAttack(IBattleHandler target)
    {
        this.target = target;
        print("second AutoAttack"); 
        autoAttack.Activate(target);
    }

    protected override void Update()
    {
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
            // Do Channeling Things;
        }
    }
}

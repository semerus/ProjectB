using UnityEngine;

public class Fighter : Hero {
    
	void Awake() {
		// temporary value given
		id = 1;
        status = CharacterStatus.Idle;
        maxHp = 300;
        hp = maxHp;
		speed_x = speed_x_1Value * 2f;
        speed_y = speed_y_1Value * 2f;
        
        // set skill
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
    
    public override void AutoAttack(IBattleHandler target)
    {
        this.target = target;
        autoAttack.Activate(target);
    }
		
    // may adapt to Hero class not this class
	public override void RunTime ()
	{
        // Action State Check -> Status Check
        switch (action) {
        case CharacterAction.Idle:
                if (target != null)
                    AutoAttack(target);
                break;

		case CharacterAction.Moving:
                switch (moveMethod)
                {
                    case MoveMethod.Normal:
                        Move(moveTarget);
                        break;
                    case MoveMethod.CustomSpeed:
                        Move(moveTarget, customSpeed_x, customSpeed_y);
                        break;
                }
                break;

       case CharacterAction.Jumping:
			switch (moveMethod) {
			case MoveMethod.Normal:
				Move(moveTarget);
				break;
			case MoveMethod.CustomSpeed:
				Move(moveTarget, customSpeed_x, customSpeed_y);
				break;
			}
			break;

        case CharacterAction.Channeling:
                // Do channeling Ani
            break;

        case CharacterAction.Attacking:
                // Do Attacking Ani
                break;

        case CharacterAction.Dead:
                // Do Dead Ani
                break;

        default:
                Debug.LogError("CharacterAction Debug Error!");
                break;
        }


		CheckFacing();
	}
}

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

	/*
    public override void AutoAttack(IBattleHandler target)
    {
        this.target = target;
        autoAttack.Activate(target);
    }
<<<<<<< HEAD
=======
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

            //여기에다가 함수를 추가하고 뺄 수는 없을까? -> 해놓고 델리게이트로 바꿔 보도록 합시다!!!
            if (this is Hero)
            {
                foreach (Buff eachbuff in buffs)
                {
                    if (eachbuff is Buff_Link_ProtectionArea)
                    {
                        Buff_Link_ProtectionArea buff = eachbuff as Buff_Link_ProtectionArea;
                        if (buff.helaer_ProtectionArea.LinkerState == LinkerState.OnLink || buff.helaer_ProtectionArea.LinkerState == LinkerState.willBreak)
                        {
                            buff.helaer_ProtectionArea.ReceiveDamage(attacker, receivedDamage);
                            return;
                        }
                        else
                        {
                            buff.EndBuff();
                        }
                    }
                }
            }

            hp -= receivedDamage;
            if (hp <= 0)
            {
                hp = 0;
                KillCharacter();
            }
            UpdateHpUI();
        }

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

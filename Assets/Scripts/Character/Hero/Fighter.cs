using UnityEngine;

public class Fighter : Hero {

    public override void ReceiveDamage (IBattleHandler attacker, int damage)
	{
		if (activeSkills [1].CheckSkillStatus (SkillStatus.ChannelingMask)) {
			IChanneling ch = activeSkills[1] as IChanneling;
			ch.OnInterrupt(attacker);
			return;
		}
		base.ReceiveDamage (attacker, damage);
	}

	/*
    public override void ReceiveDamage(IBattleHandler attacker, int damage)
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
                    if (buff.healer_ProtectionArea.LinkerState == LinkerState.OnLink || buff.healer_ProtectionArea.LinkerState == LinkerState.willBreak)
                    {
                        buff.healer_ProtectionArea.ReceiveDamage(attacker, receivedDamage);
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
	*/
    // may adapt to Hero class not this class
	public override void RunTime ()
	{
		base.RunTime ();
		if (anim.Anim.GetCurrentAnimatorStateInfo (0).IsName ("Idle") && autoAttack.CheckCondition ()) {
			autoAttack.OnCast ();
		}
	}
}

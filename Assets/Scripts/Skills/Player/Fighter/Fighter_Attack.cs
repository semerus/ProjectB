using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_Attack : Skill {
	// effect of this skill
	int dmg;
    #region implemented abstract members of Skill

    public override void Activate (IBattleHandler target)
	{
        Debug.Log("aaaaaaaaaaaaa");
        CheckTargetRange(target);

        if (isTargetInMeleeRange == true)
        {
			if(CheckSkillStatus(SkillStatus.ReadyMask))
            {
				if (caster.CheckCharacterStatus(CharacterStatus.Blind))
                {
                    UpdateSkillStatus(SkillStatus.ProcessOff);

                    StartCoolDown();
                }
                else
                {
                    Debug.Log("sss");
                    UpdateSkillStatus(SkillStatus.ProcessOff);

                    int attackDmg = Calculator.AttackDamage(caster, dmg);
                    target.ReceiveDamage(caster, attackDmg);
                    //LifeStealValue;
                    caster.ReceiveHeal(10);

                    StartCoolDown();
                }
            }
            else
            {
                // waiting onCoolDown;
            }
        }
        else
        {
			caster.ChangeMoveTarget(positionToMeleeAttack);
        }
	}

    #endregion

    #region MonoBehaviours
    void Awake()
    {
        // set original value
        cooldown = 0.9f;
        dmg = 10;

        // set initial value
		skillStatus = SkillStatus.ReadyOn;
        timer_cooldown = 0f;
        isTargetInMeleeRange = false;
        positionToMeleeAttack = new Vector3();
    }

    #endregion

    #region Field&Method

    // Check Melee Range
    bool isTargetInMeleeRange;
    Vector3 positionToMeleeAttack;
    private void CheckTargetRange(IBattleHandler attackTarget)
    {
        // you can change 'as Enemy' to 'as Hero' (or something that has IBattleHandler
        // to get position
        Vector3 enemyPosition = (attackTarget as Enemy).transform.position;

        float myA = 2.1f;
        float myB = 0.7f;

        float outerX = 0.5f;
        float innerX = 0.2f;
        float outerY = 0.3f;

        float dX = enemyPosition.x - this.transform.position.x;
        float dY = enemyPosition.y - this.transform.position.y;

        float inX = (myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + innerX;
        float outX = (myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + outerX;

        float m_inX = -1 * ((myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + innerX);
        float m_outX = -1 * ((myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + outerX);

        float deltaX = enemyPosition.x - caster.transform.position.x;
        float deltaY = enemyPosition.y - caster.transform.position.y;

        if (((-1 * outerY) <= dY && dY <= outerY) && ((inX <= dX && dX <= outX) || (m_outX <= dX && dX <= m_inX)))
        {
            isTargetInMeleeRange = true;
        }
        else
        {
            isTargetInMeleeRange = false;
            if (this.gameObject.transform.position.x <= enemyPosition.x)
            {
                positionToMeleeAttack = enemyPosition - new Vector3(2.3f, 0, 0);
            }
            else
            {
                positionToMeleeAttack = enemyPosition + new Vector3(2.3f, 0, 0);
            }
        }
    }

    #endregion
}

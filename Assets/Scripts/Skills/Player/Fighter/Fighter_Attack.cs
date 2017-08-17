using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_Attack : Skill {

    #region implemented abstract members of Skill

    public override void Activate ()
	{
		Debug.Log ("fighter attacking");

		CheckTargetRange(caster.Target);

        if (isTargetInMeleeRange == true)
        {
			if(CheckSkillStatus(SkillStatus.ReadyMask))
            {
				if (caster.CheckCharacterStatus(CharacterStatus.Blind))
                {
                    UpdateSkillStatus(SkillStatus.ProcessOff);

                    caster.ChangeAction(CharacterAction.Attacking);

                    StartCoolDown();
                }
                else
                {
                    caster.StopMove();
                    caster.ChangeAction(CharacterAction.Attacking);
                    UpdateSkillStatus(SkillStatus.ProcessOff);

                    int attackDmg = Calculator.AttackDamage(caster, dmg);
					caster.Target.ReceiveDamage (caster, attackDmg);
                    caster.ReceiveHeal(10);

                    caster.ChangeAction(CharacterAction.Attacking);
                    
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
		caster = GetComponent<Character> ();
		Hero h = caster as Hero;
		if (h != null) {
			h.autoAttack = this;
		}
        isTargetInMeleeRange = false;
        positionToMeleeAttack = new Vector3();
    }

    #endregion

    #region Field&Method
    // effect of this skill
    int dmg;

    // Check Melee Range
    bool isTargetInMeleeRange;
    Vector3 positionToMeleeAttack;
    private void CheckTargetRange(IBattleHandler attackTarget)
    {
        if(attackTarget is Character)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(caster.transform.position,
                (attackTarget as Character).gameObject.transform.position, LayerMask.NameToLayer("EnemyCollider"));

            if(hits != null)
            {
                
            }
            else
            {
                positionToMeleeAttack = (attackTarget as Character).transform.position;
            }
        }
        //float myA = 2.1f;
        //float myB = 0.7f;

        //float outerX = 0.5f;
        //float innerX = 0.2f;
        //float outerY = 0.3f;

        //float dX = enemyPosition.x - this.transform.position.x;
        //float dY = enemyPosition.y - this.transform.position.y;

        //float inX = (myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + innerX;
        //float outX = (myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + outerX;

        //float m_inX = -1 * ((myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + innerX);
        //float m_outX = -1 * ((myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + outerX);

        //float deltaX = enemyPosition.x - caster.transform.position.x;
        //float deltaY = enemyPosition.y - caster.transform.position.y;

        //if (((-1 * outerY) <= dY && dY <= outerY) && ((inX <= dX && dX <= outX) || (m_outX <= dX && dX <= m_inX)))
        //{
        //    isTargetInMeleeRange = true;
        //}
        //else
        //{
        //    isTargetInMeleeRange = false;
        //}
    }
    #endregion
}

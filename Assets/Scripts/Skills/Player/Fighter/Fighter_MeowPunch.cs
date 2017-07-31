using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter_MeowPunch : HeroActive {
    #region implemented abstract members of Skill

    public override void Activate()
    {
        CheckTargetRange(caster.Target);

        if (isTargetInMeleeRange == true && caster.CurHP > HPCost)
        {
            if (CheckSkillStatus(SkillStatus.ReadyOn))
            {
                Character tarChraracter = caster.Target as Character;

                int attackDmg = Calculator.SkillDamage(caster, dmg);
                tarChraracter.ReceiveDamage(caster, Calculator.SkillDamage(caster, attackDmg));

                TraitBuffCasting(caster, tarChraracter);

                caster.CurHP -= HPCost;
            }
            else
            {
                // waiting onCoolDown;
            }
        }
        else
        {
			caster.BeginMove(positionToMeleeAttack);
        }
    }
    #endregion

    #region TraitChanges
    protected abstract void TraitBuffCasting(Character caster, Character tarCharacter);
    protected abstract void TraitSetValue();
    #endregion

    #region MonoBehaviours
    protected void Awake()
    {
        // set original value
        TraitSetValue();
        
        // set initial value
        skillStatus = SkillStatus.ReadyOn;
        timer_cooldown = 0f;
        isTargetInMeleeRange = false;
        positionToMeleeAttack = new Vector3();

        //UI
		button = Resources.Load<Sprite> ("Skills/Heroes/Fighter/Fighter_Skill1");
    }

    #endregion

    #region Field&Method
    // effect & cost of this Skill
    protected int dmg;
    protected int HPCost;
    
    // Check Melee Range
    bool isTargetInMeleeRange;
    Vector3 positionToMeleeAttack;
    private void CheckTargetRange(IBattleHandler attackTarget)
    {
        // you can change 'as Enemy' to 'as Hero' (or something that has IBattleHandler
        // to get position
        Vector3 enemyPosition = (attackTarget as Enemy).transform.position;
        float deltaX = enemyPosition.x - caster.transform.position.x;
        float deltaY = enemyPosition.y - caster.transform.position.y;

        // you can chage  melee attack range by setting this. 
        float outerX = 0.5f;
        float innerX = 0.3f;
        float outerY = 0.2f;

        float avgX = (outerX + innerX) / 2;
        float halfY = outerY / 2;

        // x>=0 case
        if (deltaX > outerX)
            deltaX -= avgX;
        else if (deltaX > innerX)
            deltaX = 0;
        else if (deltaX >= 0)
            deltaX = -avgX + deltaX;

        // x<0 case
        else if (deltaX < -outerX)
            deltaX += avgX;
        else if (deltaX < -innerX)
            deltaX = 0;
        else if (deltaX < 0)
            deltaX = avgX + deltaX;

        // y case
        if (deltaY > outerY)
            deltaY -= halfY;
        else if (deltaY < -outerY)
            deltaY += halfY;
        else
            deltaY = 0;

        if (deltaX == 0 && deltaY == 0)
            isTargetInMeleeRange = true;
        else
        {
            isTargetInMeleeRange = false;
            positionToMeleeAttack = transform.position + new Vector3(deltaX, deltaY, 0);
        }
    }

    #endregion
}

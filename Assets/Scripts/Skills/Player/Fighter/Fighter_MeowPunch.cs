using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter_MeowPunch : HeroActive {

	IBattleHandler target;
	protected int cost;
	protected int damage;

    #region implemented abstract members of Skill

    public override void Activate()
    {
        caster.StopMove();
		if (caster.ChangeAction (CharacterAction.Active1)) {
			target = caster.Target;
			StartCoolDown ();
            caster.ChangeAction(CharacterAction.Active1);
            caster.Anim.ClearAnimEvent ();
			caster.Anim.onCue += Attack;
            
        } else {
			Debug.Log ("meowpunch failed");
		}

		/*
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
        */
    }
    #endregion

    #region TraitChanges
    protected abstract void TraitBuffCasting(Character caster, Character tarCharacter);
    //protected abstract void TraitSetValue();
    #endregion

    #region MonoBehaviours
    protected void Awake()
    {
		caster = gameObject.GetComponent<Character> ();
		Hero h = caster as Hero;
		if (h != null) {
			h.activeSkills [0] = this;
		}
		/*	
        // set original value
        TraitSetValue();
        
        // set initial value
        skillStatus = SkillStatus.ReadyOn;
        timer_cooldown = 0f;
        isTargetInMeleeRange = false;
        positionToMeleeAttack = new Vector3();
		*/

        //UI
		button = Resources.Load<Sprite> ("Skills/Heroes/Fighter/Fighter_Skill1");
    }

    #endregion

    #region Field&Method

	public override void SetSkill (Dictionary<string, object> param)
	{
		base.SetSkill (param);
		this.damage = (int)param["damage"];
		this.cost = (int)param ["cost"];
	}

	public override bool CheckCondition ()
	{
		bool isReady = false;
		Hero h = caster as Hero;
		MeleeAttack melee = h.autoAttack as MeleeAttack;
		isReady = melee.CheckRange () && caster.CurHP > cost;

		return isReady && base.CheckCondition ();
	}

	protected void Attack() {
		caster.Anim.onCue -= Attack;
		caster.AttackTarget (target, damage);
		// apply buff here
		UpdateSkillStatus (SkillStatus.ProcessOff);
		caster.ChangeAction (CharacterAction.Idle);
		target = null;
	}

	/*
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
	*/
    #endregion
}

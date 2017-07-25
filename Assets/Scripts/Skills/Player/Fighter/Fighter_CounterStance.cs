using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_CounterStance : HeroActive, IChanneling {
    #region implemented abstract members of Skill

	public override void OnCast ()
	{
		// resource check
		if (caster.CurHP <= HPCost)
		{
			print("hp low");
			return;
		}

		// skillState check
		if (!CheckSkillStatus(SkillStatus.ReadyMask))
		{
			print("skill not ready");
			return;
		}

		// Change character State
		if (caster.ChangeAction (CharacterAction.Channeling)) {
			caster.CurHP -= HPCost;
			Debug.Log ("fighter channeling  1213");
			// Change skill State
			UpdateSkillStatus(SkillStatus.ChannelingOn);

			// Time system Check
			TimeSystem.GetTimeSystem ().AddTimer (this);

		} else {
			// fails to channel
		}
		base.OnCast ();
	}

    public override void Activate(IBattleHandler target)
    {
		
    }

    #endregion

    #region implemented IChanneling
    public float ChannelTime
    {
        get { return channelTime; }
    }
    public float Timer_Channeling
    {
        get { return timer_Channeling; }
        set { timer_Channeling = value; }
    }

    public void OnChanneling()
    {
		Debug.Log ("fighter channeling");
        timer_Channeling += Time.deltaTime;

        // skill fail
        if(timer_Channeling > countableTime)
        {
            //time check
            timer_Channeling = 0f;
            timer_cooldown = 0f;
            StartCoolDown();

            //character status
			caster.ChangeAction(CharacterAction.Idle);
			UpdateSkillStatus (SkillStatus.ChannelingOff);
        }
    }

	public void OnInterrupt(IBattleHandler interrupter)
    {
		Debug.Log ("Stance interrupted");
		UpdateSkillStatus (SkillStatus.ChannelingOff);
		ReflectDamage (interrupter);
    }
    #endregion

    #region MonoBehaviours
    void Awake()
    {
        // set original value
        cooldown = 20f;
        dmg = 250;
        HPCost = 50;
        countableTime = 5f; // for check easily

        // set initial value
		skillStatus = SkillStatus.ReadyOn;
        timer_cooldown = 0f;
        Timer_Channeling = 0f;
        isTargetInMeleeRange = false;
        positionToMeleeAttack = new Vector3();

		button = Resources.Load<Sprite> ("Skills/Heroes/Fighter/Fighter_Skill2");
    }

    #endregion

    #region Field&Method

    // effect & cost of this Skill
    int dmg;
    int HPCost;

    float countableTime;

    float channelTime;
    float timer_Channeling;

    // Reflect Damage
    public void ReflectDamage(IBattleHandler attacker)
    {
        // check melee range check
        if(CheckTargetRange(attacker) == true)
        {
            // effect
            (attacker as Character).ReceiveDamage(caster, dmg);
            // characterState
            print("damage reflected!");
        }
		caster.ChangeAction(CharacterStatus.Idle);
		timer_cooldown = 0f;
		StartCoolDown();
    }

    // Check Melee Range
    bool isTargetInMeleeRange;
    Vector3 positionToMeleeAttack;
    public bool CheckTargetRange(IBattleHandler attackTarget)
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

        return isTargetInMeleeRange;
    }

    #endregion

}

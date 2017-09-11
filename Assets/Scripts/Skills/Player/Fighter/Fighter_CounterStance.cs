using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_CounterStance : HeroActive, IChanneling {

	protected int damage;
	protected int cost;
	protected float channelTime;
	protected float timer_Channeling = 0f;
	protected IBattleHandler interrupter;


    #region implemented abstract members of Skill
    public override void Activate()
    {
		caster.ChangeAction (CharacterAction.Active2);
		caster.Anim.ClearAnimEvent ();
		caster.Anim.onCue += ReflectDamage;
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
        timer_Channeling += Time.deltaTime;

        // skill fail
        if(timer_Channeling > channelTime)
        {
            //time check
            timer_Channeling = 0f;
            StartCoolDown();

            //character status
			caster.ChangeAction(CharacterAction.Idle);
			UpdateSkillStatus (SkillStatus.ChannelingOff);
        }
    }

	public void OnInterrupt(IBattleHandler interrupter)
    {
		if (interrupter != null) {
			timer_Channeling = 0f;
			UpdateSkillStatus (SkillStatus.ChannelingOff);
			StartCoolDown();
			this.interrupter = interrupter;
			Activate ();
		}
    }
    #endregion

    #region TraitChanges
    //protected abstract void TraitBuffCasting(Character caster, Character tarCharacter);
    //protected abstract void TraitSetValue();
    #endregion

    #region MonoBehaviours
    void Awake()
    {
		caster = gameObject.GetComponent<Character> ();
		Hero h = caster as Hero;
		if (h != null) {
			h.activeSkills [1] = this;
		}

		/*
        dmg = 250;
        HPCost = 50;
        channelTime = 5f; 

        // set initial value
		skillStatus = SkillStatus.ReadyOn;
        timer_cooldown = 0f;
        Timer_Channeling = 0f;
        isTargetInMeleeRange = false;
        positionToMeleeAttack = new Vector3();
		*/

		button = Resources.Load<Sprite> ("Skills/Heroes/Fighter/Fighter_Skill2");
    }

    #endregion

    #region Field&Method

	public override void SetSkill (Dictionary<string, object> param)
	{
		base.SetSkill (param);
		this.damage = (int)param ["damage"];
		this.cost = (int)param ["cost"];
		this.channelTime = (float)((double)param ["channel_time"]);
	}
   

    // Reflect Damage
    public void ReflectDamage()
    {
		(interrupter as Character).ReceiveDamage(caster, damage);
        caster.ChangeAction(CharacterStatus.Idle);
		caster.Anim.onCue -= ReflectDamage;
    }

	/*
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
	*/
    #endregion

}

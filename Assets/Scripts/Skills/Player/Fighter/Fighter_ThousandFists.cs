using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_ThousandFists : HeroActive {

	protected int damage;
	protected int finalDamage;
	protected int hitCount = 0;
	protected int cost;
	IBattleHandler target;
	protected float movedLength = 0f;
	protected float speed = 20f;

    #region implemented abstract members of Skill + override things

    public override void Activate()
    {
		if (caster.ChangeAction (CharacterAction.Active3)) {
			target = caster.Target;
			StartCoolDown ();
			//UpdateSkillStatus (SkillStatus.ProcessOn);
			caster.Anim.ClearAnimEvent ();
			caster.Anim.onCue += Attack;
		} else {
			Debug.Log ("thousandpunch failed");
		}

		/*
        // resource check
        if (caster.CurHP <= HPCost)
        {
            print("hp row");
            return;
        }

        // skillState check
		if (skillStatus != SkillStatus.ReadyOn)
        {
            print("skill not ready");
            return;
        }
        // should refine!!
        
        
		CheckTargetRange(caster.Target);

        if(isTargetInMeleeRange == true)
        {
            // Change character State
			caster.ChangeAction(CharacterAction.Channeling);
            //caster.CurHP -= HPCost;

            // Change skill State
			skillStatus = SkillStatus.ChannelingOn;

            // Time system Check
            TimeSystem.GetTimeSystem().AddTimer(this);
        }
        else
        {
            Debug.LogError("isTargetInMeleeRange is " + isTargetInMeleeRange);
        }
		*/
    }

	/*
    public void TryAttack(IBattleHandler target)
    {
        if(curHitCoutns < 6 && timer_Channeling > delayBetweenNormal)
        {
            curHitCoutns++;
            timer_Channeling = 0f;
            // animation state
        
            caster.AttackTarget(target, normalDmg);
            print("nor punch!!");
        }
        else if(curHitCoutns == 6 && timer_Channeling > delayBeforeLast)
        {
            // skill inspect
            curHitCoutns++;
            timer_Channeling = 0f;
        
            caster.AttackTarget(target, lastDmg);
            print("last punch!");
            
			skillStatus = SkillStatus.OnCoolDownOn;

            // character inspect
			caster.ChangeAction(CharacterAction.Idle);
            // animation state
        }
        else
        {
            // do nothing
        }
    }
    */
	/*
    protected override void OnCoolDown()
    {
        timer_cooldown += Time.deltaTime;

        if (timer_cooldown >= cooldown)
        {
            // reset chanlling things
            curHitCoutns = 0;
            timer_Channeling = 0;

			skillStatus = SkillStatus.ReadyOn;
            timer_cooldown = 0f;
            TimeSystem.GetTimeSystem().DeleteTimer(this as ITimeHandler);
        }
    }
	*/
    #endregion
    
    #region implemented IChanneling
	/*
    public float ChannelTime
    {
        get { return channelTime; }
    }
    public float Timer_Channeling
    {
        get { return timer_Channeling; }
        set { timer_Channeling = value; }
    }
    
    public void ResetChannelingValue()
    {
        curHitCoutns = 0;
        Timer_Channeling = 0;
    }

    public void OnChanneling()
    {
		UpdateSkillStatus (SkillStatus.ChannelingOn);
        timer_Channeling += Time.deltaTime;
    }

	public void OnInterrupt(IBattleHandler interrupter)
    {
        Debug.LogError("not realized yet");
    }
    */
    #endregion

    #region MonoBehaviours
    void Awake()
    {
		caster = gameObject.GetComponent<Character> ();
		Hero h = caster as Hero;
		if (h != null) {
			h.activeSkills [2] = this;
		}
        /*
		// set original value
        normalDmg = 20;
        lastDmg = 50;
        HPCost = 30;

        hitCounts = 7;

        delayBetweenNormal = 0.15f;
        delayBeforeLast = 0.5f;
        

        // set Timer value
        skillStatus = SkillStatus.ReadyOn;
        timer_cooldown = 0f;
        timer_Channeling = 0f;
        isTargetInMeleeRange = false;
        positionToMeleeAttack = new Vector3();
		*/
		button = Resources.Load<Sprite> ("Skills/Heroes/Fighter/Fighter_Skill3");
    }

    #endregion

    #region Field&Method
    
	public override void SetSkill (Dictionary<string, object> param)
	{
		base.SetSkill (param);
		this.damage = (int)param ["damage"];
		this.finalDamage = (int)param ["final_damage"];
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

	protected override void OnProcess ()
	{
		PushTarget ();
	}

	protected void Attack() {
		if (hitCount < 6) {
			hitCount++;
			caster.AttackTarget (target, damage);
		} else {
			caster.AttackTarget (target, finalDamage);
			hitCount = 0;
			UpdateSkillStatus (SkillStatus.ProcessOn);
			caster.Anim.onCue -= Attack;
		}
	}

	protected void PushTarget() {
		Character c = target as Character;

		if (c != null) {
			if (c.transform.position.x > caster.transform.position.x) {
				// move right
				c.transform.position += new Vector3(speed, 0f, 0f) * Time.deltaTime;
			} else {
				// move left
				c.transform.position -= new Vector3(speed, 0f, 0f) * Time.deltaTime;
			}
			movedLength += speed * Time.deltaTime;
		}

		if (movedLength > 3f || !Background.GetBackground().CheckBoundaries(c.transform.position)) {
			movedLength = 0f;
			UpdateSkillStatus (SkillStatus.ProcessOff);
			caster.ChangeAction (CharacterAction.Idle);
		}
	}

	/*
    // effect & cost of this Skill
    int normalDmg;
    int lastDmg;
    int HPCost;

    int hitCounts;
    int curHitCoutns;
    
    float delayBetweenNormal;
    float delayBeforeLast;

    float channelTime;
    float timer_Channeling;

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

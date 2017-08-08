using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IBattleHandler, ITimeHandler {

	protected int id;
	protected Team team;
	protected int maxHp;
    
	[SerializeField]
	protected int hp;
	protected float speed_x;
	protected float speed_y;
    protected float customSpeed_x;
    protected float customSpeed_y;

    [SerializeField]
	protected List<Buff> buffs = new List<Buff>();
	protected Skill[] skills = new Skill[0];

    [SerializeField]
	protected CharacterAction action = CharacterAction.Idle;
	protected int status = CharacterStatus.Idle;
    [SerializeField]
    protected MoveMethod moveMethod = MoveMethod.Normal;

    [SerializeField]
	protected IBattleHandler target;
    protected Vector3 moveTarget;
	protected bool isFacingLeft = true;
	protected AnimationController anim;

	public event EventHandler<MoveEventArgs> MoveComplete;

	#region Getters and Setters

	public List<Buff> Buffs {
		get {
			return buffs;
		}
	}

    public bool IsFacingLeft
    {
        get { return isFacingLeft; }
    }

	public Skill[] Skills {
		get {
			return skills;
		}
	}

	public IBattleHandler Target {
		get {
			return target;
		}
        set
        {
            target = value;
        }
	}

	public AnimationController Anim {
		get {
			return anim;
		}
	}

    public int CurHP
    {
        get { return hp; }
        set { hp = value; }
    }
    #endregion

    #region IBattleHandler implementation

    public Team Team {
		get {
			return this.team;
		}
	}

	public CharacterAction Action {
		get {
			return this.action;
		}
	}

	public virtual void ReceiveDamage (IBattleHandler attacker, int damage)
	{
        if(skills != null)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                IChanneling ch = skills[i] as IChanneling;
                if (ch != null && SkillStatus.CheckStatus(skills[i].Status, SkillStatus.ChannelingMask))
                {
                    ch.OnInterrupt(attacker);
                }
            }
        }

        int receivedDamage = Calculator.ReceiveDamage(this, damage);

        //여기에다가 함수를 추가하고 뺄 수는 없을까? -> 해놓고 델리게이트로 바꿔 보도록 합시다!!!
        if(this is Hero)
        {
            foreach(Buff eachbuff in buffs)
            {
                if(eachbuff is Buff_Link_ProtectionArea)
                {
                    Buff_Link_ProtectionArea buff = eachbuff as Buff_Link_ProtectionArea;
                    if(buff.helaer_ProtectionArea.LinkerState == LinkerState.OnLink || buff.helaer_ProtectionArea.LinkerState == LinkerState.willBreak)
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
        if (hp <= 0) {
			hp = 0;
			KillCharacter ();
		}
		UpdateHpUI ();
	}

	public virtual void ReceiveHeal (int heal)
	{
		if (action == CharacterAction.Dead)
			return;
		hp += heal;
		if (hp >= maxHp) {
			hp = maxHp;
		}
		UpdateHpUI ();
	}

    #endregion

    #region Debugging Field & Method
    public List<string> BuffList;
    public void UpdateBuffList()
    {
        if(buffs != null)
        {
            BuffList.Clear();
            foreach(Buff eachBuff in buffs)
            {
                BuffList.Add(eachBuff.BuffName);
            }
        }
    }
    #endregion

	#region ITimeHandler implementation

	public virtual void RunTime ()
	{
        CheckFacing();
        switch (action) {
		case CharacterAction.Moving:
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
		}
	}

	#endregion

	protected virtual void OnMoveComplete(MoveEventArgs e) {
		EventHandler<MoveEventArgs> moveComplete = MoveComplete;
		if (moveComplete != null) {
			moveComplete (this, e);
		}
	}

	protected virtual void Start() {
		Spawn ();
	}

	/// <summary>
	/// Things to happen at load
	/// </summary>
	public virtual void Spawn () {
		// set team
		if (this is Hero) {
			team = Team.Friendly;
		} else if (this is Enemy) {
			team = Team.Hostile;
		}

		// add to entity list
		BattleManager.GetBattleManager ().AddEntity (this as IBattleHandler);
		TimeSystem.GetTimeSystem ().AddTimer (this);

		// initialize animation status
		anim = GetComponentInChildren<AnimationController> ();
		anim.UpdateSortingLayer ();
		RefreshBuff ();
	}

	/// <summary>
	/// Refreshes the buff (please run this after each add buff / delete buff)
	/// </summary>
	public void RefreshBuff() {
		int change = CharacterStatus.Idle;
		for (int i = 0; i < buffs.Count; i++) {
            IStatusBuff buff = buffs [i] as IStatusBuff;
			if (buff != null) {
				change |= buff.Status;
			}
		}
		status = change;

		// immune
		/*
		if(CheckCharacterStatus(CharacterStatus.IsImmuneMask)) {
		// turn off all negative buffs	
		}
		*/
		// on silence
		if (CheckCharacterStatus (CharacterStatus.IsSilencedMask)) {
			for (int i = 0; i < skills.Length; i++) {
				IChanneling ch = skills [i] as IChanneling;
				if (ch != null) {
					ch.OnInterrupt (null);
				}
			}
		}
		// on rooted
		if (CheckCharacterStatus (CharacterStatus.IsRootedMask)) {
			if (action == CharacterAction.Moving || action == CharacterAction.Jumping) {
				StopMove ();
			}
		}
		UpdateCCUI ();
	}

	// update hpBar for each character
	protected abstract void UpdateCCUI();
	protected abstract void UpdateHpUI ();

	public virtual void AttackTarget(IBattleHandler target, int damage) {
        int returnDmg = damage;

        if(this is Hero)
            // Switch if it is Skill, Attack
            returnDmg = Calculator.AttackDamage(this, returnDmg);
        else if(this is Enemy)
            returnDmg = Calculator.AllDamage(this, returnDmg);
        
        target.ReceiveDamage(this,returnDmg);
    }

	public virtual void HealTarget(int heal, IBattleHandler target) {
		target.ReceiveHeal (heal);
	}

	protected virtual void KillCharacter () {
        //for time System Off
        //1) remove Buff from timer
        if (buffs != null)
        {
            foreach(Buff eachBuff in buffs)
            {
                ITimeHandler eachBuffTime = eachBuff as ITimeHandler;
                if (eachBuffTime != null)
                    TimeSystem.GetTimeSystem().DeleteTimer(eachBuffTime);
            }
        }
        //2) remove Character from timer
        Debug.LogWarning("Check this with Dead Animation");
        if (TimeSystem.GetTimeSystem().CheckTimer(this) == true)
            TimeSystem.GetTimeSystem().DeleteTimer(this);

		gameObject.SetActive (false);
		ChangeAction (CharacterAction.Dead);
		TimeSystem.GetTimeSystem().DeleteTimer(this);

		// BattleManager check
		BattleManager.GetBattleManager ().CheckGame ();
	}

	/// <summary>
	/// Begins the move at normal speed
	/// </summary>
	/// <returns><c>true</c>, if move was begun, <c>false</c> otherwise.</returns>
	/// <param name="target">Target.</param>
	public bool BeginMove(Vector3 target) {
		if (ChangeAction (CharacterAction.Moving)) {
			moveMethod = MoveMethod.Normal;
			Move (target);
			return true;
		} else
			return false;
	}

	/// <summary>
	/// Begins the move at custom speed
	/// </summary>
	/// <returns><c>true</c>, if move was begun, <c>false</c> otherwise.</returns>
	/// <param name="target">Target.</param>
	/// <param name="speed_x">Speed x.</param>
	/// <param name="speed_y">Speed y.</param>
	public bool BeginMove(Vector3 target, float speed_x, float speed_y) {
		if (ChangeAction (CharacterAction.Moving)) {
			moveMethod = MoveMethod.CustomSpeed;
			Move (target, speed_x, speed_y);
			return true;
		} else
			return false;
	}

	public bool BeginJumpTarget(Vector3 target, float speed_x, float speed_y) {
		if (ChangeAction (CharacterAction.Jumping)) {
			moveMethod = MoveMethod.CustomSpeed;
			Move (target, speed_x, speed_y);
			return true;
		} else
			return false;
	}

	protected void Move (Vector3 target) {
		Move (target, this.speed_x, this.speed_y);
	}

	protected void Move(Vector3 target, float speed_x, float speed_y) {
		float speed;
        customSpeed_x = speed_x;
        customSpeed_y = speed_y;
		moveTarget = target;

		/*
		if (!Background.GetBackground ().CheckBoundaries (target)) {
			Debug.LogError ("Moved outside boundaries");
			ChangeAction (CharacterAction.Idle);
			//MoveEventArgs x = new MoveEventArgs (false, transform.position);
			//OnMoveComplete (x);
			return;
		}
		*/

		// calculate speed
		Vector3 n = Vector3.Normalize(target - transform.position);
		speed = speed_x * Mathf.Sqrt(n.x * n.x / (n.x * n.x + n.y * n.y)) + speed_y * Mathf.Sqrt(n.y * n.y / (n.x * n.x + n.y * n.y));
        speed *= Calculator.MoveSpeed(this);


		if (Vector3.Distance (target, transform.position) > 0.01f) {
			transform.position = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime);
		} else{
			moveTarget = transform.position;
			ChangeAction (CharacterAction.Idle);

			// send move complete(reached destination event)
			MoveEventArgs e = new MoveEventArgs (true, transform.position);
			OnMoveComplete (e);
        }
	}

    public void CheckFacing()
    {
		Vector3 movePosition = moveTarget;
        switch(team)
        {
            case Team.Friendly:
                if (target != null)
                {
                    Character t = target as Character;
                    Vector3 targetPosition = t.transform.position;

                    if (transform.position.x - targetPosition.x >= 0f)
                    {
                        isFacingLeft = true;
                        anim.UpdateFacing(isFacingLeft);
                    }
                    else
                    {
                        isFacingLeft = false;
                        anim.UpdateFacing(isFacingLeft);
                    }
                }
                else
                {
                    if (transform.position.x - moveTarget.x > 0f)
                    {
                        isFacingLeft = true;
                        anim.UpdateFacing(isFacingLeft);
                    }
                    else if (transform.position.x - moveTarget.x == 0f)
                    {
                        anim.UpdateFacing(isFacingLeft);
                    }
                    else
                    {
                        isFacingLeft = false;
                        anim.UpdateFacing(isFacingLeft);
                    }
                }
                break;

            case Team.Hostile:
                if (target != null&&action==CharacterAction.Idle)
                {
                    Character t = target as Character;
                    Vector3 targetPosition = t.transform.position;

                    if (transform.position.x - targetPosition.x >= 0f)
                    {
                        isFacingLeft = true;
                        anim.UpdateFacing(isFacingLeft);
                    }
                    else
                    {
                        isFacingLeft = false;
                        anim.UpdateFacing(isFacingLeft);
                    }
                }
                else
                {
                    if (transform.position.x - moveTarget.x > 0f)
                    {
                        isFacingLeft = true;
                        anim.UpdateFacing(isFacingLeft);
                    }
                    else if (transform.position.x - moveTarget.x == 0f)
                    {
                        anim.UpdateFacing(isFacingLeft);
                    }
                    else
                    {
                        isFacingLeft = false;
                        anim.UpdateFacing(isFacingLeft);
                    }
                }
                break;
        }
        anim.UpdateSortingLayer();
    }

    public bool ChangeMoveTarget(Vector3 target)
    {
		if (action == CharacterAction.Moving) {
			moveTarget = target;
			return true;
		} else if (ChangeAction (CharacterAction.Moving)) {
			BeginMove (target);
			return true;
		}
		return false;
    }

	public void StopMove() {
		ChangeAction (CharacterStatus.Idle);
		MoveEventArgs e = new MoveEventArgs (false, transform.position);
		OnMoveComplete (e);
	}

	public bool ChangeAction(CharacterAction action) {
		// changed successfully return true
		// else return false
		switch (action) {
		case CharacterAction.Idle:
		case CharacterAction.Attacking:
		case CharacterAction.Dead:
			break;
		case CharacterAction.Jumping:
		case CharacterAction.Moving:
			if (CheckCharacterStatus (CharacterStatus.IsRootedMask))
				return false;
			break;
		case CharacterAction.Channeling:
			if (CheckCharacterStatus (CharacterStatus.IsSilencedMask))
				return false;
			break;
		}

		this.action = action;
		this.anim.UpdateAnimation ();
		return true;
	}

	public bool CheckCharacterStatus(int mask) {
		if ((status & mask) > 0)
			return true;
		else
			return false;
	}
}

public enum MoveMethod
{
    Normal,
    CustomSpeed
}
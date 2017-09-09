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
	protected Skill[] skills;

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
        set
        {
            isFacingLeft = value;
            //CheckFacing();
        }
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
                    if(buff.healer_ProtectionArea.LinkerState == LinkerState.OnLink || buff.healer_ProtectionArea.LinkerState == LinkerState.willBreak)
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

	#region Movement

	protected virtual void OnMoveComplete(MoveEventArgs e) {
		EventHandler<MoveEventArgs> moveComplete = MoveComplete;
		if (moveComplete != null) {
			moveComplete (this, e);
		}
	}

	/// <summary>
	/// Begins the move at normal speed
	/// </summary>
	/// <returns><c>true</c>, if move was begun, <c>false</c> otherwise.</returns>
	/// <param name="target">Target.</param>
	public bool BeginMove(Vector3 target) {
        if (!CheckBoundary(target))
        {
            target = SetAdjustPosition(target);
        }
            if (ChangeAction(CharacterAction.Moving))
            {
                moveMethod = MoveMethod.Normal;
                Move(target);
                return true;
            }
            else
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
        if (!CheckBoundary(target))
        {
            target = SetAdjustPosition(target);
        }
        if (ChangeAction(CharacterAction.Moving))
            {
                moveMethod = MoveMethod.CustomSpeed;
                Move(target, speed_x, speed_y);
                return true;
            }
            else
                return false;
	}

	public bool BeginJumpTarget(Vector3 target, float speed_x, float speed_y) {
        if (!CheckBoundary(target))
        {
            SetAdjustPosition(target);
        }
        if (ChangeAction (CharacterAction.Jumping)) {
			moveMethod = MoveMethod.CustomSpeed;
            ChangeAction(CharacterAction.Jumping);
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

    public Vector3 SetAdjustPosition(Vector3 target) {
        BoxCollider2D controlArea = GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>();
        float lx = controlArea.size.x/ 2 + controlArea.offset.x;
        float mlx = controlArea.offset.x - controlArea.size.x / 2;
        float ly = controlArea.size.y / 2 + controlArea.offset.y;
        float mly = controlArea.offset.y - controlArea.size.y / 2;

        if (target.x >= lx)
        {
            target.x = lx - 0.2f;
        }
        if(target.x <= mlx)
        {
            target.x = mlx +0.2f;
        }
        if(target.y >=ly)
        {
            target.y = ly - 0.2f;
        }
        if(target.y <= mly)
        {
            target.y = mly + 0.2f;
        }
        return target;
    }

    public bool CheckBoundary(Vector3 target)
    {
        Vector3 position = target;
        float cpx = position.x;
        float cpy = position.y;
        float lx = GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().size.x / 2;
        float mlx = GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().offset.x - (GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().size.x / 2);
        float ly = (GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().size.y / 2) + GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().offset.y;
        float mly = GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().offset.y - (GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().size.y / 2);

        if ((cpx < lx && cpx > mly) && (cpy < ly && cpy > mly))
        {
            return true;
        }
        else
        {
            return false;
        }
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

	#endregion

	/// <summary>
	/// Things to happen at load
	/// </summary>
	public virtual void Spawn (Dictionary<string, object> data, int[] skills) {
		// apply paramters
		id = (int)data ["id"];
		transform.name = data["name"].ToString();
		maxHp = (int)data ["max_hp"];
		hp = maxHp;
		double[] speed = (double[])data ["speed"];
		speed_x = (float)speed [0];
		speed_y = (float)speed [1];

		this.skills = new Skill[skills.Length];

		// apply skills
		for (int i = 0; i < skills.Length; i++) {
			Dictionary<string, object> param = (Dictionary<string, object>)LoadManager.Instance.SkillData [skills [i]];
			Type t = Type.GetType (param ["name_script"].ToString ());
			Skill skill = gameObject.AddComponent (t) as Skill;
			if (skill != null) {
				skill.SetSkill (param);
			}
			this.skills [i] = skill;
		}

		// add to entity list
		BattleManager.GetBattleManager ().AddEntity (this);
		TimeSystem.GetTimeSystem ().AddTimer (this);

		// initialize animation status
		action = CharacterAction.Idle;
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

	public void UseHp(int cost) {
		hp -= cost;
	}

	public virtual void AttackTarget(IBattleHandler target, int damage) {
        int returnDmg = damage;

        if(this is Hero)
            // Switch if it is Skill, Attack
            returnDmg = Calculator.AttackDamage(this, returnDmg);
        else if(this is Enemy)
            returnDmg = Calculator.AllDamage(this, returnDmg);
        
        target.ReceiveDamage(this, returnDmg);
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

		//gameObject.SetActive (false);
		ChangeAction (CharacterAction.Dead);
		anim.onCue += DisposeBody;
		TimeSystem.GetTimeSystem().DeleteTimer(this);

		// BattleManager check
		BattleManager.GetBattleManager ().CheckGame ();
	}

	private void DisposeBody() {
		anim.onCue -= DisposeBody;
		gameObject.SetActive (false);
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
		case CharacterAction.Active1:
		case CharacterAction.Active2:
		case CharacterAction.Active3:
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
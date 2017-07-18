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

	protected List<Buff> buffs = new List<Buff>();
	protected Skill[] skills;

    [SerializeField]
	protected CharacterAction action = CharacterAction.Idle;
	protected int status = CharacterStatus.Idle;
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

	public Skill[] Skills {
		get {
			return skills;
		}
	}

	public IBattleHandler Target {
		get {
			return target;
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
		hp -= damage;
		if (hp <= 0) {
			hp = 0;
			KillCharacter ();
		}
		Debug.Log (transform.name + "Received Damage: " + damage);
		UpdateHpUI ();
	}

	public virtual void ReceiveHeal (int heal)
	{
		hp += heal;
		if (hp >= maxHp) {
			hp = maxHp;
		}
		UpdateHpUI ();
	}

	#endregion

	#region ITimeHandler implementation

	public virtual void RunTime ()
	{
		switch(action) {
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

		// on silence

		// on rooted
	}

	// update hpBar for each character
	protected abstract void UpdateHpUI ();

	public void AttackTarget(IBattleHandler target, int damage) {
		// check for buffs
		float dmgRatio = 1f;
		float lifeStealRatio = 0f;
        float lifeStealAbs = 0f;
        float lifeStealSum = 0f;

		for (int i = 0; i < buffs.Count; i++) {
            IDamageBuff buff = buffs [i] as IDamageBuff;
			if (buff != null) {
				// change this formula later
				dmgRatio = dmgRatio * buff.DamageRatio;
			}
		}

        for (int i = 0; i < buffs.Count; i++)
        {
            ILifeStealAbsBuff buff = buffs[i] as ILifeStealAbsBuff;
            if (buff != null)
            {
                lifeStealAbs += buff.LifeStealAbs;
            }
    
        }

        lifeStealSum = (damage * dmgRatio * lifeStealRatio) + lifeStealAbs;

		target.ReceiveDamage(this, (int) (damage * dmgRatio));
        target.ReceiveHeal((int)lifeStealSum);
	}

	public virtual void HealTarget(int heal, IBattleHandler target) {
		// check for buffs
		float ratio = 1f;

		// add heal ratio

		target.ReceiveHeal ((int) (heal * ratio));
	}

	protected virtual void KillCharacter () {
		
		gameObject.SetActive (false);
		ChangeAction (CharacterAction.Dead);

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

		// calculate speed
		Vector3 n = Vector3.Normalize(target - transform.position);
		speed = speed_x * Mathf.Sqrt(n.x * n.x / (n.x * n.x + n.y * n.y)) + speed_y * Mathf.Sqrt(n.y * n.y / (n.x * n.x + n.y * n.y));
		// i added Mathf.sqrt because calculation doesn't match. think about deltaX,speedX = 3, deltaY,speedY=4  (3,4,5 triangle)

		if (Vector3.Distance (target, transform.position) > 0.01f) {
			transform.position = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime);
		} else{
			moveTarget = transform.position;
			ChangeAction (CharacterAction.Idle);

			// send move complete(reached destination event)
			MoveEventArgs e = new MoveEventArgs (true, transform.position);
			OnMoveComplete (e);
		}

		// update sorting layer order by y axis
		anim.UpdateSortingLayer ();
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

	protected bool CheckCharacterStatus(int mask) {
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
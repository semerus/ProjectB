using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IBattleHandler {

	protected int id;
    [SerializeField]
	protected Team team;
	protected int maxHp;
	protected int hp;
	protected float speed_x;
	protected float speed_y;
    protected float customSpeed_x;
    protected float customSpeed_y;


	protected List<Buff> buffs = new List<Buff>();

	protected int status = CharacterStatus.Idle;
    protected MoveMethod moveMethod = MoveMethod.Normal;

    [SerializeField]
    protected CharacterState state;
    // may input interface queueAble
    public CharacterState queueState;
    
	protected IBattleHandler target;
    protected Vector3 moveTarget;

	public event EventHandler<MoveEventArgs> MoveComplete;

	#region Getters and Setters

	public List<Buff> Buffs {
		get {
			return buffs;
		}
	}

	public IBattleHandler Target {
		get {
			return target;
		}
	}

	public int Status {
		get {
			return status;
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

	public CharacterState State {
		get {
			return this.state;
		}
        set { this.state = value; }
	}

	public void ReceiveDamage (int damage)
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
		//UpdateHpUI ();
	}

	#endregion

	public void OnMoveComplete(MoveEventArgs e) {
		EventHandler<MoveEventArgs> moveComplete = MoveComplete;
		if (moveComplete != null) {
			moveComplete (this, e);
		}
	}

	protected virtual void Update() {
        switch (moveMethod)
        {
            case MoveMethod.Normal:
                if ((status & CharacterStatus.IsMovingMask) > 0)
                {
                    Move(moveTarget);
                }
                break;
            case MoveMethod.CustomSpeed:
                if ((status & CharacterStatus.IsMovingMask) > 0)
                {
                    Move(moveTarget, customSpeed_x, customSpeed_y);
                }
                break;
            default:
                break;
        }
	}

	/// <summary>
	/// Refreshs the status.
	/// </summary>
	/// <param name="newAction">New action(Idle, Moving, Channeling)</param>
	public virtual void RefreshStatus(int newAction) {
		for (int i = 0; i < buffs.Count; i++) {
			IStatusBuff buff = buffs [i] as IStatusBuff;
			if (buff != null) {
				newAction = newAction | buff.Status;
			}
		}
		status = newAction;
	}

	// update hpBar for each character
	protected abstract void UpdateHpUI ();

	public void AttackTarget(IBattleHandler target, int damage) {
		// check for buffs
		float dmgRatio = 1f;
		float lifeStealRatio = 1f;

		for (int i = 0; i < buffs.Count; i++) {
			IDamageBuff buff = buffs [i] as IDamageBuff;
			if (buff != null) {
				// change this formula later
				dmgRatio = dmgRatio * buff.DamageRatio;
			}
		}

		// add life steal later

		target.ReceiveDamage ((int) (damage * dmgRatio));
	}

	public virtual void HealTarget(int heal, IBattleHandler target) {
		// check for buffs
		float ratio = 1f;

		// add heal ratio

		target.ReceiveHeal ((int) (heal * ratio));
	}

	public virtual void Spawn () {
		// things to happen at load
		BattleManager.GetBattleManager ().AddEntity (this as IBattleHandler);

		// place at the correct place
	}

	protected virtual void KillCharacter () {
		state = CharacterState.Dead;

		// BattleManager check
		BattleManager.GetBattleManager ().CheckGame ();
	}

	public void Move (Vector3 target) {
        // order matters
		Move (target, this.speed_x, this.speed_y);
        moveMethod = MoveMethod.Normal;
	}

	public void Move(Vector3 target, float speed_x, float speed_y) {
		float speed;

        customSpeed_x = speed_x;
        customSpeed_y = speed_y;
        moveMethod = MoveMethod.CustomSpeed;

		// do not move when rooted
		if ((status & CharacterStatus.IsRootedMask) > 0) {
			RefreshStatus (CharacterStatus.Idle);
			return;
		}

		RefreshStatus (CharacterStatus.Moving);
		moveTarget = target;

		// calculate speed
		Vector3 n = Vector3.Normalize(target - transform.position);
		speed = speed_x * Mathf.Sqrt(n.x * n.x / (n.x * n.x + n.y * n.y)) + speed_y * Mathf.Sqrt(n.y * n.y / (n.x * n.x + n.y * n.y));
		// i added Mathf.sqrt because calculation doesn't match. think about deltaX,speedX = 3, deltaY,speedY=4  (3,4,5 triangle)

		if (Vector3.Distance (target, transform.position) > 0.01f) {
			transform.position = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime);
		} else{
			moveTarget = transform.position;
			RefreshStatus (CharacterStatus.Idle);

			// send move complete(reached destination event)
			MoveEventArgs e = new MoveEventArgs (true, transform.position);
			OnMoveComplete (e);
		}
	}

    public void Move(Vector3 target, float sec)
    {
        moveTarget = target;

        RefreshStatus(CharacterStatus.Moving);
        if (Vector3.Distance(target, transform.position) > 0.01f)
        {
            this.gameObject.transform.position += Time.deltaTime * target / sec;
        }

        else
        {
            moveTarget = transform.position;
            RefreshStatus(CharacterStatus.Idle);
            // send move complete(reached destination event)
            MoveEventArgs e = new MoveEventArgs(true, transform.position);
            OnMoveComplete(e);
        }
    }

    public void ChangeMoveTarget(Vector3 target)
    {
        moveTarget = target;
        RefreshStatus(CharacterStatus.Moving);
    }

	public void StopMove() {
		RefreshStatus (CharacterStatus.Idle);
		MoveEventArgs e = new MoveEventArgs (false, transform.position);
		OnMoveComplete (e);
	}
}


public enum MoveMethod
{
    Normal,
    CustomSpeed
}
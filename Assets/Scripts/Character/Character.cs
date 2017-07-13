using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IBattleHandler {

	protected int id;
	protected Team team;
	protected int state = 0;
	protected CharacterState state_temp;
	protected int maxHp;
	protected int hp;
	protected float speed_x;
	protected float speed_y;
	protected List<Buff> buffs = new List<Buff>();
	protected IBattleHandler target;
	protected Vector3 moveTarget;

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

	#endregion

	#region IBattleHandler implementation

	public Team Team {
		get {
			return this.team;
		}
	}

	public CharacterState State {
		get {
			return this.state_temp;
		}
	}

	public void ReceiveDamage (int damage)
	{
		hp -= damage;
		if (hp <= 0) {
			hp = 0;
			KillCharacter ();
		}
		
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

	void Update() {

		// set state



		switch (state_temp) {
		case CharacterState.Idle:
			break;
		case CharacterState.Moving:
			Move (moveTarget);
			break;
		default:
			break;
		}
	}

	protected virtual void RefreshState() {
		// this should reflect moving or channeling
		state = state & (Status.IsMovingMask | Status.IsChannelingMask);

		// compare with buff
		for (int i = 0; i < buffs.Count; i++) {
			IStatusBuff buff = buffs [i] as IStatusBuff;
			if (buff != null) {
				state = state | buff.Status;
			}
		}
	}

	// update hpBar for each character
	protected abstract void UpdateHpUI ();

	public void AttackTarget(int damage, IBattleHandler target) {
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

	protected void KillCharacter () {
		state_temp = CharacterState.Dead;

		// BattleManager check
		BattleManager.GetBattleManager ().CheckGame ();
	}

	public void Move (Vector3 target) {
		float speed;
		moveTarget = target;
		state_temp = CharacterState.Moving;

		// calculate speed
		Vector3 n = Vector3.Normalize(target - transform.position);
		speed = speed_x * (n.x * n.x / (n.x * n.x + n.y * n.y)) + speed_y * (n.y * n.y / (n.x * n.x + n.y * n.y));

		if (Vector3.Distance (target, transform.position) > 0.01f) {
			transform.position = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime);
		} else {
			moveTarget = transform.position;
			state_temp = CharacterState.Idle;
		}
	}
}

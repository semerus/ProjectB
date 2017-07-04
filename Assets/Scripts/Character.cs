using UnityEngine;

public abstract class Character : MonoBehaviour, IBattleHandler {

	protected int id;
	protected Team team;
	protected CharacterState state;
	protected int maxHp;
	protected int hp;
	protected float speed;
	protected Vector3 moveTarget;

	#region IBattleHandler implementation

	public void ReceiveDamage (int damage)
	{
		throw new System.NotImplementedException ();
	}

	public void ReceiveHeal (int heal)
	{
		throw new System.NotImplementedException ();
	}

	#endregion

	void Update() {
		switch (state) {
		case CharacterState.Idle:
			break;
		case CharacterState.Moving:
			Move (moveTarget);
			break;
		default:
			break;
		}
	}

	public abstract void Spawn ();
	protected void KillCharacter () {
		state = CharacterState.Dead;

		// BattleManager check
	}

	public void Move (Vector3 target) {
		moveTarget = target;
		state = CharacterState.Moving;

		if (Vector3.Distance (target, transform.position) > 0.01f) {
			transform.position = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime);
		} else {
			moveTarget = transform.position;
			state = CharacterState.Idle;
		}
	}
}

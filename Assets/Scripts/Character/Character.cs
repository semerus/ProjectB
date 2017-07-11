using UnityEngine;

public abstract class Character : MonoBehaviour, IBattleHandler {

	protected int id;
	protected Team team;
	protected CharacterState state;
	protected int maxHp;
	protected int hp;
	protected float speed_x;
	protected float speed_y;
	protected Vector3 moveTarget;

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
	}

	public void ReceiveDamage (int damage)
	{
		throw new System.NotImplementedException ();
	}

	public void ReceiveHeal (int heal)
	{
		throw new System.NotImplementedException ();
	}

	#endregion

	protected virtual void Update() {
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

	public virtual void Spawn () {
		// things to happen at load
		BattleManager.GetBattleManager ().AddEntity (this as IBattleHandler);

		// place at the correct place
	}

	protected void KillCharacter () {
		state = CharacterState.Dead;

		// BattleManager check
		BattleManager.GetBattleManager ().CheckGame ();
	}

	public void Move (Vector3 target) {
		float speed;
		moveTarget = target;
		state = CharacterState.Moving;

		// calculate speed
		Vector3 n = Vector3.Normalize(target - transform.position);
		speed = speed_x * Mathf.Sqrt(n.x * n.x / (n.x * n.x + n.y * n.y)) + speed_y * Mathf.Sqrt(n.y * n.y / (n.x * n.x + n.y * n.y));
        // i added Mathf.sqrt because calculation doesn't match. think about deltaX,speedX = 3, deltaY,speedY=4  (3,4,5 triangle)

		if (Vector3.Distance (target, transform.position) > 0.01f) {
			transform.position = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime);
            // may cause bugs if delta Distance is greater than 0.01f ex -> 0.13f - 0.25 -> abs. 0.12f
            // but i think it's unique case
		} else {
			moveTarget = transform.position;
            // why not
            // moveTarget = null;
            // may want to  keep last finished moveLocation?
			state = CharacterState.Idle;
		}
	}
}

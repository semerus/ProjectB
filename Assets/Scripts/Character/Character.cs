using UnityEngine;

public abstract class Character : MonoBehaviour, IBattleHandler {

	protected int id;
    [SerializeField]
	protected Team team;
    [SerializeField]
    protected CharacterState state;
    // may input interface queueAble
    public CharacterState queueState;

    protected int maxHp;
	protected int hp;
	protected float speed_x;
	protected float speed_y;
    
	protected IBattleHandler target;
    protected Vector3 moveTarget;

	#region Getters and Setters

	public IBattleHandler Target {
		get {
			return target;
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
		
		UpdateHpUI ();
	}

	public void ReceiveHeal (int heal)
	{
		hp += heal;
		if (hp >= maxHp) {
			hp = maxHp;
			KillCharacter ();
		}

		UpdateHpUI ();
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

    // update hpBar for each character
    protected abstract void UpdateHpUI();

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
		} else {
			moveTarget = transform.position;
            state = CharacterState.Idle;
		}
	}


}

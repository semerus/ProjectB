using UnityEngine;

public class OgreMale : Enemy {

	// temporary
	public Enemy partner;

	public Skill heal;

	void Start() {
		// temporary <- put this in spawn
		id = 3;
		team = Team.Hostile;
		state_temp = CharacterState.Idle;
		maxHp = 15000;
		hp = 15000;
		speed_x = 1f;
		speed_y = 1f;

		heal.SetSkill (this);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.H)) {
			OgreHeal oh = heal as OgreHeal;
			oh.SetTarget (partner);
			heal.OnClick ();
		}
	}

	protected override void InstructEnemyAI ()
	{
		
	}
}

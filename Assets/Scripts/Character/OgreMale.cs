using UnityEngine;

public class OgreMale : Enemy {

	// temporary
	public Enemy partner;

	public Skill heal;
	public MoveTest moveTest;

	void Start() {
		// temporary <- put this in spawn
		id = 3;
		team = Team.Hostile;
		state = CharacterState.Idle;
		maxHp = 15000;
		hp = 15000;
		speed_x = 1f;
		speed_y = 1f;

		heal.SetSkill (this);
	}

	void Update() {
		base.Update ();

		if (Input.GetKeyDown (KeyCode.H)) {
			OgreHeal oh = heal as OgreHeal;
			oh.SetTarget (partner);
			heal.OnClick ();
		}

		if (Input.GetKeyDown (KeyCode.M)) {
			moveTest.SetSkill (this);
			moveTest.OnClick ();
		}
	}

	protected override void InstructEnemyAI ()
	{
		
	}
}

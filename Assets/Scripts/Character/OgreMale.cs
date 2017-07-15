using UnityEngine;

public class OgreMale : Enemy, ITimeHandler {

	// temporary
	public OgreFemale partner;
	public OgreHeal heal;
	public OgreSetFire fire;
	public OgreMeteorStrike meteor;

	protected float ai_timer;
	protected int pattern = 1;

	#region ITimeHandler implementation
	public void RunTime ()
	{
		ai_timer += Time.deltaTime;
		InstructEnemyAI ();
	}
	#endregion

	void Start() {
		// temporary <- put this in spawn
		id = 3;
		team = Team.Hostile;
		state = CharacterState.Idle;
		maxHp = 15000;
		hp = 15000;
		speed_x = 1f;
		speed_y = 1f;

		heal = gameObject.AddComponent<OgreHeal> ();
		meteor = gameObject.AddComponent<OgreMeteorStrike> ();
		fire = gameObject.AddComponent<OgreSetFire> ();
		fire.SetSkill (this);
		heal.SetSkill (this);
		meteor.SetSkill (this);

		ai_timer = 0f;
		TimeSystem.GetTimeSystem ().AddTimer (this);
	}

	protected override void InstructEnemyAI ()
	{
		switch (pattern) {
		// start
		case 0:
			if (ai_timer >= 20f) {
				pattern = 1;
				heal.SetTarget (partner);
				heal.OnClick ();
				ai_timer = 0f;
			}
			break;
		// after heal
		case 1:
			if (ai_timer >= 20f) {
				pattern = 2;
				// torch fire
				fire.OnClick();
				Debug.Log("Torch fire");
				ai_timer = 0f;
			}
			break;
		// after torch file
		case 2:
			if (ai_timer >= 20f) {
				pattern = 0;
				meteor.OnClick ();
				Debug.Log("Meteor");
				ai_timer = 0;
			}
			break;
		default:
			Debug.LogError ("Wrong " + this.transform.root.name + " actions");
			break;
		}

//		if (ai_timer >= 20f) {
//			meteor.OnClick ();
//			ai_timer = 0f;
//		}
	}

	protected override void KillCharacter ()
	{
		base.KillCharacter ();
		TimeSystem.GetTimeSystem ().DeleteTimer (this);
	}
}

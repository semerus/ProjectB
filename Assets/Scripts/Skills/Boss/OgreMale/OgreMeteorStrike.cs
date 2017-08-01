using System.Collections.Generic;
using UnityEngine;

public class OgreMeteorStrike : Skill {

	Character target;

	public GameObject meteorPrefab;
	protected int counter;
	protected float progressTimer;
	protected int strikes = 6;
	protected Stack<IPooledItem_Character> pool = new Stack<IPooledItem_Character>();

	public Stack<IPooledItem_Character> Pool {
		get {
			return pool;
		}
	}

	public override void RunTime ()
	{
		base.RunTime ();

		if (CheckSkillStatus(SkillStatus.ProcessMask)) {
			progressTimer += Time.deltaTime;
			if (progressTimer >= 1.5f) {
				progressTimer = 0f;
				InProgress ();
			}
		}
	}

	#region implemented abstract members of Skill

	public override void Activate ()
	{
		UpdateSkillStatus (SkillStatus.ProcessOn);
		// first strike
		counter = 0;
		InProgress ();
		TimeSystem.GetTimeSystem ().AddTimer (this);
	}

	#endregion

	void Start() {
		cooldown = 15f;
		meteorPrefab = Resources.Load ("Skills/Area/Meteor") as GameObject;
	}

	protected void InProgress() {
		if (counter++ < strikes) {
			Vector3 pos = SetTarget ();
			CreateMeteor (pos);
		} else {
			StartCoolDown ();
		}
	}

	Vector3 SetTarget () {
		Vector3 pos;
		IBattleHandler[] friendly = BattleManager.GetBattleManager ().GetEntities (Team.Friendly);
		int random = Random.Range (0, friendly.Length);
		target = friendly [random] as Character;
		if (target != null) {
			return target.transform.position;
		} else {
			return Vector3.zero;
		}
	}

	void CreateMeteor(Vector3 pos) {
		Meteor meteor = Instantiate (meteorPrefab).GetComponent<Meteor> ();
		meteor.SetMeteor (caster, pos);
	}
}

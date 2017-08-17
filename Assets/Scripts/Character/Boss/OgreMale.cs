/*
 * Written by Insung Kim
 * Updated: 2017.08.13
 */
using UnityEngine;

public class OgreMale : Enemy {

	// temporary
	public OgreFemale partner;

	protected float ai_timer;
	protected int pattern = 0;

	#region ITimeHandler implementation
	public override void RunTime ()
	{
		base.RunTime ();
		ai_timer += Time.deltaTime;
		InstructEnemyAI ();
	}
	#endregion

	protected void Start() {
		// temporary <- put this in spawn

		/*
		skills = new Skill[3];
		skills[0] = gameObject.AddComponent<OgreHeal> ();
		skills[2] = gameObject.AddComponent<OgreMeteorStrike> ();
		skills[1] = gameObject.AddComponent<OgreSetFire> ();

		for (int i = 0; i < skills.Length; i++) {
			//skills [i].SetSkill (this);
		}
		*/

		ai_timer = 0f;
		TimeSystem.GetTimeSystem ().AddTimer (this);
	}

	protected override void InstructEnemyAI ()
	{
		base.InstructEnemyAI ();
		switch (pattern) {
		// start
		case 0:
			if (ai_timer >= 20f) {
				pattern = 1;
				OgreHeal heal = skills [0] as OgreHeal;
				heal.SetTarget (partner);
				skills[0].OnCast ();
				ai_timer = 0f;
			}
			break;
		// after heal
		case 1:
			if (ai_timer >= 20f) {
				pattern = 2;
				// torch fire
				skills[1].OnCast();
				ai_timer = 0f;
			}
			break;
		// after torch file
		case 2:
			if (ai_timer >= 20f) {
				pattern = 0;
				skills[2].OnCast ();
				Debug.Log("Meteor");
				ai_timer = 0;
			}
			break;
		default:
			Debug.LogError ("Wrong " + this.transform.root.name + " actions");
			break;
		}
	}

	protected override void KillCharacter ()
	{
		TimeSystem.GetTimeSystem ().DeleteTimer (this);
		base.KillCharacter ();
	}
}

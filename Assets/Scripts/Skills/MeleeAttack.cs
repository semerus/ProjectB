using System;
using UnityEngine;

public class MeleeAttack : Skill {

	protected int damage;
	protected float range;
	protected bool isMoving = false;

	#region implemented abstract members of Skill

	public override void Activate ()
	{
		// run animation
		UpdateSkillStatus(SkillStatus.ProcessOn);
		caster.ChangeAction (CharacterAction.Attacking);
		caster.Anim.ClearAnimEvent ();
		caster.Anim.onCue += Attack;
	}

	#endregion

	void Awake() {
		caster = gameObject.GetComponent<Character> ();
		Hero h = caster as Hero;
		if (h != null) {
			h.autoAttack = this;
		}
	}

	void OnMoveComplete(object sender, EventArgs e) {
		MoveEventArgs m = e as MoveEventArgs;
		isMoving = false;
		if (m != null && m.result) {
			Activate ();
		}
		caster.MoveComplete -= OnMoveComplete;
	}

	public override void RunTime ()
	{
		base.RunTime ();
		if (isMoving) {
			ApproachTarget ();
		}
	}

	public override void OnCast ()
	{
		// check condition
		if (!CheckCondition ()) {
			Debug.Log ("Get Moving");
			return;
		}

		// move
		TimeSystem.GetTimeSystem().AddTimer(this);
		isMoving = true;
		ApproachTarget();
		caster.MoveComplete += new EventHandler<MoveEventArgs> (OnMoveComplete);
	}

	public override void SetSkill (System.Collections.Generic.Dictionary<string, object> param)
	{
		base.SetSkill (param);
		this.damage = (int)param ["damage"];
		this.range = (float)((double)param ["range"]);
	}

	public override bool CheckCondition ()
	{
		bool isReady = false;
		Character c = caster.Target as Character;

		if (c != null && c.Action != CharacterAction.Dead) {
			isReady = true;
		}

		return isReady && base.CheckCondition ();
	}

	public bool CheckRange() {
		Character c = caster.Target as Character;
		if (c == null) {
			return false;
		}

		if (Vector3.Distance (caster.transform.position, c.transform.position) < (range + 0.1f)) {
			return true;
		} else {
			return false;
		}
	}

	protected void ApproachTarget() {
		Character c = caster.Target as Character;
		Vector3 targetPos;

		if (caster.transform.position.x < c.transform.position.x) {
			targetPos = c.transform.position - new Vector3 (range, 0f);
		} else {
			targetPos = c.transform.position + new Vector3 (range, 0f);
		}

		if (c != null) {
			caster.ChangeMoveTarget (targetPos);
		}
	}

	protected virtual void Attack() {
		// temporary solution (try to solve attack cancel issue)
		if (caster.Target != null) {
			caster.AttackTarget (caster.Target, damage);
		}
		UpdateSkillStatus (SkillStatus.ProcessOff);
		caster.ChangeAction (CharacterAction.Idle);
		StartCoolDown ();
		caster.Anim.onCue -= Attack;
	}
}

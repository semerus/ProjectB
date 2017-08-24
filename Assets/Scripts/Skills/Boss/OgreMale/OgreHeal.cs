/*
 * Written by Insung Kim
 * Updated: 2017.08.23
 */
using UnityEngine;

public class OgreHeal : Skill, IChanneling {

	protected int heal;
	protected float channelTime;
	protected float timer_channeling;
	protected Character target;

	#region implemented abstract members of Skill

	public override void Activate ()
	{
		caster.HealTarget (heal, target);
		Teleport (this.target);
		StartCoolDown ();
	}

	#endregion

	#region IChanneling implementation

	public void OnChanneling ()
	{
		timer_channeling += Time.deltaTime;

		if (timer_channeling >= channelTime) {
			timer_channeling = 0f;
			UpdateSkillStatus (SkillStatus.ChannelingOff);
			Activate ();
		}
	}

	public void OnInterrupt (IBattleHandler interrupter)
	{
		UpdateSkillStatus (SkillStatus.ChannelingOff);
		timer_channeling = 0f;
		StartCoolDown ();
	}

	public float ChannelTime {
		get {
			return channelTime;
		}
	}

	public float Timer_Channeling {
		get {
			return timer_channeling;
		}
		set {
			timer_channeling = value;
		}
	}

	#endregion

	void Awake() {
		caster = gameObject.GetComponent<Character> ();
	}

	public override void SetSkill (System.Collections.Generic.Dictionary<string, object> param)
	{
		base.SetSkill (param);
		this.heal = (int)param ["heal"];
		this.channelTime = (float)((double)param ["channel_time"]);
	}

	public virtual void SetTarget(Character target) {
		this.target = target;
	}

	protected void Teleport(Character target) {
		Vector3 des;
		if (target.IsFacingLeft) {
			des = new Vector3 (target.transform.position.x + 4f, target.transform.position.y, 0f);
		} else {
			des = new Vector3 (target.transform.position.x - 4f, target.transform.position.y, 0f);
		}

		if (Background.GetBackground ().CheckBoundaries (des)) {
			// if inside boundaries teleport
			caster.transform.position = des;
		}

		caster.ChangeAction (CharacterAction.Idle);
	}
}

using UnityEngine;

public class OgreHeal : Skill, IChanneling {

	protected int healPoint;
	protected float channelTime;
	protected float timer_channeling;
	protected Character target;

	#region implemented abstract members of Skill

	public override void Activate ()
	{
		UpdateSkillStatus (SkillStatus.ChannelingOff);
		caster.HealTarget (healPoint, target);
		Teleport (this.target);
		StartCoolDown ();
	}

	#endregion

	#region IChanneling implementation

	public void OnChanneling ()
	{
		if(!CheckSkillStatus(SkillStatus.ChannelingMask))
			UpdateSkillStatus (SkillStatus.ChannelingOn);

		if(!TimeSystem.GetTimeSystem().CheckTimer(this))
			TimeSystem.GetTimeSystem ().AddTimer (this);

		timer_channeling += Time.deltaTime;
		if (timer_channeling >= channelTime) {
			Activate ();
			timer_channeling = 0f;
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

	void Start() {
		// temporary
		cooldown = 20f;
		healPoint = 3000;
		channelTime = 4f;

		timer_cooldown = 0f;
		timer_channeling = 0f;
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
	}
}

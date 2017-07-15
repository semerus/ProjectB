using UnityEngine;

public abstract class Skill : MonoBehaviour, ITimeHandler {

	// gameUI prefab
	protected int id;
	protected Character caster;
    [SerializeField]
	protected SkillState state;
	protected float cooldown;
    [SerializeField]
	protected float timer_cooldown;

	#region ITimeHandler implementation

	public virtual void RunTime ()
	{
		switch(state){
		case SkillState.OnCoolDown:
			OnCoolDown ();
			break;
		case SkillState.Channeling:
			IChanneling ch = this as IChanneling;
			ch.OnChanneling ();
			break;
		default:
			break;
		}
	}

    #endregion
    
    public void SetSkill(Character caster) {
		this.caster = caster;
	}

	// when ui button is clicked
	public virtual void OnClick() {
		// check state
	}

	// activate skill (launch projectile, area etc)
	// run cooldown
	public abstract void Activate (IBattleHandler target);

	protected void OnCoolDown() {
		timer_cooldown += Time.deltaTime;

		// change skill ui if necessary

		if (timer_cooldown >= cooldown) {
			// reset skill to ready
			state = SkillState.Ready;
			timer_cooldown = 0f;
			TimeSystem.GetTimeSystem ().DeleteTimer (this as ITimeHandler);
		}
	}

	protected void StartCoolDown() {
		state = SkillState.OnCoolDown;
		if (!TimeSystem.GetTimeSystem ().CheckTimer (this as ITimeHandler)) {
			TimeSystem.GetTimeSystem ().AddTimer (this as ITimeHandler);
		}
	}
}

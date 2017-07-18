﻿using System;
using UnityEngine;

public abstract class Skill : MonoBehaviour, ITimeHandler {

	// gameUI prefab
	protected int id;
	protected Character caster;
    [SerializeField]
	protected int skillStatus = SkillStatus.ReadyOn;
	protected float cooldown;
    [SerializeField]
	protected float timer_cooldown;

	public event EventHandler<SkillEventArgs> EndSkill;

	#region ITimeHandler implementation

	public virtual void RunTime ()
	{
		if (CheckSkillState(SkillStatus.OnCoolDownMask)) {
			OnCoolDown ();
		}
		if (CheckSkillState(SkillStatus.ChannelingMask)) {
			IChanneling ch = this as IChanneling;
			ch.OnChanneling ();
		}
	}

    #endregion

    #region Getters and Setters
	public int State
    {
		get { return skillStatus; }
    }
    #endregion

    public void OnEndSkill(SkillEventArgs e) {
		UpdateSkillState (SkillStatus.ProcessOff);

		EventHandler<SkillEventArgs> endSkill = EndSkill;
		if (endSkill != null) {
			endSkill (this, e);
		}
	}

    public void SetSkill(Character caster) {
		this.caster = caster;
	}

	// when ui button is clicked
	public virtual void OnCast() {
		// check state

		// states when skill can be used
		IChanneling ch = this as IChanneling;
		if (ch != null) {
			// start channeling
			ch.OnChanneling ();
		} else {
			Activate (caster.Target);
		}
	}

	// activate skill (launch projectile, area etc)
	// run cooldown
	public abstract void Activate (IBattleHandler target);

	protected virtual void OnCoolDown() {
		timer_cooldown += Time.deltaTime;

		// change skill ui if necessary

		if (timer_cooldown >= cooldown) {
			// reset skill to ready
			UpdateSkillState(SkillStatus.OnCoolDownOff);
			timer_cooldown = 0f;
			TimeSystem.GetTimeSystem ().DeleteTimer (this as ITimeHandler);
		}
	}

	protected virtual void StartCoolDown() {
		UpdateSkillState (SkillStatus.OnCoolDownOn);
		if (!TimeSystem.GetTimeSystem ().CheckTimer (this as ITimeHandler)) {
			TimeSystem.GetTimeSystem ().AddTimer (this as ITimeHandler);
		}
	}

	public bool CheckSkillState(int mask){
		int test = skillStatus & mask;
		if (test > 0)
			return true;
		else
			return false;
	}

	protected void UpdateSkillState(int change) {
		skillStatus = change & SkillStatus.All;

		if (change == SkillStatus.ChannelingOn || change == SkillStatus.ProcessOn || change == SkillStatus.OnCoolDownOn) {
			skillStatus = skillStatus | change;
		} else if (change == SkillStatus.ChannelingOff || change == SkillStatus.ProcessOff || change == SkillStatus.OnCoolDownOff) {
			skillStatus = skillStatus & change;
		}

		if (skillStatus == 0) {
			skillStatus = SkillStatus.ReadyOn;
		}
	}
}

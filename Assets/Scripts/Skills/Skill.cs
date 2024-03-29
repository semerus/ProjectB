﻿/*
 * Written by Insung Kim
 * Updated: 2017.08.13
 */
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour, ITimeHandler {

	// gameUI prefab
	protected int id;
	protected Character caster;
	protected int skillStatus = SkillStatus.ReadyOn;
	protected float cooldown;
	protected float curCooldown;
    [SerializeField]
	protected float timer_cooldown = 0f;

	public event EventHandler<SkillEventArgs> EndSkill;

	#region Getters and Setters
	public int Status
	{
		get { return skillStatus; }
	}
	#endregion

	#region ITimeHandler implementation

	public virtual void RunTime ()
	{
		if (CheckSkillStatus(SkillStatus.OnCoolDownMask)) {
			OnCoolDown ();
		}
		if (CheckSkillStatus(SkillStatus.ChannelingMask)) {
			IChanneling ch = this as IChanneling;
            if(ch != null)
            {
                ch.OnChanneling();
            }
		}
		if (CheckSkillStatus (SkillStatus.ProcessMask)) {
			OnProcess ();
		}
	}

    #endregion

	public virtual void SendEndMessage(SkillEventArgs e) {
		UpdateSkillStatus (SkillStatus.ProcessOff);

		EventHandler<SkillEventArgs> endSkill = EndSkill;
		if (endSkill != null) {
			endSkill (this, e);
		}
	}

	public virtual void SetSkill(Dictionary<string, object> param) {
		//this.caster = gameObject.GetComponent<Character> ();
		this.id = (int)param ["id"];
		this.cooldown = (float)((double)param ["cooldown"]);
		curCooldown = cooldown;
	}

	// when ui button is clicked
	public virtual void OnCast() {
		// check state
		if (!CheckCondition ()) {
			return;
		}

		// states when skill can be used
		IChanneling ch = this as IChanneling;
		if (ch != null) {
			// start channeling
			caster.ChangeAction(CharacterAction.Channeling);
			UpdateSkillStatus(SkillStatus.ChannelingOn);
			ch.OnChanneling ();
            TimeSystem.GetTimeSystem().AddTimer(this);
		} else {
			Activate ();
		}
	}

	// activate skill (launch projectile, area etc)
	// run cooldown

	public virtual void Activate () {
	}

	public virtual bool CheckCondition () {
		if (CheckSkillStatus (SkillStatus.ReadyMask) && !caster.CheckCharacterStatus(CharacterStatus.NotOrderableMask)) {
			return true;
		} else {
			return false;
		}
	}

	protected virtual void OnProcess() {
	}

    protected virtual void OnCoolDown() {
		timer_cooldown += Time.deltaTime;

		// change skill ui if necessary

		if (timer_cooldown >= curCooldown) {
			// reset skill to ready
			UpdateSkillStatus(SkillStatus.OnCoolDownOff);
			timer_cooldown = 0f;
			TimeSystem.GetTimeSystem ().DeleteTimer (this as ITimeHandler);
		}
	}

	protected virtual void StartCoolDown() {
		UpdateSkillStatus (SkillStatus.OnCoolDownOn);
		TimeSystem.GetTimeSystem ().AddTimer (this as ITimeHandler);
	}

	public virtual void OnEndSkill() {
		UpdateSkillStatus (SkillStatus.ProcessOff);

		SkillEventArgs s = new SkillEventArgs(this.name, true);
		SendEndMessage(s);
	}

	public bool CheckSkillStatus(int mask){
		int test = skillStatus & mask;
		if (test > 0)
			return true;
		else
			return false;
	}

	protected void UpdateSkillStatus(int change) {
		skillStatus &= SkillStatus.All;

		if (change == SkillStatus.ChannelingOn || change == SkillStatus.ProcessOn || change == SkillStatus.OnCoolDownOn) {
			skillStatus = skillStatus | change;
		} else if (change == SkillStatus.ChannelingOff || change == SkillStatus.ProcessOff || change == SkillStatus.OnCoolDownOff) {
			skillStatus = skillStatus & change;
		}

		if (skillStatus == 0) {
			skillStatus = SkillStatus.ReadyOn;
		}
	}

    public void AddCooltime(float time)
    {
        timer_cooldown += time;
    }

	public void CancelSkill() {
		UpdateSkillStatus (SkillStatus.ProcessOff);
		StartCoolDown ();
	}
}

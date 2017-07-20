using UnityEngine;

public abstract class Buff {
	Character caster;
	Character target;

	// use constructor for creating instance

	/// <summary>
	/// Starts the buff. Please base.StartBuff() when overriding
	/// </summary>
	/// <param name="caster">Caster.</param>

	public virtual void StartBuff(Character target) {
		// add to Buff list to target
		AddBuff(target);

		// base.StartBuff();
		// add timer to TimeSystem if it implements ITimeHandler
	}

	public virtual void EndBuff() {
		// delete from Buff list
		DeleteBuff(this.target, this);

		// base.EndBuff();
		// delete timer to TimeSystem if it implements ITimeHandler
	}

	private void AddBuff(Character target) {
		if (target.Buffs.Contains (this))
			return;
		target.Buffs.Add (this);
		target.RefreshBuff ();
	}

	private void DeleteBuff(Character target, Buff buff) {
		if (target.Buffs.Remove (this)) {
			target.Anim.UpdateAnimation ();
			target.RefreshBuff ();
		}
	}
}

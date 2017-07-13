using UnityEngine;

public abstract class Buff {
	Character caster;
	Character target;

	// use constructor for creating instance

	public virtual void StartBuff(Character caster, Character target) {
		// add to Buff list to target
		this.target = target;
		target.Buffs.Add(this);

		// base.StartBuff();
		// add timer to TimeSystem if it implements ITimeHandler
	}

	public virtual void EndBuff() {
		// delete from Buff list
		this.target.Buffs.Remove(this);

		// base.EndBuff();
		// delete timer to TimeSystem if it implements ITimeHandler
	}
}

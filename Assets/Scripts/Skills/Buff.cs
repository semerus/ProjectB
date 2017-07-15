using UnityEngine;

public abstract class Buff {
	Character caster;
	Character target;

	// use constructor for creating instance

	/// <summary>
	/// Starts the buff. Please base.StartBuff() when overriding
	/// </summary>
	/// <param name="caster">Caster.</param>
	/// <param name="target">Target.</param>

	public virtual void StartBuff(Character caster, Character target) {
		// add to Buff list to target
		this.target = target;
		target.Buffs.Add(this);

		// check for status changes to the character
		IStatusBuff buff = this as IStatusBuff;
		if (buff != null) {
			if ((buff.Status & CharacterStatus.IsSilencedMask) > 0) {
				// interrupt all channeling
			}
			if ((buff.Status & CharacterStatus.IsRootedMask) > 0) {
				// stop all moving
				target.StopMove();
			}
		}
			
		target.RefreshStatus (CharacterStatus.GetCurrentActionStatus (target));

		// base.StartBuff();
		// add timer to TimeSystem if it implements ITimeHandler
	}

	public virtual void EndBuff() {
		// delete from Buff list
		target.Buffs.Remove(this);

		target.RefreshStatus (CharacterStatus.GetCurrentActionStatus (target));

		// base.EndBuff();
		// delete timer to TimeSystem if it implements ITimeHandler
	}
}

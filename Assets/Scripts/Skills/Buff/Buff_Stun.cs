/*
 * Written by Insung Kim
 * Updated: 2017.08.2
 */
using UnityEngine;

public class Buff_Stun : Buff, ITimeHandler, IStatusBuff {

	float buffTimer = 0;
	float buffTime;

	#region implemented abstract members of Buff

	public override void CheckCounterBuff ()
	{
		
	}

	#endregion

	#region ITimeHandler implementation

	public void RunTime ()
	{
		buffTimer += Time.deltaTime;
		if (buffTimer >= buffTime) {
			EndBuff ();
		}
	}

	#endregion

	#region IStatusBuff implementation

	public int Status {
		get {
			return CharacterStatus.Stunned;
		}
	}

	#endregion

	public Buff_Stun (float time, Character caster, Character target) {
		this.buffTime = time;

		StartBuff (caster, target);
	}

	public override void StartBuff (Character caster, Character target)
	{
		base.StartBuff (caster, target);
		TimeSystem.GetTimeSystem ().AddTimer (this);
	}
}

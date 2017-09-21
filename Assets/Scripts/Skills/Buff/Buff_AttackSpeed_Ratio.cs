using UnityEngine;

public class Buff_AttackSpeed_Ratio : Buff, IBuff_AttackSpeed_Ratio {

	float attackSpeed = -0.3f;

	#region implemented abstract members of Buff

	public override void CheckCounterBuff ()
	{
		//throw new System.NotImplementedException ();
		for (int i = 0; i < caster.Buffs.Count; i++) {
			if (caster.Buffs [i] is Buff_AttackSpeed_Ratio) {
				caster.Buffs [i].EndBuff ();
			}
		}
	}

	#endregion

	#region IBuff_AttackSpeed_Ratio implementation

	public float AttackSpeed_Ratio {
		get {
			return attackSpeed;
		}
	}

	#endregion

	public Buff_AttackSpeed_Ratio(Character caster, Character target) {
		this.isBuff = false;

		this.buffName = "AttackSpeed";

		StartBuff (caster, target);
	}
}

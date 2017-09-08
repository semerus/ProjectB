using UnityEngine;

public class Healer_Restore_BonusHeal : Healer_Restore {

	protected int heal;

	public override void SetSkill (System.Collections.Generic.Dictionary<string, object> param)
	{
		base.SetSkill (param);
		this.heal = (int)param ["heal"];
	}

	protected override void InvokeEffect (Character target)
	{
		base.InvokeEffect (target);
		caster.HealTarget (heal, target);
	}

}

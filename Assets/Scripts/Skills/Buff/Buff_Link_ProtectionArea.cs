using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_Link_ProtectionArea : Buff {

	public Healer_ProtectionArea healer_ProtectionArea;

    #region override Buff things
    public override void CheckCounterBuff()
    {
        // Have no counter Buff
        ;
    }

    public override void EndBuff()
    {
        base.EndBuff();
        healer_ProtectionArea.LinkedHeroes.Remove(target as Hero);
    }
    #endregion

    #region Constructor
    public Buff_Link_ProtectionArea(Healer_ProtectionArea healer_ProtectionArea, Character target, string name)
    {
        caster = null;
        this.target = target;
        isBuff = true;
		this.healer_ProtectionArea = healer_ProtectionArea;
		healer_ProtectionArea.LinkedHeroes.Add(target as Hero);
        StartBuff(null, this.target);

        // for Debugging
        buffName = name;
        target.UpdateBuffList();
    }
    #endregion
}

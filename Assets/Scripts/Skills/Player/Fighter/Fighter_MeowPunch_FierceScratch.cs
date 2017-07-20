using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_MeowPunch_FierceScratch : Fighter_MeowPunch {
    #region TraitChanges Implment
    protected override void TraitBuffCasting(Character caster, Character tarCharacter)
    {
        Buff_ReceiveDmg_Ratio debuff = new Buff_ReceiveDmg_Ratio(caster, tarCharacter, false, 0.25f, 2f, "RDmg+25%2sec");;
    }
    protected override void TraitSetValue()
    {
        cooldown = 15f;
        dmg = 100;
        HPCost = 50;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_MeowPunch_ForePaw : Fighter_MeowPunch {
    #region implemented abstract members of Skill
    protected override void TraitBuffCasting(Character caster, Character tarCharacter)
    {
        Buff_AllDmg_Ratio debuff = new Buff_AllDmg_Ratio(caster, tarCharacter, false, -0.25f, 2f, "ADmg-25%2sec"); ;
    }
    #endregion
}

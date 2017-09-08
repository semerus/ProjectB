﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_MeowPunch_FierceScratch : Fighter_MeowPunch {
    #region TraitChanges Implment
    protected override void TraitBuffCasting(Character caster, Character tarCharacter)
	{
		new Buff_ReceiveDmg_Ratio (caster, tarCharacter, false, 0.25f, 2f, "RDmg+25%2sec");
	}
    #endregion
}

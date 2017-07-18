using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter_Passive : Skill {
    #region implemented abstract members of Skill
    
    public override void Activate(IBattleHandler target)
    {
        float lifeDrainValue = 10f;

        Character self = target as Character;
        Buff_LifeDrainAbs_FighterPassive fighter_Passive_Lifedrain = new Buff_LifeDrainAbs_FighterPassive(lifeDrainValue);
        self.Buffs.Add(fighter_Passive_Lifedrain);
    }

    #endregion
}
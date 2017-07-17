using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_LifeDrainAbs_FighterPassive :  Buff, ILifeStealAbsBuff {

    float lifeDrainAbs;

    #region implemented ILifeStealAbsBuff

    public float LifeStealAbs
    {
        get { return lifeDrainAbs; }
    }

    #endregion

    #region class Constructor

    public Buff_LifeDrainAbs_FighterPassive(float lifeDrainAbsValue)
    {
        lifeDrainAbs = lifeDrainAbsValue;
    }

    #endregion



}

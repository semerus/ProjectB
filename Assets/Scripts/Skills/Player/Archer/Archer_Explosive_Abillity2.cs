using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Explosive_Abillity2 : Archer_Explosive {

    int abillity = 2;

    public override void Activate()
    {
        EffectOn(abillity);
        EffectMove();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Explosive_Abillity1 : Archer_Explosive {

    int abillity = 1;

    public override void Activate()
    {
        EffectOn(abillity);
        EffectMove();
    }
}

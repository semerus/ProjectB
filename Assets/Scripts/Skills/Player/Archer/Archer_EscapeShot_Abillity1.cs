using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_EscapeShot_Abillity1 : Archer_EscapeShot {

    private int abillity=1;

    public override void Activate()
    {
        EscapeTarget(abillity);
    }
}

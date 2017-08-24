using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_EscapeShot_Abillity2 : Archer_EscapeShot {

    private int abillity = 2;

    public override void Activate()
    {
        EscapeTarget(abillity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_SignTrap_Abillity1 : Archer_SignTrap {

    private int abillity = 1;

    public override void Activate()
    {
        TrapOn(abillity);
    }
}

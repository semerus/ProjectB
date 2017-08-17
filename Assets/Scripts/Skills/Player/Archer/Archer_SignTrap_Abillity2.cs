using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_SignTrap_Abillity2 : Archer_SignTrap {

    private int abillity = 2;

    public override void Activate()
    {
        TrapOn(abillity);
    }
}

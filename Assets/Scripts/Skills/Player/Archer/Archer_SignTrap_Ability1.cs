﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_SignTrap_Ability1 : Archer_SignTrap {

    private void Awake()
    {
        
    }

    protected override void SetTrap()
    {
        trap = Instantiate(Resources.Load<GameObject>("Skills / Heroes / Archer / SignTrap / SignTrap_Abillity1"));
        base.SetTrap();
    }
}

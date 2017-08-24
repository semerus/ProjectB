using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_AutoAttack_Projectile : Projectile {

    public override void OnArrival(int a)
    {
        Debug.Log("hi");
        caster.AttackTarget(target, 20);
        base.OnArrival(a);
    }

}

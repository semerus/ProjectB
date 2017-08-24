using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack_Projectile : Projectile {

    public override void OnArrival(int a)
    {
		caster.AttackTarget (target, 20);
        base.OnArrival(a);
    }
}

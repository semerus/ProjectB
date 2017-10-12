using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Explosive_Ability2 : Archer_Explosive {

    protected override void SetArrowProjectiles()
    {
        projectile = Instantiate(Resources.Load("Skills/Heroes/Archer/Explosive/Explosive_Abillity2")) as Projectile;
        projectile.ProjectileOn(caster);
        base.SetArrowProjectiles();
    }
}

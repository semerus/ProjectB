using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Explosive_Ability1 : Archer_Explosive {

    protected override void SetArrowProjectiles()
    {   
        projectile = Instantiate(Resources.Load("Skills/Heroes/Archer/Explosive/Explosive_Abillity1")) as Projectile;
        projectile.ProjectileOn(caster);
        base.SetArrowProjectiles();
    }
}

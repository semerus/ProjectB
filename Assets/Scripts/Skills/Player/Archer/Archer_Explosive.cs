using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer_Explosive : HeroActive {

    Projectile projectile = new Projectile();

    public void EffectOn(int abillity)
    {
        switch(abillity)
        {
            case 1:
                projectile = Instantiate(Resources.Load("Skills/Heroes/Archer/Explosive/Explosive_Abillity1")) as Projectile;
                projectile.ProjectileOn(caster, 1);
                break;

            case 2:
                projectile = Instantiate(Resources.Load("Skills/Heroes/Archer/Explosive/Explosive_Abillity2")) as Projectile;
                projectile.ProjectileOn(caster, 2);
                break;
        }
        projectile.transform.SetParent(GameObject.Find("Projectiles").transform);
        projectile.transform.position = caster.transform.position;
    }

    public void EffectMove()
    {
        projectile.ProjectileMove(caster.Target as Character, 10);
    }
}

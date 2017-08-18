using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Snowball_Abillity1 : Wizard_Snowball
{
    protected int abillity = 1;
    public static int abillity1count = 0;

    public override void OnChanneling()
    {
        base.OnChanneling();
        SnowProjectileRoundOn(abillity);
    }

    public override void OnInterrupt(IBattleHandler interrupter)
    {
        base.OnInterrupt(interrupter);
        DeleteProjectile();
        caster.ChangeAction(CharacterAction.Attacking);
    }

    protected override void OnProcess()
    {
        Wizard_Passive.skillCount++;
        SnowProjectileShoot(abillity);
    }

    public void DeleteProjectile()
    {
        for (int i = 0; i <= (int)(skilltime - 0.1 * i); i++)
        {
            projectile[i].DeleteProjectile();
        }
    }
}
    
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
        caster.ChangeAction(CharacterAction.Attacking);
    }

    protected override void OnProcess()
    {
        SnowProjectileShoot(abillity);
    }
}
    
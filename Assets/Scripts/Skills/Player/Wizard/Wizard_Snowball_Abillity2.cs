using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Snowball_Abillity2 : Wizard_Snowball{
    
    protected int abillity = 2;
    public static int abillity2count = 0;

    public override void OnChanneling()
    {
        base.OnChanneling();
        SnowProjectileRoundOn(abillity);
    }

    public override void OnInterrupt(IBattleHandler interrupter)
    {
        base.OnInterrupt(interrupter);
        OnProcess();
    }

    protected override void OnProcess()
    {
        SnowProjectileShoot(abillity);
    }
}

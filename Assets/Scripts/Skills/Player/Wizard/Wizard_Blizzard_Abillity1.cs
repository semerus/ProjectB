using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Blizzard_Abillity1 : Wizard_Blizzard {

    protected int abillity = 1;

    public override void OnChanneling()
    {
        base.OnChanneling();
        SkillChanneling(abillity);
    }

    protected override void OnProcess()
    {
        Wizard_Passive.skillCount++;
        base.OnProcess();
        Blizzard(abillity);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_AutoAttack : Skill
{
    public override void Activate(IBattleHandler target)
    {
        cooldown = 2;
        StartCoolDown();
        AuttoAttack();
    }

    public void AuttoAttack()
    {
        int damage = 20;
        IBattleHandler target = caster.Target;
        caster.AttackTarget(target, damage);
    }
}

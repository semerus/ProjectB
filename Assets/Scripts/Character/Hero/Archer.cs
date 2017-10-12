using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Hero
{
    public override void RunTime()
    {
        base.RunTime();
        if (Action == CharacterAction.Idle && autoAttack.CheckCondition())
        {
            autoAttack.OnCast();
        }
    }

    protected override void KillCharacter()
    {
        base.KillCharacter();
        gameObject.SetActive(false);
    }
}


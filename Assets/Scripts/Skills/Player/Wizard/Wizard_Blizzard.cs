using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Blizzard : Skill
{
    public override void Activate(IBattleHandler target)
    {
        
    }

    public void Blizzard()
    {
        float tx;

        if (caster.IsFacingLeft)
        {
            tx = this.gameObject.transform.position.x - 3f;
        }
        else
        {
            tx = this.gameObject.transform.position.x + 3f;
        }

        Vector3 targetPosition = new Vector3(tx, this.gameObject.transform.position.y);
    }
}

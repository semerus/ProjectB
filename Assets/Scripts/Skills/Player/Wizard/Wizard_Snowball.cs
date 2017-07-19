using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Snowball : Skill
{
    IBattleHandler[] enemyNum;
    IBattleHandler target;

    public override void Activate(IBattleHandler target)
    {
        ResetSetting();
        if(target!=null)
        {
            StartCoolDown();
            SnowBall();
        }
        else
        {
            return;
        }
    }

    public void ResetSetting()
    {
        enemyNum = BattleManager.GetBattleManager().GetEntities(Team.Hostile);
        cooldown = 10f;
        target = caster.Target;
    }

    #region SnowBall_Skill

    public void SnowBall()
    {
        int damage = (int)(50 * Wizard_Passive.mag);
        int splash = 25;
        for(int i=0; i<=4; i++)
        {
            caster.AttackTarget(target, damage);
            Debug.Log("Wizard SnowBall "+ damage+" dmg");

            for(int p=0; p < enemyNum.Length; p++)
            {
                Character c = enemyNum[p] as Character;
                Character t = target as Character;
                bool hitcheck = EllipseScanner(3f, 1.3f, t.transform.position,c.transform.position);
                if(hitcheck && t.transform.position!= c.transform.position)
                {
                    caster.AttackTarget(enemyNum[p], splash);
                }
            }
        }
        Wizard_Passive.skillCount++;
        Wizard_Passive.skillResfresh = true;
    }

    #endregion

    #region EllipseScanner

    private bool EllipseScanner(float a, float b, Vector3 center, Vector3 targetPosition)
    {
        float dx = targetPosition.x - center.x;
        float dy = targetPosition.y - center.y;

        float l1 = Mathf.Sqrt((dx + Mathf.Sqrt(a * a - b * b)) * (dx + Mathf.Sqrt(a * a - b * b)) + (dy * dy));
        float l2 = Mathf.Sqrt((dx - Mathf.Sqrt(a * a - b * b)) * (dx - Mathf.Sqrt(a * a - b * b)) + (dy * dy));

        if (l1 + l2 <= 2 * a)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

}

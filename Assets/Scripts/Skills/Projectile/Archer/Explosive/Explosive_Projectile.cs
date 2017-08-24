using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Projectile {

    IBattleHandler[] enemyNum;

    private void Awake()
    {
        enemyNum = BattleManager.GetBattleManager().GetEntities(Team.Hostile);
    }

    public override void OnArrival(int abillity)
    {
        base.OnArrival(abillity);
        ExplosiveDamage(abillity);
    }

    public void ExplosiveDamage(int abillity)
    {
        int damage = 500;

        for(int i=0; i<= enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;
            Character t = caster.Target as Character;
            bool hitcheck = EllipseScanner(5f, 2.1f, t.transform.position, c.transform.position);
            if(hitcheck)
            {
                caster.AttackTarget(caster.Target, damage);
                switch(abillity)
                {
                    case 1:
                        // 타겟한테 5초간 공격력 10퍼 감소 디버프
                        break;

                    case 2:
                        // 타겟 2초간 속박
                        break;
                }
            }
        }
    }

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

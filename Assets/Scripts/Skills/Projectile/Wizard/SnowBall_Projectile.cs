using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall_Projectile : Projectile {

    IBattleHandler[] enemyNum;

    public override void OnArrival(int abiility)
    {
        SnowBall(abiility);
        base.OnArrival(abiility);
    }

    public void SnowBall(int abillity)
    {
        enemyNum = BattleManager.GetBattleManager().GetEntities(Team.Hostile);
        switch (abillity)
        {
            case 1:
                int damage1 = (int)(120 * Wizard_Passive.mag);
                int splash1 = 60;
                caster.AttackTarget(caster.Target, damage1);

                CameraShake.GetCameraShakeSystem().CamShakeOn(0.1f, 0.1f);

                Wizard_Snowball_Abillity1.abillity1count++;
                if(Wizard_Snowball_Abillity1.abillity1count == 5)
                {
                    Debug.Log(caster.Target + " 기절");
                    Wizard_Snowball_Abillity1.abillity1count = 0;
                   // caster.Target.기절
                }
                Debug.Log("Wizard SnowBall " + damage1 + " dmg");

                for (int p = 0; p < enemyNum.Length; p++)
                {
                    Character c = enemyNum[p] as Character;
                    Character t = caster.Target as Character;
                    bool hitcheck = EllipseScanner(3f, 1.3f, t.transform.position, c.transform.position);
                    if (hitcheck && t.transform.position != c.transform.position)
                    {
                        caster.AttackTarget(enemyNum[p], splash1);
                    }
                }
                break;

            case 2:
                int damage2 = (int)(20 * (Wizard_Snowball_Abillity2.abillity2count+2) * Wizard_Passive.mag);
                int splash2 = damage2/2;
                Wizard_Snowball_Abillity2.abillity2count++;
                caster.AttackTarget(caster.Target, damage2);
                Debug.Log("Wizard SnowBall " + damage2 + " dmg");

                for (int p = 0; p < enemyNum.Length; p++)
                {
                    Character c = enemyNum[p] as Character;
                    Character t = caster.Target as Character;
                    bool hitcheck = EllipseScanner(3f, 1.3f, t.transform.position, c.transform.position);
                    if (hitcheck && t.transform.position != c.transform.position)
                    {
                        caster.AttackTarget(enemyNum[p], splash2);
                    }
                }
                break;
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

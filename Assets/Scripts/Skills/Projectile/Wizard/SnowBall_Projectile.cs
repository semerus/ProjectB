using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall_Projectile : Projectile {

    public override void OnArrival(int abiility)
    {
        SnowBall();
        base.OnArrival(abiility);
    }

    public void SnowBall()
    {
		IBattleHandler[] enemyNum = BattleManager.GetBattleManager().GetEntities(Team.Hostile);

		int finalDamage = (int)(damage * Wizard_Passive.mag);
		caster.AttackTarget (target, finalDamage);

		for (int i = 0; i < enemyNum.Length; i++)
		{
			bool hitcheck = Scanner.EllipseScanner(3f, 1.3f, enemyNum[i].Transform.position, target.Transform.position);
			if (hitcheck && enemyNum[i] != target)
			{
				caster.AttackTarget(enemyNum[i], (int)finalDamage/2);
			}
		}

		CameraShake.GetCameraShakeSystem ().CamShakeOn (0.1f, 0.1f);

		/*
        enemyNum = BattleManager.GetBattleManager().GetEntities(Team.Hostile);
        switch (abillity)
        {
            case 1:
                int damage1 = (int)(120 * Wizard_Passive.mag);
                int splash1 = 60;
				caster.AttackTarget(target, damage1);

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
                    Character t = target as Character;
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
				caster.AttackTarget(target, damage2);
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
        */
    }
}

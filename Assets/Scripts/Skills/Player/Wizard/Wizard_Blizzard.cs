using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Blizzard : Skill
{
    IBattleHandler[] enemyNum;
    Vector3 targetPosition=new Vector3();
    float skilltime = 0;
    float [] inTime =new float[10];
    int ability = 2;

    public override void Activate(IBattleHandler target)
    {
        ResetSetting();
        StartCoolDown();
    }

    public override void RunTime()
    {
        base.RunTime();
        Blizzard();
    }

    public void ResetSetting()
    {
        cooldown = 10;
        enemyNum = BattleManager.GetBattleManager().GetEntities(Team.Hostile);
        for (int i = 0; i < enemyNum.Length; i++)
        {
            inTime[i] = 0;
        }
        FindPosition();
        skilltime = 0;
    }

    public void Blizzard()
    {
        if (skilltime<=5)
        {
            skilltime += Time.deltaTime;

            switch(ability)
            {
                case 1:
                    break;

                case 2:
                    Blizzard_ability2();
                    break;
            }
        }
    }

    public void FindPosition()
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
        targetPosition = new Vector3(tx, this.gameObject.transform.position.y,0);
    }

    #region Blizzard_ability2

    public void Blizzard_ability2()
    {
        Gravity_2();
        Damage();
    }

    public void Gravity_2()
    {
        for(int i=0; i< enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;
            bool hitcheck = EllipseScanner(4f, 1.8f, targetPosition, c.transform.position );
            if(hitcheck)
            {
                float dx = targetPosition.x - c.transform.position.x;
                float dy = targetPosition.y - c.transform.position.y;

                c.transform.position += (new Vector3(dx, dy, 0)) * Time.deltaTime;
            }
        }
    }

    #endregion

    public void Damage()
    {
        int damage = 50;
        for (int i = 0; i < enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;
            bool hitcheck = EllipseScanner(4f, 1.8f, targetPosition, c.transform.position);
            if (hitcheck)
            {
                inTime[i] += Time.deltaTime;

                if(inTime[i]>=1)
                {
                    inTime[i] = 0;
                    caster.AttackTarget(enemyNum[i], damage);
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

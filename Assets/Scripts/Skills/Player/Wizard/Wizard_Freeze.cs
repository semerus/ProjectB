using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Freeze : HeroActive
{
    float skilltime = 0;
    int skillCount = 0;
    IBattleHandler[] enemyNum;
    IBattleHandler target = null;
    Vector3 targetPosition = new Vector3();

    public override void Activate(IBattleHandler target)
    {
        ResetSetting();
        if (target != null)
        {
            StartCoolDown();
        }
    }

    public override void RunTime()
    {
        base.RunTime();
        Freeza_abillity2();
    }

    public void ResetSetting()
    {
        cooldown = 10;
        enemyNum = BattleManager.GetBattleManager().GetEntities(Team.Hostile);
        skilltime = 0;
        skillCount = 0;
        cooldown = 10;
        target = caster.Target;
        Character t = target as Character;
        targetPosition = t.transform.position;
    }

    #region Freeza_abillity1

    public void Freeza_abillity1()
    {
        if (skillCount <= 5)
        {
            skilltime += Time.deltaTime;
            if (skilltime >= 1)
            {
                Freeza_abillity1_On();
                skilltime = 0;
            }
        }
    }

    public void Freeza_abillity1_On()
    {
        int damage = 10;

        for (int i = 0; i < enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;
            caster.AttackTarget(enemyNum[i], damage);
            // 이속 공속 20퍼 감소 디버프
        }
    }

    #endregion

    #region Freeza_abillity2

    public void Freeza_abillity2()
    {
        if(skillCount <= 2 && target!=null)
        {
            skilltime += Time.deltaTime;
            if (skilltime >= 1)
            {
                Freeza_abillity2_On();
                skilltime = 0;
            }
        }
    }

    public void Freeza_abillity2_On()
    {
        int damage = (int) (120* Wizard_Passive.mag);

        for (int i = 0; i < enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;
            bool hitcheck = EllipseScanner(0.6f, 0.3f, targetPosition, c.transform.position);
            if (hitcheck)
            {
                caster.AttackTarget(enemyNum[i], damage);
            }
        }
        skillCount++;

        if (skillCount == 3)
        {
            Wizard_Passive.skillCount++;
            Wizard_Passive.skillResfresh = true;
        }

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

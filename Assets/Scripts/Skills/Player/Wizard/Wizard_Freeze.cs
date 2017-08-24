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
    GameObject fl;

	void Awake() {
		caster = gameObject.GetComponent<Character> ();
		Hero h = caster as Hero;
		if (h != null) {
			h.activeSkills [1] = this;
		}

		button = Resources.Load<Sprite> ("Skills/Heroes/Wizard/Wizard_Skill2");
	}

    public void EffectOn()
    {
        fl = Instantiate(Resources.Load<GameObject>("Skills/Heroes/Wizard/Freeze/Freeze_abillity1"));
        fl.transform.position = new Vector3(0, 0, 0);
        fl.SetActive(true);
    }

    public override void Activate()
    {
        Wizard_Passive.skillCount++;
        ResetSetting();
        StartCoolDown();
        UpdateSkillStatus(SkillStatus.ProcessOn);
        EffectOn();
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
        if(t!=null)
        {
            targetPosition = t.transform.position;
        }
    }

    #region Freeza_abillity1

    public void Freeza_abillity1()
    {
        if (skillCount <= 4)
        {
            skilltime += Time.deltaTime;
            if (skilltime >= 1)
            {
                skillCount++;
                Freeza_abillity1_On();
                skilltime = 0;
            }
        }
        else
        {
            Destroy(fl);
            UpdateSkillStatus(SkillStatus.ProcessOff);
        }
    }

    public void Freeza_abillity1_On()
    {
        int damage = (int)(100 * Wizard_Passive.mag);

        for (int i = 0; i < enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;
            if(c.gameObject.active)
            {
                caster.AttackTarget(enemyNum[i], damage);
                // 이속 공속 20퍼 감소 디버프
            }
        }
    }

    #endregion

    #region Freeza_abillity2

    public void Freeza_abillity2()
    {
        if(skilltime==0)
        {
            int damage = (int)(120 * Wizard_Passive.mag);
            for (int i = 0; i < enemyNum.Length; i++)
            {
                Character c = enemyNum[i] as Character;
                if (c.gameObject.active)
                {
                    caster.AttackTarget(enemyNum[i], damage);
                    // 이동불가 디버프
                }
            }
        }

        skilltime += Time.deltaTime;
        
        if(skilltime>=3)
        {
            Destroy(fl);
            Wizard_Passive.skillCount++;
            Wizard_Passive.skillResfresh = true;
            UpdateSkillStatus(SkillStatus.ProcessOff);
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Snowball : HeroActive,IPooling_Character
{
    IBattleHandler[] enemyNum;
    IBattleHandler target;
    Projectile[] projectile = new Projectile[5];
    float skilltime = 0;
    float shoottime = 0.2f;
    int num = 0;
    int count = 0;
    float r = 1f;

    void Awake() {
		button = Resources.Load<Sprite> ("Skills/Heroes/Wizard/Wizard_Skill1");
    }

    #region Projectile

    public void SetSnowProjectile()
    {
        for(int i=0; i<=4; i++)
        {
            GameObject p = Instantiate(Resources.Load<GameObject>("snow"));
            p.transform.SetParent(GameObject.Find("Projectiles").transform);
            projectile[i] = p.gameObject.GetComponent<Projectile>();
            projectile[i].gameObject.SetActive(false);
        }
    }

    public void SnowProjectileRoundOn()
    {
        if (CheckSkillStatus(SkillStatus.ProcessMask))
        {
            skilltime += Time.deltaTime;
            for (int i = 0; i <= 4; i++)
            {
                float rspeed = i + 0.5f * skilltime;

                float cpx = r * Mathf.Sin(2 * rspeed * Mathf.PI / 5);
                float cpy = r * Mathf.Cos(2 * rspeed * Mathf.PI / 5) + 0.3f;
                Vector3 target = caster.transform.position;
                Vector3 position = new Vector3(cpx, cpy, 0f);
                projectile[i].transform.position = position + target;

                if (!projectile[i].gameObject.active && (int)skilltime == i + 1)
                {
                    projectile[i].ProjectileOn();
                    if(i==4)
                    {
                        UpdateSkillStatus(SkillStatus.ProcessOff);
                    }
                }
            }
        }
        else
        {
            SnowProjectileShoot();
        }
    }

    public void SnowProjectileShoot()
    {
        shoottime += Time.deltaTime;
        if(shoottime>=0.2&&count<=4)
        {
            projectile[count].ProjectileMove(caster.Target as Character, 30);
            shoottime = 0;
            count++;
        }
    }

    public void SnowProjectileDamage()
    {
        for (int i = 0; i <= 4; i++)
        {
            if (projectile[i].CheckArrival())
            {
                SnowBall();
                if(i==4)
                {
                    ActivePassive();
                }
                projectile[i].EndProjectile();
            }
        }
    }

    #endregion

    public override void RunTime()
    {
        base.RunTime();
        Debug.Log(CheckSkillStatus(SkillStatus.ProcessMask));
        SnowProjectileRoundOn();
        SnowProjectileDamage();
    }

    public override void Activate(IBattleHandler target)
    {
        ResetSetting();
        UpdateSkillStatus(SkillStatus.ProcessOn);
        if (num==0)
        {
            SetSnowProjectile();
            num = 1;
        }
        if (target!=null)
        {
            StartCoolDown();
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
        skilltime = 0;
    }

    #region SnowBall_Skill

    public void SnowBall()
    {
        int damage = (int)(500 * Wizard_Passive.mag);
        int splash = 250;

        caster.AttackTarget(target, damage);
        Debug.Log("Wizard SnowBall " + damage + " dmg");

        for (int p = 0; p < enemyNum.Length; p++)
        {
            Character c = enemyNum[p] as Character;
            Character t = target as Character;
            bool hitcheck = EllipseScanner(3f, 1.3f, t.transform.position, c.transform.position);
            if (hitcheck && t.transform.position != c.transform.position)
            {
                caster.AttackTarget(enemyNum[p], splash);
            }
        }
    }

    public void ActivePassive()
    {
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

    #region None
    public Stack<IPooledItem_Character> Pool
    {
        get
        {
            throw new NotImplementedException();
        }
    }
#endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Snowball : HeroActive,IPooling_Character,IChanneling
{
    IBattleHandler[] enemyNum;
    IBattleHandler target;
    Projectile[] projectile = new Projectile[5];
    float skilltime = 0;
    float shoottime = 0.2f;
    int count = 0;
    float r = 1f;

    void Awake() {
		button = Resources.Load<Sprite> ("Skills/Heroes/Wizard/Wizard_Skill1");
        SetSnowProjectile();
        cooldown = 10f;
    }

    protected override void OnProcess()
    {
        Debug.Log("sss");
        SnowProjectileShoot();
    }

    public void OnChanneling()
    {
        caster.ChangeAction(CharacterAction.Channeling);

        UpdateSkillStatus(SkillStatus.ChannelingOn);

        SnowProjectileRoundOn();
    }

    public void OnInterrupt(IBattleHandler interrupter)
    {
        StartCoolDown();
        UpdateSkillStatus(SkillStatus.ChannelingOff);
        caster.ChangeAction(CharacterAction.Attacking);
        ResetSetting();
    }

    #region Projectile

    public void SetSnowProjectile()
    {
        for(int i=0; i<=4; i++)
        {
            GameObject p = Instantiate(Resources.Load<GameObject>("Skills/Heroes/Wizard/SnowBall/Snowball"));
            p.transform.SetParent(GameObject.Find("Projectiles").transform);
            projectile[i] = p.gameObject.GetComponent<Projectile>();
            projectile[i].gameObject.SetActive(false);
        }
    }

    public void SnowProjectileRoundOn()
    {
        if (CheckSkillStatus(SkillStatus.ChannelingMask))
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
                    projectile[i].ProjectileOn(caster);
                    projectile[count].enabled = false;
                    if (i == 4)
                    {
                        StartCoolDown();
                        UpdateSkillStatus(SkillStatus.ChannelingOff);
                        UpdateSkillStatus(SkillStatus.ProcessOn);
                    }
                }
            }
        }
    }

    public void SnowProjectileShoot()
    {
        shoottime += Time.deltaTime;
        if (shoottime>=0.2&&count<=5)
        {
            projectile[count].enabled = true;
            projectile[count].ProjectileMove(caster.Target as Character, 30);
            shoottime = 0;
            count++;
        }
        if(count>=5)
        {
            UpdateSkillStatus(SkillStatus.ProcessOff);
            caster.ChangeAction(CharacterAction.Attacking);
            ResetSetting();
        }
    }

    #endregion

    public void ResetSetting()
    {
        cooldown = 10f;
        target = caster.Target;
        skilltime = 0;
        shoottime = 0;
        count = 0;
    }

    public void ActivePassive()
    {
        Wizard_Passive.skillCount++;
        Wizard_Passive.skillResfresh = true;
    }

    #region None
    public override void Activate(IBattleHandler target)
    {

    }

    public Stack<IPooledItem_Character> Pool
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public float ChannelTime
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public float Timer_Channeling { get ; set ; }
    #endregion
}

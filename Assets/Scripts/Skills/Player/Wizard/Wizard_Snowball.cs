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
    int snowStack = 0;
    float r = 1f;
    private int start = 0;
    CameraShake a;

    void Awake() {
		button = Resources.Load<Sprite> ("Skills/Heroes/Wizard/Wizard_Skill1");
        SetSnowProjectile();
        cooldown = 10f;
        
    }
    
    public virtual void OnChanneling()
    {
        if (start == 0)
        {
            ResetSetting();
            start = 1;
        }
        caster.ChangeAction(CharacterAction.Channeling);
        UpdateSkillStatus(SkillStatus.ChannelingOn);
    }

    public virtual void OnInterrupt(IBattleHandler interrupter)
    {
        StartCoolDown();
        UpdateSkillStatus(SkillStatus.ChannelingOff);
        start = 0;
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

    public void SnowProjectileRoundOn(int abillity)
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

                switch(abillity)
                {
                    case 1:
                        if (!projectile[i].gameObject.active && (int)(skilltime-0.1*i) == i + 1)
                        {
                            projectile[i].ProjectileOn(caster,abillity);
                            if (i == 4)
                            {
                                StartCoolDown();
                                UpdateSkillStatus(SkillStatus.ChannelingOff);
                                UpdateSkillStatus(SkillStatus.ProcessOn);
                                SlowMotion();
                            }
                        }
                        break;

                    case 2:
                        if (!projectile[i].gameObject.active && (int)skilltime == i + 1)
                        {
                            projectile[i].ProjectileOn(caster,abillity);
                            snowStack++;
                            if (i == 4)
                            {
                                StartCoolDown();
                                UpdateSkillStatus(SkillStatus.ChannelingOff);
                                UpdateSkillStatus(SkillStatus.ProcessOn);
                                SlowMotion();
                            }
                        }
                        break;
                }
            }
        }
    }

    public void SnowProjectileShoot(int abillity)
    {
        shoottime += Time.deltaTime;

        switch(abillity)
        {
            case 1:
                if (shoottime >= 0.4 && count <= 5)
                {
                    projectile[count].ProjectileMove(caster.Target as Character, 36);
                    shoottime = 0;
                    count++;
                }
                if (count >= 5)
                {
                    UpdateSkillStatus(SkillStatus.ProcessOff);
                    caster.ChangeAction(CharacterAction.Attacking);
                    TimeSystem.GetTimeSystem().UnSlowMotion();
                    ResetSetting();
                }
                break;

            case 2:
                if (shoottime >= 0.2 && count < snowStack)
                {
                    projectile[count].ProjectileMove(caster.Target as Character, 36);
                    shoottime = 0;
                    count++;
                }
                if (shoottime>=0.2&& count >= snowStack)
                {
                    UpdateSkillStatus(SkillStatus.ProcessOff);
                    caster.ChangeAction(CharacterAction.Attacking);
                    TimeSystem.GetTimeSystem().UnSlowMotion();
                    ResetSetting();
                    Wizard_Snowball_Abillity2.abillity2count = 0;
                }
                break;  
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
        snowStack = 0;
    }

    public void ActivePassive()
    {
        Wizard_Passive.skillCount++;
        Wizard_Passive.skillResfresh = true;
    }

    #region None
    public override void Activate()
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

    void SlowMotion()
    {
        List<ITimeHandler> temp = new List<ITimeHandler>();
        temp.Add(this);
        temp.Add(this.caster);
        for(int i = 0; i < projectile.Length; i++)
        {
            temp.Add(projectile[i]);
        }

        ITimeHandler[] nonSlow = new ITimeHandler[temp.Count];
        temp.CopyTo(nonSlow);
        TimeSystem.GetTimeSystem().SlowMotion(nonSlow);
    }
}

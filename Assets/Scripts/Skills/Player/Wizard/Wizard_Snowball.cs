using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Snowball : HeroActive, IPooling_Character, IChanneling
{
    protected IBattleHandler target;
	protected SnowBall_Projectile[] projectile = new SnowBall_Projectile[5];
    protected int damage;
    protected float speed;
    protected float skilltime = 0;

    const float r = 1f;

	/*************************/

	protected float shootTimer = 0f;
	protected float spinTimer = 0f;
	protected float channelTimer = 0f;
	protected int ballCount = 0;
	protected int shootCount = 0;
	protected float createTime = 1f;
	protected float shootTime = 0.2f;


	#region IChanneling implementation

	public virtual void OnChanneling()
	{
		channelTimer += Time.deltaTime;
		spinTimer += Time.deltaTime;

		if (channelTimer > createTime) {
			channelTimer = 0f;
			CreateSnowBall ();
		}
		TurnSnowBall ();
	}

	public virtual void OnInterrupt(IBattleHandler interrupter)
	{
		Wizard_Passive.stackOff = true;
		StartCoolDown();
		UpdateSkillStatus (SkillStatus.ChannelingOff);
		ResetSetting ();
	}

	#endregion

    void Awake() {
		caster = gameObject.GetComponent<Character> ();
		Hero h = caster as Hero;
		if (h != null) {
			h.activeSkills [0] = this;
		}
		button = Resources.Load<Sprite> ("Skills/Heroes/Wizard/Wizard_Skill1");
    }

    public override void SetSkill(Dictionary<string, object> param)
    {
        base.SetSkill(param);
        damage = (int)param["damage"];
        speed = (float)((double)param["speed"]);
        SetSnowProjectile();
    }

	public override bool CheckCondition ()
	{
		bool isReady = false;
		if (target == null) {
			Hero hero = caster as Hero;
			if (hero != null) {
				Wizard_AutoAttack auto = hero.autoAttack as Wizard_AutoAttack;
				if (auto != null) {
					target = auto.AtkTarget;
				}
			}
		}

		if (target != null) {
			if (target.Action == CharacterAction.Dead) {
				target = null;
			} else {
				isReady = true;
			}
		}

		return isReady && base.CheckCondition ();
	}

	protected override void OnProcess ()
	{
		base.OnProcess ();
		shootTimer += Time.deltaTime;

		if (shootTimer > shootTime) {
			shootTimer = 0f;
			ShootSnowBall ();
		}
	}

    #region Projectile

	protected virtual void SetSnowProjectile() {
        for(int i = 0; i < 5; i++) {
            GameObject p = Instantiate(Resources.Load<GameObject>("Skills/Heroes/Wizard/SnowBall/Snowball"));
            p.transform.SetParent(GameObject.Find("Projectiles").transform);
			projectile[i] = p.gameObject.GetComponent<SnowBall_Projectile>();
            projectile[i].gameObject.SetActive(false);
        }
    }

	protected virtual void CreateSnowBall() {
		if (ballCount < 4) {
			SetSnowBall (projectile [ballCount++]);
		} else {
			// if count is 5 shoot
			SetSnowBall (projectile [ballCount]);

			StartCoolDown();
			UpdateSkillStatus(SkillStatus.ChannelingOff);
			UpdateSkillStatus(SkillStatus.ProcessOn);
			SlowMotion();
		}
	}

	protected void SetSnowBall(SnowBall_Projectile ball) {
		ball.ProjectileOn (caster);
		ball.SetProjectile (target, damage, speed);
	}

	private void TurnSnowBall() {
		for (int i = 0; i < 5; i++) {
			float rspeed = i + 0.5f * spinTimer;
			float cpx = r * Mathf.Sin(2 * rspeed * Mathf.PI / 5);
			float cpy = r * Mathf.Cos(2 * rspeed * Mathf.PI / 5) + 0.3f;
			Vector3 target = caster.transform.position;
			Vector3 position = new Vector3(cpx, cpy, 0f);
			projectile[i].transform.position = position + target;
		}
	}

	protected void ShootSnowBall() {
		projectile [shootCount++].ProjectileMove ();
		if (shootCount > ballCount) {
			UpdateSkillStatus(SkillStatus.ProcessOff);
			caster.ChangeAction(CharacterAction.Idle);
			TimeSystem.GetTimeSystem().UnSlowMotion();
			Background.GetBackground ().SetDark (false);
			ResetSetting();
		}
	}

	/*
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
					if (!projectile[i].gameObject.activeSelf && (int)(skilltime-0.1*i) == i + 1)
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
					if (!projectile[i].gameObject.activeSelf && (int)skilltime == i + 1)
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
    */

	/*
    public void SnowProjectileShoot(int abillity)
    {
        shootTime += Time.deltaTime;

        switch(abillity)
        {
            case 1:
                if (shootTime >= 0.4 && count <= 5)
                {
					projectile [count].SetProjectile (target, damage, speed);
					projectile[count].ProjectileMove(target as Character);
                    shootTime = 0;
                    count++;
                }
                if (count >= 5)
                {
                    UpdateSkillStatus(SkillStatus.ProcessOff);
                    caster.ChangeAction(CharacterAction.Idle);
                    TimeSystem.GetTimeSystem().UnSlowMotion();
                    ResetSetting();
                }
                break;

            case 2:
                if (shootTime >= 0.2 && count < snowStack)
                {
					projectile [count].SetProjectile (target, damage, speed);				
					projectile[count].ProjectileMove(target as Character);
                    shootTime = 0;
                    count++;
                }
                if (shootTime>=0.2&& count >= snowStack)
                {
                    UpdateSkillStatus(SkillStatus.ProcessOff);
                    caster.ChangeAction(CharacterAction.Idle);
                    TimeSystem.GetTimeSystem().UnSlowMotion();
                    ResetSetting();
                    Wizard_Snowball_Abillity2.abillity2count = 0;
                }
                break;  
        }
    }
	*/

    #endregion

    public void ResetSetting() {
		shootTimer = 0f;
		spinTimer = 0f;
		channelTimer = 0f;
		ballCount = 0;
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

    protected void SlowMotion()
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

		Background.GetBackground ().SetDark (true);
    }
}

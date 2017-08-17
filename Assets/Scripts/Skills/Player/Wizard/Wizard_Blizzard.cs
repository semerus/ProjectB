using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard_Blizzard : HeroActive,IChanneling
{
    IBattleHandler[] enemyNum;
    Vector3 targetPosition=new Vector3();
    float channeling = 0;
    float skilltime = 0;
    float explosiontime = 0;
    float [] inTime =new float[10];
    int count = 0;
    Character[] target = new Character[10];
    bool[] FacingLeft = new bool[10];
    GameObject bl;

<<<<<<< HEAD
    void Awake() {
=======
	void Awake() {
		caster = gameObject.GetComponent<Character> ();
		Hero h = caster as Hero;
		if (h != null) {
			h.activeSkills [2] = this;
		}

>>>>>>> 96a441a56d03b4f6eda8cbf73eb63b00e7d93ad2
		button = Resources.Load<Sprite> ("Skills/Heroes/Wizard/Wizard_Skill3");
    }

    public void BlizzardPrefabOn(int abillity)
    {
        switch (abillity)
        {
            case 1:
                bl = Instantiate(Resources.Load<GameObject>("Skills/Heroes/Wizard/Blizzard/Blizzard_Abillity1"));
                break;

            case 2:
                bl = Instantiate(Resources.Load<GameObject>("Skills/Heroes/Wizard/Blizzard/Blizzard_Abillity2"));
                break;
        }
        bl.gameObject.SetActive(true);
        bl.gameObject.transform.position = targetPosition + new Vector3(0,2.2f,0);
    }

    public void BlizzardPrefabOff()
    {
        Destroy(bl);
    }

    public virtual void OnChanneling()
    {
        caster.ChangeAction(CharacterAction.Channeling);
        UpdateSkillStatus(SkillStatus.ChannelingOn);
    }

    public virtual void OnInterrupt(IBattleHandler interrupter)
    {
        Wizard_Passive.stackOff = true;
        ResetSetting();
        StartCoolDown();
        UpdateSkillStatus(SkillStatus.ChannelingOff);
    }

    public void SkillChanneling(int abillity)
    {
        channeling += Time.deltaTime;
        switch (abillity)
        {
            case 1:
                if(channeling>=4)
                {
                    ResetSetting();
                    StartCoolDown();
                    UpdateSkillStatus(SkillStatus.ChannelingOff);
                    UpdateSkillStatus(SkillStatus.ProcessOn);
                    BlizzardPrefabOn(abillity);
                    caster.ChangeAction(CharacterAction.Idle);
                }
                break;

            case 2:
                if (channeling >= 3)
                {
                    ResetSetting();
                    StartCoolDown();
                    UpdateSkillStatus(SkillStatus.ChannelingOff);
                    UpdateSkillStatus(SkillStatus.ProcessOn);
                    BlizzardPrefabOn(abillity);
                    caster.ChangeAction(CharacterAction.Idle);
                }
                break;
        }
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
        count = 0;
        explosiontime = 0;
        channeling = 0;
    }

    public void Blizzard(int abiliity)
    {
        if (skilltime<=5)
        {
            skilltime += Time.deltaTime;
            Gravity(abiliity);
            Damage(abiliity);
        }
        else
        {
            Explosion(abiliity);
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

    public void Gravity(int abillity)
    {
        for(int i=0; i< enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;
            bool hitcheck=false;

            switch (abillity)
            {
                case 1:
                    hitcheck = EllipseScanner(3f, 1.3f, targetPosition, c.transform.position);
                    break;

                case 2:
                    hitcheck = EllipseScanner(4f, 1.8f, targetPosition, c.transform.position);
                    break;
            }
            
            if(hitcheck)
            {
                float dx = targetPosition.x - c.transform.position.x;
                float dy = targetPosition.y - c.transform.position.y;

                c.transform.position += (new Vector3(dx, dy, 0)) * Time.deltaTime;
            }
        }
    }

    public void ExplosionCheck()
    {
        for (int i = 0; i < enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;

            bool hitcheck = false;

            hitcheck = EllipseScanner(3f, 1.3f, targetPosition, c.transform.position);

            if (hitcheck)
            {
                target[count] = c;

                if (c.IsFacingLeft)
                {
                    FacingLeft[count] = true;
                }
                else
                {
                    FacingLeft[count] = false;
                }
                count++;
            }
        }
    }

    public bool CheckBoundary(Character target)
    {
        Vector3 position = target.transform.position;
        float cpx = position.x;
        float cpy = position.y;
        float lx = GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().size.x / 2;
        float ly = (GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().size.y / 2) + GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().offset.y;

        if ((cpx<=lx && cpx >= -lx)&&(cpy<ly && cpy > -ly))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Explosion(int abillity)
    {
        if(explosiontime==0)
        {
            LastDamage(abillity);
        }

        explosiontime += Time.deltaTime;
        if (explosiontime<=0.2)
        {
            for (int i = 0; i < count; i++)
            {
                target[i].StopMove();
                if (FacingLeft[i])
                {
                    if (CheckBoundary(target[i]))
                    {
                        target[i].transform.position += new Vector3(20, 0, 0) * Time.deltaTime;
                    }
                }
                else
                {
                    if (CheckBoundary(target[i]))
                    {
                        target[i].transform.position -= new Vector3(20, 0, 0) * Time.deltaTime;
                    }
                }
            }
        }
        else
        {
            BlizzardPrefabOff();
            UpdateSkillStatus(SkillStatus.ProcessOff);
        }
    }


    public void LastDamage(int abillity)
    {
        int damage = 50;

        for (int i = 0; i < enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;

            bool hitcheck = false;

            switch (abillity)
            {
                case 1:
                    hitcheck = EllipseScanner(3f, 1.3f, targetPosition, c.transform.position);
                    ExplosionCheck();
                    break;
            }
            if (hitcheck)
            {
                caster.AttackTarget(enemyNum[i], damage);
            }
        }
    }

    public void Damage(int abillity)
    {
        int damage = 50;
        for (int i = 0; i < enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;

            bool hitcheck = false;

            switch (abillity)
            {
                case 1:
                    hitcheck = EllipseScanner(3f, 1.3f, targetPosition, c.transform.position);
                    break;

                case 2:
                    hitcheck = EllipseScanner(4f, 1.8f, targetPosition, c.transform.position);
                    break;
            }

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

    #region None
    public float ChannelTime
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public float Timer_Channeling { get; set; }
#endregion

}

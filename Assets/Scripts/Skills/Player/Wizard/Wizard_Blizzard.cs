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
    float [] inTime =new float[10];

    void Awake() {
		button = Resources.Load<Sprite> ("Skills/Heroes/Wizard/Wizard_Skill3");
    }

    public virtual void OnChanneling()
    {
        caster.ChangeAction(CharacterAction.Channeling);
        UpdateSkillStatus(SkillStatus.ChannelingOn);
    }

    public virtual void OnInterrupt(IBattleHandler interrupter)
    {
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
                }
                break;

            case 2:
                if (channeling >= 3)
                {
                    ResetSetting();
                    StartCoolDown();
                    UpdateSkillStatus(SkillStatus.ChannelingOff);
                    UpdateSkillStatus(SkillStatus.ProcessOn);
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
            LastDamage(abiliity);
            UpdateSkillStatus(SkillStatus.ProcessOff);
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

    public void Explosion()
    {
        for (int i = 0; i < enemyNum.Length; i++)
        {
            Character c = enemyNum[i] as Character;

            bool hitcheck = false;

            hitcheck = EllipseScanner(3f, 1.3f, targetPosition, c.transform.position);

            if (hitcheck)
            {
                c.StopMove();
                if(c.IsFacingLeft)
                {
                    Vector3 t = c.transform.position + new Vector3(6, 0, 0);
                    Debug.Log(c.transform.position+"         "+ t);

                    c.BeginMove(t, 30, 0);
                }
                else
                {
                    c.BeginMove(c.transform.position + new Vector3(-6, 0, 0), 1, 0);
                }
            }
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
                    Explosion();
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

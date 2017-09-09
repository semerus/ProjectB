using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk4 : Skill
{
	int damage;
	int count = 0;
    int mod = 1;
    int startNum = 0;
    bool sk4_On = false;
    IBattleHandler[] friendlyNum;
    float[] burnTimer = new float[10];
    bool[] burnCheck = new bool [10];

    void Awake() {
		caster = gameObject.GetComponent<Character> ();
	}

    public override void SetSkill (Dictionary<string, object> param)
	{
		base.SetSkill (param);
		this.damage = (int)param ["damage"];
	}

    public override void RunTime()
    {
        base.RunTime();
        if(CheckSkillStatus(SkillStatus.ProcessMask))
        {
            Sk4();
        }
    }

    public override void Activate()
    {
        for(int i=0; i<10;i++)
        {
            burnCheck[i] = false;
            burnTimer[i] = 0f;
        }
        caster.StopMove();
		// order is important should go after stop move
		OgreFemale of = caster as OgreFemale;
		if (of != null) {
			of.SetPattern (4);
		}
        UpdateSkillStatus(SkillStatus.ProcessOn);
        //caster.RefreshStatus(CharacterStatus.);
        cooldown = 20f;
        StartCoolDown();
        friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
    }

    private void Sk4()
    {
        BurnRun();
        EdgeChange();
        BurnBurn();
        Debug.Log(count);
    }

    private void BurnBurn()
    {
        for (int i = 0; i < friendlyNum.Length; i++)
        {
            Character c = friendlyNum[i] as Character;
            bool hitCheck = EllipseScanner(2.6f, 0.9f, this.gameObject.transform.position, c.transform.position);

            if (hitCheck == true && !burnCheck[i])
            {
                burnCheck[i] = true;
                Debug.Log("Burn " + c.transform.name);
				caster.AttackTarget(c, damage / 10);
            }

            if (burnCheck[i] == true)
            {
                burnTimer[i] += Time.deltaTime;
                if (burnTimer[i] >= 2)
                {
                    burnCheck[i] = false;
                }
            }
        }
    }

	Buff_Stun stun0;
	Buff_Stun stun1;
    private void BurnRun()
    {
        float speed = 3 * Mathf.Sqrt(2);
        Vector3 target = FindTarget();
        caster.BeginMove (target, speed, speed);
        if(count>=6)
        {
            UpdateSkillStatus(SkillStatus.ProcessOff);
            caster.StopMove();
            sk4_On = false;
            count = 0;

			// stun both bosses
			OgreFemale f = caster as OgreFemale;
			stun0 = new Buff_Stun (2f, caster, caster);
			stun1 = new Buff_Stun (2f, caster, f.Partner);

            SkillEventArgs s = new SkillEventArgs(this.name, true);
            OnEndSkill(s);
        }
    }

    private Vector3 FindTarget()
    {
        BoxCollider2D controlArea = GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>();
        float lx = controlArea.size.x / 2 + controlArea.offset.x;
        float mlx = controlArea.offset.x - controlArea.size.x / 2;
        float ly = controlArea.size.y / 2 + controlArea.offset.y;
        float mly = controlArea.offset.y - controlArea.size.y / 2;

        Vector3 cPosition = this.gameObject.transform.position;

        Vector3 target = new Vector3();

        switch (mod)
        {
            case 1:
                if(ly - cPosition.y <= lx-cPosition.x)
                {
                    target.y = ly;
                    target.x = cPosition.x + (ly - cPosition.y);
                    return target;
                }
                else
                {
                    target.x = lx;
                    target.y = cPosition.y + (lx - cPosition.x);
                    return target;
                }

            case 2:
                if (lx - cPosition.x <= cPosition.y - mly)
                {
                    target.x = lx;
                    target.y = cPosition.y - (lx - cPosition.x);
                    return target;
                }
                else
                {
                    target.y = mly;
                    target.x = cPosition.x + (cPosition.y - mly);
                    return target;
                }

            case 3:
                if (cPosition.y - mly <= cPosition.x - mlx)
                {
                    target.y = mly;
                    target.x = cPosition.x - (cPosition.y - mly);
                    return target;
                }
                else
                {
                    target.x = mlx;
                    target.y = cPosition.y - (cPosition.x - mlx);
                    return target;
                }

            case 4:
                if(cPosition.x - mly <= ly - cPosition.y)
                {
                    target.x = mlx;
                    target.y = cPosition.y + (cPosition.x -mlx);
                    return target;
                }
                else
                {
                    target.y = ly;
                    target.x = cPosition.x - (ly - cPosition.y);
                    return target;
                }
            default:
                return new Vector3(0,0,0);
        }
        
    }

    private void EdgeChange()
    {
        BoxCollider2D controlArea = GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>();
        float lx = controlArea.size.x / 2 + controlArea.offset.x;
        float mlx = controlArea.offset.x - controlArea.size.x / 2;
        float ly = controlArea.size.y / 2 + controlArea.offset.y;
        float mly = controlArea.offset.y - controlArea.size.y / 2;
        Vector3 myPosition = this.gameObject.transform.position;
        if (count <= 6)
        {
            if (myPosition.y >= ly-0.5f)
            {
                if (mod == 1)
                {
                    mod = 2;
                    count++;
                }
                else if (mod==4)
                {
                    mod = 3;
                    count++;
                }
                
            }
            else if (myPosition.y <= mly+0.5)
            {
                if (mod == 3)
                {
                    mod = 4;
                    count++;
                }
                else if (mod==2)
                {
                    mod = 1;
                    count++;
                }
            }
            else if (myPosition.x >= lx-0.5f)
            {
                if (mod == 2)
                {
                    mod = 3;
                    count++;
                }
                if(mod==1)
                {
                    mod = 4;
                    count++;
                }
            }
            else if (myPosition.x <= mlx+0.5f)
            {
                if (mod == 4)
                {
                    mod = 1;
                    count++;
                }
                else if (mod==3)
                {
                    mod = 2;
                    count++;
                }
            }
        }
        else
        {

        }
    }

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
}

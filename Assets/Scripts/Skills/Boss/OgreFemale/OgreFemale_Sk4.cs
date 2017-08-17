using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk4 : Skill
{
    int count = 0;
    int mod = 1;
    int startNum = 0;
    bool sk4_On = false;
    IBattleHandler[] friendlyNum;
    float tx;
    float ty;

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
        float dx = GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().size.x / 2;
        float dy = (GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().size.y / 2) + GameObject.Find("Background").GetComponentInChildren<BoxCollider2D>().offset.y;
        tx = dx - 0.1f;
        ty = dy - 0.1f;
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
    }

    private void BurnBurn()
    {
        for (int i = 0; i < friendlyNum.Length; i++)
        {
            Character c = friendlyNum[i] as Character;
            bool burnCheck = EllipseScanner(2.6f, 0.9f, this.gameObject.transform.position, c.transform.position);

            if (burnCheck == true)
            {
                Debug.Log("Burn " + c.transform.name);
                // 불붙기
            }
        }
    }

    private void BurnRun()
    {
        float speed = 3 * Mathf.Sqrt(2);
        Vector3 target = FindTarget();
		caster.BeginMove (target, speed, speed);
        if(count>=6)
        {
            caster.StopMove();
            sk4_On = false;
            count = 0;
            SkillEventArgs s = new SkillEventArgs(this.name, true);
            OnEndSkill(s);
        }
    }

    private Vector3 FindTarget()
    {
        Vector3 cPosition = this.gameObject.transform.position;

        Vector3 target = new Vector3();

        switch (mod)
        {
            case 1:
                if(ty - cPosition.y <= tx-cPosition.x)
                {
                    target.y = ty;
                    target.x = cPosition.x + (ty - cPosition.y);
                    return target;
                }
                else
                {
                    target.x = tx;
                    target.y = cPosition.y + (tx - cPosition.x);
                    return target;
                }

            case 2:
                if (tx - cPosition.x <= cPosition.y + ty)
                {
                    target.x = tx;
                    target.y = -cPosition.y + (tx - cPosition.x);
                    return target;
                }
                else
                {
                    target.y = -ty;
                    target.x = cPosition.x + (ty + cPosition.y);
                    return target;
                }

            case 3:
                if (cPosition.y+ty <= cPosition.x+tx)
                {
                    target.y = -ty;
                    target.x = cPosition.x - (cPosition.y + ty);
                    return target;
                }
                else
                {
                    target.x = -tx;
                    target.y = cPosition.y + (cPosition.x + tx);
                    return target;
                }

            case 4:
                if(9+cPosition.x <= ty-cPosition.y)
                {
                    target.x = -tx;
                    target.y = cPosition.y + (cPosition.x + 9);
                    return target;
                }
                else
                {
                    target.y = ty;
                    target.x = cPosition.x - (ty - cPosition.y);
                    return target;
                }
            default:
                return new Vector3(0,0,0);
        }
    }

    private void EdgeChange()
    {
        Vector3 myPosition = this.gameObject.transform.position;
        if (count <= 6)
        {
            if (myPosition.y >= ty)
            {
                if (mod == 1)
                {
                    mod = 2;
                }
                else
                {
                    mod = 3;
                }
                count++;
            }
            if (myPosition.y <= -ty)
            {
                if (mod == 3)
                {
                    mod = 4;
                }
                else
                {
                    mod = 1;
                }
                count++;
            }
            if (myPosition.x >= tx)
            {
                if (mod == 2)
                {
                    mod = 3;
                }
                else
                {
                    mod = 4;
                }
                count++;
            }
            if (myPosition.x <= -tx)
            {
                if (mod == 4)
                {
                    mod = 1;
                }
                else
                {
                    mod = 2;
                }
                count++;
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

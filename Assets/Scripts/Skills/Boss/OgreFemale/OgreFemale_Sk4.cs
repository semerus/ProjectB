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

    public override void RunTime()
    {
        base.RunTime();
        Sk4();
        Debug.Log(sk4_On);

    }

    public override void Activate(IBattleHandler target)
    {
        if (startNum == 0)
        {
            friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
            Debug.Log(friendlyNum.Length);
            startNum = 1;

            sk4_On = true;
            if (!TimeSystem.GetTimeSystem().CheckTimer(this))
            {
                TimeSystem.GetTimeSystem().AddTimer(this);
            }
        }
    }

    private void Sk4()
    {
        if (sk4_On == true)
        {
            BurnRun();
            EdgeChange();
            BurnBurn();
        }
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
        caster.Move(target, speed, speed);
        if(count>=6)
        {
            caster.StopMove();
            sk4_On = false;
            count = 0;
            Debug.Log(target);
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
                if(3.4f-cPosition.y <= 9-cPosition.x)
                {
                    target.y = 3.4f;
                    target.x = cPosition.x + (3.4f - cPosition.y);
                    return target;
                }
                else
                {
                    target.x = 9f;
                    target.y = cPosition.x + (9f - cPosition.x);
                    return target;
                }

            case 2:
                if (9 - cPosition.x <= cPosition.y + 3.4f)
                {
                    target.x = 9f;
                    target.y = cPosition.y - (9 - cPosition.x);
                    return target;
                }
                else
                {
                    target.y = -3.4f;
                    target.x = cPosition.x + (3.4f + cPosition.y);
                    return target;
                }

            case 3:
                Debug.Log(mod);
                if (cPosition.y+3.14f <= cPosition.x+9)
                {
                    target.y = -3.4f;
                    target.x = cPosition.x - (cPosition.y + 3.4f);
                    return target;
                }
                else
                {
                    target.x = -9f;
                    target.y = cPosition.y + (cPosition.x + 9);
                    return target;
                }

            case 4:
                if(9+cPosition.x <= 3.4f-cPosition.y)
                {
                    target.x = -9f;
                    target.y = cPosition.y + (cPosition.x + 9);
                    return target;
                }
                else
                {
                    target.y = 3.4f;
                    target.x = cPosition.x - (3.4f - cPosition.y);
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
            if (myPosition.y >= 3.39)
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
            if (myPosition.y <= -3.39)
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
            if (myPosition.x >= 8.99)
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
            if (myPosition.x <= -8.99)
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

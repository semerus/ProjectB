using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk2 : Skill {

    IBattleHandler[] friendlyNum;
    Vector3 adjustpoint;
    Vector3 jumpdistance;
    Character maxC;
    IBattleHandler maxNum = null;

    private void Start()
    {
        cooldown = 10f;
    }

    public override void Activate(IBattleHandler target)
    {
        adjustpoint = this.gameObject.transform.position + Vector3.down;
        friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
        StartCoolDown();
        JumpAttackTargetting();
        JumpAttack();
    }
    void JumpAttack()
    {
        float x = Mathf.Abs(adjustpoint.x - this.gameObject.transform.position.x);
        float y = Mathf.Abs(adjustpoint.y - this.gameObject.transform.position.y);
        Vector3 s = new Vector3(x, y);
        caster.Move(adjustpoint, s.x, s.y);
        caster.MoveComplete += new EventHandler<MoveEventArgs>(OnMoveComplete);
    }

    public void JumpAttackTargetting()
    {
        float max = 0;
        for (int i = 0; i < friendlyNum.Length; i++)
        {
            Character c = friendlyNum[i] as Character;
            float x = this.gameObject.transform.position.x - c.transform.position.x;
            float y = this.gameObject.transform.position.y - c.transform.position.y;
            float distance = x * x + y * y;

            if (distance >= max)
            {
                max = distance;
                maxNum = friendlyNum[i];
            }
        }
        maxC = maxNum as Character;

        if (this.gameObject.transform.position.x >= maxC.transform.position.x)
        {
            adjustpoint = maxC.transform.position;
            adjustpoint.x = maxC.transform.position.x + 1.25f;
            jumpdistance = adjustpoint - this.gameObject.transform.position;
        }
        else
        {
            adjustpoint = maxC.transform.position;
            adjustpoint.x = maxC.transform.position.x - 1.25f;
            jumpdistance = adjustpoint - this.gameObject.transform.position;
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

    void OnMoveComplete(object sender, EventArgs e)
    {
        MoveEventArgs m = e as MoveEventArgs;
        if (m != null)
        {
            if(m.result)
            {
                for (int i = 0; i < friendlyNum.Length; i++)
                {
                    Character c = friendlyNum[i] as Character;
                    bool hitCheck = EllipseScanner(2, 1.4f, maxC.gameObject.transform.position, c.gameObject.transform.position);
                    if (hitCheck == true)
                    {
                        if (OgreFemale_Sk5.rageOn == true)
                        {
                            IBattleHandler ch = c as IBattleHandler;
                            caster.AttackTarget(c, 100);
                        }
                        else
                        {
                            Debug.Log("Auto Attack => " + c.gameObject.transform.name);
                            IBattleHandler ch = c as IBattleHandler;
                            caster.AttackTarget(c, 50);
                        }
                    }
                }
                SkillEventArgs s = new SkillEventArgs(this.name, true);
                OnEndSkill(s);
            }
            caster.MoveComplete -= OnMoveComplete;
        }
    }
}

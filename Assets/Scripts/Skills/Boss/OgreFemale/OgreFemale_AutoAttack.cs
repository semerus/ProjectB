using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OgreFemale_AutoAttack : Skill {

    IBattleHandler[] friendlyNum;
    float min = 1000000000000;
    IBattleHandler minNum = null;
    Character minC;

    public override void RunTime()
    {
        AutoAttack();
        base.RunTime();
    }

    public override void Activate(IBattleHandler target)
    {
        friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
        TimeSystem.GetTimeSystem().AddTimer(this);
    }

    private void AutoAttacking()
    {
        for (int i = 0; i < friendlyNum.Length; i++)
        {
            Character c = friendlyNum[i] as Character;
            bool hitCheck = EllipseScanner(2, 1.4f, minC.gameObject.transform.position, c.gameObject.transform.position);
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
                    caster.AttackTarget(c, 0);
                }
            }
        }
        SkillEventArgs s = new SkillEventArgs(this.name, true);
        OnEndSkill(s);
    }

    public void AutoAttack()
    {
        AutoAttackTargetting();
        CheckTargetRange();
    }

    private void AutoAttackTargetting()
    {
        for (int i = 0; i < friendlyNum.Length; i++)
        {
            Character c = friendlyNum[i] as Character;
            float x = this.gameObject.transform.position.x - c.transform.position.x;
            float y = this.gameObject.transform.position.y - c.transform.position.y;

            float distance = x * x + y * y;
            
            if (distance <= min)
            {
                min = distance;
                minNum = friendlyNum[i];
            }
        }
        minC = minNum as Character;
        Debug.Log(minC);
    }

    private void CheckTargetRange()
    {
        Vector3 enemyPosition = minC.transform.position;
        
        float myA = 2.1f;
        float myB = 0.7f;

        float outerX = 0.5f;
        float innerX = 0.2f;
        float outerY = 0.3f;

        float dX = enemyPosition.x - this.transform.position.x;
        float dY = enemyPosition.y - this.transform.position.y;

        float inX = (myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + innerX;
        float outX = (myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + outerX;

        float m_inX = -1 * ((myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + innerX);
        float m_outX = -1 * ((myA / myB * Mathf.Sqrt(myB * myB - dY * dY)) + outerX);


        if (((-1 * outerY) <= dY && dY <= outerY) && ((inX <= dX && dX <= outX) || (m_outX <= dX && dX <= m_inX)))
        {
            //aster.Move(this.gameObject.transform.position);
            AutoAttacking();
            TimeSystem.GetTimeSystem().DeleteTimer(this);
            Debug.Log("attack on");
        }
        else
        {
            Debug.Log("no");
            if(this.gameObject.transform.position.x <= minC.transform.position.x)
            {
                if ((caster.Status & CharacterStatus.IsMovingMask) == 0)
                {
                    caster.MoveComplete += new EventHandler<MoveEventArgs>(OnMoveComplete);
                }
                caster.ChangeMoveTarget(minC.transform.position- new Vector3(2.5f,0,0));
            }
            else
            {
                if ((caster.Status & CharacterStatus.IsMovingMask) == 0)
                {
                    caster.MoveComplete += new EventHandler<MoveEventArgs>(OnMoveComplete);
                }
                caster.ChangeMoveTarget(minC.transform.position + new Vector3(2.5f, 0, 0));
            }
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

    public void OnMoveComplete(object sender, EventArgs e)
    {
        MoveEventArgs m = e as MoveEventArgs;
        if(m != null)
        {
            if(!m.result)
            {
                TimeSystem.GetTimeSystem().DeleteTimer(this);
                SkillEventArgs s = new SkillEventArgs(this.name, true);
                OnEndSkill(s);
                TimeSystem.GetTimeSystem().DeleteTimer(this);
                
                caster.MoveComplete -= OnMoveComplete;
            }
        }
    }
}

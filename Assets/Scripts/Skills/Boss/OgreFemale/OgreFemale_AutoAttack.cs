using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_AutoAttack : Skill {

    IBattleHandler[] friendlyNum;
    float x;
    float y;
    float min = 1000000000000;
    float ofTime = 0f;
    bool attackOn = false;
    IBattleHandler minNum = null;
    Character minC;

    public override void RunTime()
    {
        ofTime += Time.deltaTime;
        base.RunTime();
    }

    // Use this for initialization
    void Start()
    {
       friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
       Debug.Log(friendlyNum.Length);
    }

    private void AutoAttacking()
    {
        if (attackOn == true)
        {
            if (!TimeSystem.GetTimeSystem().CheckTimer(this))
            {
                TimeSystem.GetTimeSystem().AddTimer(this);
                ofTime = 0;
            }

            if (ofTime < 1)
            {
                // 준비동작
            }
            if (ofTime <= 1.0 && ofTime >= 0.99)
            {
                for (int i = 0; i < friendlyNum.Length; i++)
                {
                    Character c = friendlyNum[i] as Character;
                    bool hitCheck = EllipseScanner(2, 1.4f, minC.gameObject.transform.position, c.gameObject.transform.position);
                    if (hitCheck == true)
                    {
                        if (OgreFemale_Sk5.rageOn == true)
                        {
                            // 데미지 100
                        }
                        else
                        {
                            Debug.Log("Auto Attack => " + c.gameObject.transform.name);
                            //데미지 50
                        }
                    }
                }
            }
            if (ofTime > 1 && ofTime <= 1.5)
            {
                // 마무리 동작
            }
            if (ofTime > 1.5)
            {
                if (TimeSystem.GetTimeSystem().CheckTimer(this))
                {
                    TimeSystem.GetTimeSystem().DeleteTimer(this);
                }
                attackOn = false;
                ofTime = 0;
            }
        }
    }

    public void AutoAttack()
    {
        if (attackOn == false)
        {
            AutoAttackTargetting();
            CheckTargetRange();
        }
    }

    private void AutoAttackTargetting()
    {
        for (int i = 0; i < friendlyNum.Length; i++)
        {
            Character c = friendlyNum[i] as Character;
            x = this.gameObject.transform.position.x - c.transform.position.x;
            y = this.gameObject.transform.position.y - c.transform.position.y;

            float distance = x * x + y * y;
            
            if (distance <= min)
            {
                min = distance;
                minNum = friendlyNum[i];
            }
        }
        minC = minNum as Character;
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
            caster.Move(this.gameObject.transform.position);
            Debug.Log("attack on");
            attackOn = true;
        }
        else
        {
            Debug.Log("no");
            attackOn = false;
            if(this.gameObject.transform.position.x <= minC.transform.position.x)
            {
                caster.ChangeMoveTarget(minC.transform.position- new Vector3(2.5f,0,0));
            }
            else
            {
                caster.ChangeMoveTarget(minC.gameObject.transform.position + new Vector3(2.5f, 0, 0));
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

    public override void Activate(IBattleHandler target)
    {
        AutoAttack();
        AutoAttacking();
    }
}

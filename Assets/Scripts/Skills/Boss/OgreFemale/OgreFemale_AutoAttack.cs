using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OgreFemale_AutoAttack : Skill {

    IBattleHandler[] friendlyNum;
    Character minC;
    float skillTime = 0f;

	void Awake() {
		caster = gameObject.GetComponent<Character> ();
	}

    public override void RunTime()
    {
        base.RunTime();
        skillTime -= Time.deltaTime;
        if (CheckSkillStatus(SkillStatus.ProcessMask))
        {
            if (skillTime <= 0f)
            {
                AutoAttack();
                skillTime = 1.5f;
            }
            caster.Target = minC;
        }
    }

    protected override void OnCoolDown()
    {
        base.OnCoolDown();
        if(CheckSkillStatus(SkillStatus.ProcessMask))
        {
            TimeSystem.GetTimeSystem().AddTimer(this);
        }
    }

    public override void Activate()
    {
        UpdateSkillStatus(SkillStatus.ProcessOn);
        TimeSystem.GetTimeSystem().AddTimer(this);
        friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
    }

    private void AutoAttacking()
    {
        caster.Target = minC;
        StartCoolDown();
        for (int i = 0; i < friendlyNum.Length; i++)
        {
            Character c = friendlyNum[i] as Character;
            bool hitCheck = EllipseScanner(2, 1.4f, minC.gameObject.transform.position, c.gameObject.transform.position);
            if (hitCheck == true)
            {
                Debug.Log("Auto Attack => " + c.gameObject.transform.name);
                IBattleHandler ch = c as IBattleHandler;
                caster.AttackTarget(c, 50);
            }
        }
		caster.Anim.onCue -= AutoAttacking;
        UpdateSkillStatus(SkillStatus.ProcessOff);
        SkillEventArgs s = new SkillEventArgs(this.name, true);
        OnEndSkill(s);
    }

    public void AutoAttack()
    {
        if (!OutNumberCheck() || minC == null)
        {
            AutoAttackTargetting();
        }
        CheckTargetRange();
    }

    private void AutoAttackTargetting()
    {
        IBattleHandler minNum=null;
        float temp = Mathf.Infinity;

        for (int i = 0; i < friendlyNum.Length; i++)
        {
            Character c = friendlyNum[i] as Character;
            float x = this.gameObject.transform.position.x - c.transform.position.x;
            float y = this.gameObject.transform.position.y - c.transform.position.y;

            float distance = x * x + y * y;
            
            if (distance <= temp)
            {
                temp = distance;
                minNum = friendlyNum[i];
            }
        }
        minC = minNum as Character;
        
    }

    private bool OutNumberCheck()
    {
        int targetingNum = 0;

        for (int i = 0; i <= friendlyNum.Length - 1; i++)
        {
            Character c = friendlyNum[i] as Character;
            bool targetOn = EllipseScanner(3, 1.8f, this.gameObject.transform.position, c.gameObject.transform.position);

            if (targetOn == true)
            {
                targetingNum++;
            }
        }

        if (targetingNum >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckTargetRange()
    {
        if (minC != null)
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
                caster.StopMove();
                if (caster.ChangeAction(CharacterAction.Attacking))
                {
                    caster.Anim.onCue += AutoAttacking;
                }
                Debug.Log("attack on");
            }
            else
            {
                if (this.gameObject.transform.position.x <= minC.transform.position.x)
                {
                    if (caster.Action != CharacterAction.Moving)
                    {
                        caster.MoveComplete += new EventHandler<MoveEventArgs>(OnMoveComplete);
                    }
                    caster.ChangeMoveTarget(minC.transform.position - new Vector3(2.5f, 0, 0));
                }
                else
                {
                    if (caster.Action != CharacterAction.Moving)
                    {
                        caster.MoveComplete += new EventHandler<MoveEventArgs>(OnMoveComplete);
                    }
                    caster.ChangeMoveTarget(minC.transform.position + new Vector3(2.5f, 0, 0));
                }
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

    protected void OnMoveComplete(object sender, EventArgs e)
    {
        MoveEventArgs m = e as MoveEventArgs;
        if(m != null)
        {
            if(!m.result)
            {
                UpdateSkillStatus(SkillStatus.ProcessOff);
                SkillEventArgs s = new SkillEventArgs(this.name, true);
                OnEndSkill(s);             
                caster.MoveComplete -= OnMoveComplete;
            }
        }
    }
}

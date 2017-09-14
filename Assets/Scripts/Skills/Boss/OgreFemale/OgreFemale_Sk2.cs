using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk2 : Skill {

    IBattleHandler[] friendlyNum;
    Vector3 adjustpoint;
    Character maxC;
    Character target;
    IBattleHandler maxNum = null;
	protected float stunTime;

	void Awake() {
		caster = gameObject.GetComponent<Character> ();
	}

	public override void SetSkill (Dictionary<string, object> param)
	{
		base.SetSkill (param);
		this.stunTime = (float)((double)param ["stun_time"]);
	}

    public override void Activate()
    {
        caster.Anim.ClearAnimEvent();
        ResetSetting();
        UpdateSkillStatus(SkillStatus.ProcessOn);
        adjustpoint = this.gameObject.transform.position + Vector3.down;
        friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
        StartCoolDown();
        JumpAttackTargetting();
        Jump();
    }

    #region JumpAttackTargetting

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

        target = maxC;

        if (this.gameObject.transform.position.x >= maxC.transform.position.x)
        {
            adjustpoint = maxC.transform.position;
            adjustpoint.x = maxC.transform.position.x + 1.25f;
        }
        else
        {
            adjustpoint = maxC.transform.position;
            adjustpoint.x = maxC.transform.position.x - 1.25f;
        }
    }

    #endregion

    #region Jump

    void Jump()
    {
        caster.Anim.onCue +=JumpEnd;
        float x = Mathf.Abs(adjustpoint.x - this.gameObject.transform.position.x);
        float y = Mathf.Abs(adjustpoint.y - this.gameObject.transform.position.y);
        Vector3 s = new Vector3(x, y);

        // 보스 뛰어다니기 스킬 후 스턴이 걸리면서 이 스킬이 실행되는데 if문을 통과 못하므로 점프가 실행되지 않음

		if (caster.BeginJumpTarget (adjustpoint, x, y)) {
            caster.ChangeAction(CharacterAction.Jumping);
			//caster.MoveComplete += new EventHandler<MoveEventArgs> (OnMoveComplete);
		} else {
            caster.ChangeAction(CharacterAction.Idle);
            UpdateSkillStatus(SkillStatus.ProcessOff);
            SkillEventArgs e = new SkillEventArgs(this.name, true);
            OnEndSkill(e);
            // enter what happens when rooted at start
            // 왜 스턴 당하고 일로 올까?
            //SkillEventArgs e = new SkillEventArgs(this.name, false);
            //OnEndSkill(e);
        }
    }

    #endregion

    #region JumpEnd

    public void JumpEnd()
    {
        caster.Anim.onCue -= JumpEnd;
		new Buff_Stun (stunTime, caster, target);
        caster.ChangeAction(CharacterAction.Idle);
        caster.ChangeAction(CharacterAction.Attacking);
        caster.Target = target;
        caster.Anim.onCue += AutoAttacking;
    }

    #endregion

    #region AutoAttacking

    private void AutoAttacking()
    {
        for (int i = 0; i < friendlyNum.Length; i++)
        {
            Character c = friendlyNum[i] as Character;
            bool hitCheck = EllipseScanner(2, 1.4f, target.gameObject.transform.position, c.gameObject.transform.position);
            if (hitCheck == true)
            {
                Debug.Log("Auto Attack => " + c.gameObject.transform.name);
                caster.AttackTarget(c, 50);
            }
        }
        caster.Anim.onCue -= AutoAttacking;
        caster.ChangeAction(CharacterAction.Idle);
        UpdateSkillStatus(SkillStatus.ProcessOff);
        SkillEventArgs s = new SkillEventArgs(this.name, true);
        OnEndSkill(s);
    }

    #endregion

    #region ResetSetting

    public void ResetSetting()
    {
        maxC = null;
        target = null;
    }

    #endregion

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

    //void OnMoveComplete(object sender, EventArgs e)
    //{
    //    MoveEventArgs m = e as MoveEventArgs;
    //    if (m != null)
    //    {
    //        if (m.result)
    //        {
    //            for (int i = 0; i < friendlyNum.Length; i++)
    //            {
    //                Character c = friendlyNum[i] as Character;
    //                bool hitCheck = EllipseScanner(2, 1.4f, maxC.gameObject.transform.position, c.gameObject.transform.position);
    //                if (hitCheck == true)
    //                {
    //                    if (OgreFemale_Sk5.rageOn == true)
    //                    {
    //                        IBattleHandler ch = c as IBattleHandler;
    //                        Debug.Log("ssssssss");
    //                        caster.AttackTarget(c, 100);
    //                    }
    //                    else
    //                    {
    //                        /*
    //                                    Debug.Log("Auto Attack => " + c.gameObject.transform.name);
    //                                    IBattleHandler ch = c as IBattleHandler;
    //                                    caster.AttackTarget(c, 50);
    //                        */

    //                        // give 1 sec stun buff
    //                    }
    //                }
    //            }
    //            SkillEventArgs s = new SkillEventArgs(this.name, true);
    //            OnEndSkill(s);
    //        }
    //        caster.MoveComplete -= OnMoveComplete;
    //    }
    //}

    #endregion

}

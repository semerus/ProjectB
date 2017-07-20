using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale : Enemy,ITimeHandler {

    IBattleHandler[] friendlyNum;
	[SerializeField]
	int pattern = 0;
	float pattern_timer = 5f;

    protected override void Start()
    {
		base.Start ();

		maxHp = 500;
		hp = 500;
		speed_x = 1f;
        speed_y = 1f;

		skills = new Skill[4];
		skills [0] = gameObject.AddComponent<OgreFemale_AutoAttack> ();
		skills [1] = gameObject.AddComponent<OgreFemale_Sk2> ();
		skills [2] = gameObject.AddComponent<OgreFemale_Sk3> ();
		skills [3] = gameObject.AddComponent<OgreFemale_Sk4> ();

		for (int i = 0; i < skills.Length; i++) {
			skills [i].SetSkill (this);
			skills [i].EndSkill += new EventHandler<SkillEventArgs> (OnSkillEnd);
		}
		friendlyNum = BattleManager.GetBattleManager ().GetEntities (Team.Friendly);
    }

	void OnSkillEnd(object sender, EventArgs e)
	{
		switch (pattern) {
		case 1:
		case 3:
		case 4:
			pattern = 0;
			break;
		case 2:
			pattern = 1;
			break;
		default:
			break;
		}
	}

	public override void RunTime()
    {
		base.RunTime ();
        InstructEnemyAI();
    }

	public void SetPattern(int change) {
		pattern = change;
	}

    protected override void InstructEnemyAI()
    {
		if (CheckCharacterStatus (CharacterStatus.NotOrderableMask))
			return;
		switch (pattern) {
		// idle
		case 0:
			if (skills[1].CheckSkillStatus (SkillStatus.ReadyMask)) {
				pattern = 2;
				skills[1].OnCast ();
			}  else if (skills[2].CheckSkillStatus (SkillStatus.ReadyMask) && Sk3 ()) {
				pattern = 3;
				skills[2].OnCast ();
			} else if (skills[0].CheckSkillStatus (SkillStatus.ReadyMask)) {
				pattern = 1;
				skills[0].OnCast ();
			}
			break;
		// autoattack
		case 1:
			if (skills[1].CheckSkillStatus (SkillStatus.ReadyMask)) {
				StopMove ();
				pattern = 2;
				skills[1].OnCast ();
			} else if (skills[2].CheckSkillStatus (SkillStatus.ReadyMask) && Sk3 ()) {
				StopMove ();
				pattern = 3;
				skills[2].OnCast (); 
			}
			else if (skills[0].CheckSkillStatus (SkillStatus.ReadyMask)) {
				skills[0].OnCast ();
			}
			break;
		default:
			break;
		}
    }

    private bool Sk3()
    {
        int targetingNum = 0;

        for (int i = 0; i <= friendlyNum.Length - 1; i++)
        {
            Character c = friendlyNum[i] as Character;
            bool targetOn = EllipseScanner(4, 1.5f, this.gameObject.transform.position, c.gameObject.transform.position);

            if (targetOn == true)
            {
                targetingNum++;
            }
        }

        if (targetingNum >= 2)
        {
            Debug.Log("e on");
            return true;
        }
        else
        {
            return false;
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

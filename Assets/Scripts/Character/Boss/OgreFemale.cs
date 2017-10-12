using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale : Enemy {

    IBattleHandler[] friendlyNum;
	[SerializeField]
	protected int pattern = 0;
	float startPattern = 5f;
	float patternTimer = 0f;
	protected OgreMale partner;

	public OgreMale Partner {
		get {
			return partner;
		}
	}

    protected void Start()
    {
		partner = GameObject.Find ("Ogre Wizard").GetComponent<OgreMale> ();
		partner.partner = this;

		for (int i = 0; i < skills.Length; i++) {
			skills [i].EndSkill += new EventHandler<SkillEventArgs> (OnSkillEnd);
		}
		friendlyNum = BattleManager.GetBattleManager ().GetEntities (Team.Friendly);
    }

	void OnSkillEnd(object sender, EventArgs e)
	{
		switch (pattern) {
		case 1:
		case 2:
		case 3:
		case 4:
			ChangeAction (CharacterAction.Idle);
			pattern = 0;
			break;
			/*
		case 2:
			pattern = 1;
			break;
			*/
		default:
			break;
		}
	}

	public override void RunTime()
    {
		base.RunTime ();
		if (patternTimer < startPattern) {
			patternTimer += Time.deltaTime;
		} else {
			InstructEnemyAI();
		}
    }

	public void SetPattern(int change) {
		pattern = change;
	}

    protected override void InstructEnemyAI()
    {
        base.InstructEnemyAI();
        if (!skills[3].CheckSkillStatus(SkillStatus.ProcessMask))
        {
            switch (pattern) {
			// idle
            case 0:
                if (skills[1].CheckSkillStatus(SkillStatus.ReadyMask))
                {
                    StopMove();
                    pattern = 2;
                    skills[1].OnCast();
                }
                else if (skills[2].CheckSkillStatus(SkillStatus.ReadyMask) && Sk3())
                {
                    pattern = 3;
                    skills[2].OnCast();
                }
                else if (skills[0].CheckSkillStatus(SkillStatus.ReadyMask))
                {
                    pattern = 1;
                    skills[0].OnCast();
                }
                break;
            // autoattack
			case 1:
				if (!skills [0].CheckSkillStatus (SkillStatus.ProcessMask)) {
					if (skills[1].CheckSkillStatus(SkillStatus.ReadyMask))
					{
						StopMove();
						pattern = 2;
						skills[1].OnCast();
					}
					else if (skills[2].CheckSkillStatus(SkillStatus.ReadyMask) && Sk3())
					{
						StopMove();
						pattern = 3;
						skills[2].OnCast();
					}
					else if (skills[0].CheckSkillStatus(SkillStatus.ReadyMask))
					{
						skills[0].OnCast();
					}
				}
                break;
			default:
				break;
            }
        }
    }

    private bool Sk3()
    {
        int targetingNum = 0;

        for (int i = 0; i <= friendlyNum.Length - 1; i++)
        {
            Character c = friendlyNum[i] as Character;
			bool targetOn = Scanner.EllipseScanner(4, 1.5f, this.gameObject.transform.position, c.gameObject.transform.position);

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
}

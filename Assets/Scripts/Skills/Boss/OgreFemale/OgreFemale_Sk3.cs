using System;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale_Sk3 : Skill {

    IBattleHandler[] friendlyNum;
	List<Character> stunnedHeroes = new List<Character> ();
	protected float stunTime;
	protected float yellTime;
	//currently uses anim time
	//protected float yellTimer = 0f;

	void Awake() {
		caster = gameObject.GetComponent<Character> ();
	}

	public override void SetSkill (Dictionary<string, object> param)
	{
		base.SetSkill (param);
		yellTime = (float)((double)param ["cast_time"]);
		stunTime = (float)((double)param ["stun_time"]);
	}

    public override void Activate()
	{
        StartCoolDown();
        friendlyNum = BattleManager.GetBattleManager().GetEntities(Team.Friendly);
		// active 1 since skill 2 uses jumping
		caster.ChangeAction (CharacterAction.Active1);
		caster.Anim.ClearAnimEvent ();
		caster.Anim.onCue += BattleCry;
    }

	protected override void OnProcess ()
	{
		base.OnProcess ();
		Sk3On ();
	}

	public override void OnEndSkill ()
	{
		base.OnEndSkill ();
		caster.ChangeAction (CharacterAction.Idle);
		stunnedHeroes.Clear ();
	}

	protected void BattleCry() {
		UpdateSkillStatus (SkillStatus.ProcessOn);
		caster.Anim.ClearAnimEvent ();
		caster.Anim.onCue += OnEndSkill;
	}

	/// <summary>
	/// stuns heroes if not have been stunned yet
	/// </summary>
    private void Sk3On()
    {
        for (int i = 0; i <= friendlyNum.Length - 1; i++)
        {
            Character c = friendlyNum[i] as Character;
			bool hitCheck = Scanner.EllipseScanner(3.5f, 1.3f, this.gameObject.transform.position, c.gameObject.transform.position);
			if (hitCheck == true && !stunnedHeroes.Contains(c))
            {
                //Debug.Log("stun " + c.transform.name);
				new Buff_Stun (stunTime, caster, c);
				stunnedHeroes.Add (c);
            }
        }   
    }
}

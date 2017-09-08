using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_Restore : HeroActive {

    #region MonoBehaviours
    protected virtual void Awake()
    {
		caster = gameObject.GetComponent<Character> ();
		Hero h = caster as Hero;
		if (h != null) {
			h.activeSkills [2] = this;
		}

        //UI
        button = Resources.Load<Sprite>("Skills/Heroes/Healer/Healer_Skill3");
    }
    #endregion

	public override void Activate()
	{
		// run animation
		OnAnimationEnd();
	}

	private void OnAnimationEnd() {
		IBattleHandler[] friends = BattleManager.GetBattleManager ().GetEntities (Team.Friendly);
		for (int i = 0; i < friends.Length; i++) {
			bool hasDebuff = false;
			Character character = friends [i] as Character;
			if (character != null) {
				for (int j = 0; j < character.Buffs.Count; j++) {
					if (!character.Buffs [j].Isbuff) {
						character.Buffs [j].EndBuff ();
						hasDebuff |= true;
					}
				}
				// invoke extra effect for characters who previously had debuffs
				if (hasDebuff) {
					InvokeEffect (character);
				}
			}
		}

		// release onCue
		StartCoolDown();
	}

	protected virtual void InvokeEffect(Character target) {	}
}

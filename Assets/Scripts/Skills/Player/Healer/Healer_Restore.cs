using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer_Restore : HeroActive {
	void Awake() {
		caster = gameObject.GetComponent<Character> ();
		Hero h = caster as Hero;
		if (h != null) {
			h.activeSkills [2] = this;
		}

		button = Resources.Load<Sprite>("Skills/Heroes/Healer/Healer_Skill3");
	}
}

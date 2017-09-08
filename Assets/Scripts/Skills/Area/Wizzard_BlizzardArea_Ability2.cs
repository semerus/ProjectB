using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizzard_BlizzardArea_Ability2 : Wizzard_BlizzardArea {

	protected override void Awake ()
	{
		base.Awake ();
		range = new Vector2(4f, 1.8f);
	}
}

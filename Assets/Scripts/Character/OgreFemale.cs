using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OgreFemale : Enemy {
	public override void ReceiveHeal (int heal)
	{
		base.ReceiveHeal (heal);
		Debug.Log ("Received heal : " + heal);
	}
}

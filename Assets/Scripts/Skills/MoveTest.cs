﻿using System;
using UnityEngine;

public class MoveTest : Skill {
	
	#region implemented abstract members of Skill

	public override void Activate ()
	{
		caster.MoveComplete += new EventHandler<MoveEventArgs>(OnMoveComplete);
		caster.BeginMove (new Vector3 ());
	}

	#endregion

	void OnMoveComplete(object sender, EventArgs e) {
		MoveEventArgs m = e as MoveEventArgs;

		if (m.result)
			Debug.Log ("Move Successful");

		caster.MoveComplete -= OnMoveComplete;
	}

	void Example() {
		//Type temp = Type.GetType ("MoveTest");
	}
}

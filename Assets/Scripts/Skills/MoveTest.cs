using System;
using UnityEngine;

public class MoveTest : Skill {
	
	#region implemented abstract members of Skill

	public override void Activate (IBattleHandler target)
	{
		caster.MoveComplete += new EventHandler<MoveEventArgs>(OnMoveComplete);
		caster.Move (new Vector3 ());
	}

	#endregion

	void OnMoveComplete(object sender, EventArgs e) {
		MoveEventArgs m = e as MoveEventArgs;

		if (m.result)
			Debug.Log ("Move Successful");

		caster.MoveComplete -= OnMoveComplete;

        
	}
}

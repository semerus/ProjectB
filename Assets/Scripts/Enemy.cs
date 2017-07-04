using UnityEngine;

public abstract class Enemy : Character, IDoubleTapHandler {
	#region implemented abstract members of Character
	public override void Spawn ()
	{
		throw new System.NotImplementedException ();
	}
	#endregion

	#region IDoubleTapHandler implementation

	public void OnDoubleTap ()
	{
		throw new System.NotImplementedException ();
	}

	#endregion

	// capsulize patterns
	protected virtual void InstructEnemyAI () {
		
	}
}

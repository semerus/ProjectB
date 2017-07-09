using UnityEngine;

public abstract class Enemy : Character, IDoubleTapHandler {

	#region IDoubleTapHandler implementation

	public void OnDoubleTap (Vector3 pixelPos)
	{
		BattleManager.GetBattleManager ().AttackByAllFriendly (this as IBattleHandler);
	}

	#endregion

	// capsulize patterns
	protected virtual void InstructEnemyAI () {
		
	}
}

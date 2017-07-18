using UnityEngine;

public abstract class Enemy : Character, IDoubleTapHandler {

	protected EnemyUI enemyUI; // load it from spawn, try creating characte spawn later

    #region implemented abstract members of Character
    protected override void UpdateHpUI()
    {
		float percent = (float)hp / (float)maxHp;
		//EnemyUI.UpdateHp (percent);
    }
    #endregion

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

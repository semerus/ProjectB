using UnityEngine;

public abstract class Enemy : Character, IDoubleTapHandler {

	public EnemyUI enemyUI; // load it from spawn, try creating characte spawn later

    #region implemented abstract members of Character
    protected override void UpdateHpUI()
    {
		float percent = (float)hp / (float)maxHp;
		enemyUI.UpdateHp (percent);
    }

	protected override void UpdateCCUI() {
		enemyUI.UpdateCC (status);
	}
    #endregion

    #region IDoubleTapHandler implementation

    public void OnDoubleTap (Vector3 pixelPos)
	{
		BattleManager.GetBattleManager ().AttackByAllFriendly (this as IBattleHandler);
	}

	#endregion

	public override void Spawn ()
	{
		base.Spawn ();
	}
	// capsulize patterns
	protected virtual void InstructEnemyAI () {
		if (CheckCharacterStatus (CharacterStatus.NotOrderableMask)) {
			return;
		}
	}
}

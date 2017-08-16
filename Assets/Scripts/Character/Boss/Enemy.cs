/*
 * Written by Insung Kim
 * Updated: 2017.08.13
 */
using System.Collections.Generic;
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

	public override void Spawn (Dictionary<string, object> data, int[] skills)
	{
		team = Team.Hostile;
		base.Spawn (data, skills);
	}
	// capsulize patterns
	protected virtual void InstructEnemyAI () {
		if (CheckCharacterStatus (CharacterStatus.NotOrderableMask)) {
			return;
		}
	}
}

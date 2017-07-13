using UnityEngine;

public class OgreMeteor : Skill {

	Character target;

	#region implemented abstract members of Skill

	public override void Activate (IBattleHandler target)
	{
		state = SkillState.InProcess;
	}

	#endregion

	void SetTarget () {
		IBattleHandler[] friendly = BattleManager.GetBattleManager ().GetEntities (Team.Friendly);
		int random = Random.Range (0, friendly.Length);

		target = friendly [random] as Character;
	}
}

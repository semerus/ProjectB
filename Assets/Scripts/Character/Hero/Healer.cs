using UnityEngine;

public class Healer : Hero {
	#region implemented abstract members of Hero

	public override void AutoAttack (IBattleHandler target)
	{
        autoAttack.Activate();
	}

	#endregion

	void Awake() {
		id = 3;
		status = CharacterStatus.Idle;
		maxHp = 200;
		hp = maxHp;
        speed_x = speed_x_1Value * 1.8f;
		speed_y = speed_y_1Value * 1.8f;

		autoAttack = gameObject.AddComponent<Healer_Attack>();
		autoAttack.SetSkill(this);

        passiveSkill = gameObject.AddComponent<Healer_Passive>();
        passiveSkill.SetSkill(this);

		activeSkills = new HeroActive[3];
		activeSkills[0] = gameObject.AddComponent<Healer_Summon_Totem_MovementSpeedBuff>();
		activeSkills[1] = gameObject.AddComponent<Healer_Summon_Totem_MovementSpeedBuff>();
		activeSkills[2] = gameObject.AddComponent<Healer_Summon_Totem_MovementSpeedBuff>();
		foreach (Skill eachSkill in activeSkills)
		{
			eachSkill.SetSkill(this);
		}
	}
}

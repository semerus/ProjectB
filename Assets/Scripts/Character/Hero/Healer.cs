using UnityEngine;

public class Healer : Hero {

	void Awake() {
		id = 3;
		team = Team.Friendly;
		status = CharacterStatus.Idle;
		maxHp = 200;
		hp = maxHp;
		speed_x = 2.57f;
		speed_y = 1.4f;

		autoAttack = gameObject.AddComponent<Wizard_AutoAttack>();
		autoAttack.SetSkill(this);

		activeSkills = new HeroActive[3];
		activeSkills[0] = gameObject.AddComponent<Wizard_Snowball>();
		activeSkills[1] = gameObject.AddComponent<Wizard_Freeze>();
		activeSkills[2] = gameObject.AddComponent<Wizard_Blizzard>();
		foreach (Skill eachSkill in activeSkills)
		{
			eachSkill.SetSkill(this);
		}
	}
}

using UnityEngine;

public class Healer : Hero {
	#region implemented abstract members of Hero

	public override void AutoAttack (IBattleHandler target)
	{
        //autoAttack.Activate();
	}

	#endregion

	void Awake() {

//		autoAttack = gameObject.AddComponent<Healer_Attack>();
//        passiveSkill = gameObject.AddComponent<Healer_Passive>();
//
//		activeSkills = new HeroActive[3];
//		activeSkills[0] = gameObject.AddComponent<Healer_Summon_Totem_MovementSpeedBuff>();
//		activeSkills[1] = gameObject.AddComponent<Healer_Summon_ProtectionArea_CooltimeReduction>();
//		activeSkills[2] = gameObject.AddComponent<Healer_Summon_Totem_MovementSpeedBuff>();
//		foreach (Skill eachSkill in activeSkills)
//		{
//			eachSkill.SetSkill(this);
//		}
	}
}

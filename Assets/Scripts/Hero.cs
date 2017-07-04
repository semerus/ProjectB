using UnityEngine;

public abstract class Hero : MonoBehaviour {



	public void SetSkill(Skill[] skills);
	public void AutoAttack (IBattleHandler target);
}

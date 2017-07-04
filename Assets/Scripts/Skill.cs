using UnityEngine;

public abstract class Skill : MonoBehaviour {
	// gameUI prefab

	// skill state
	SkillState state;

	public void OnClick() {
		// when ui button is clicked
		// start channeling
	} 
	public void Activate (IBattleHandler target) {
		// activate skill (launch projectile, area etc)
	}
}

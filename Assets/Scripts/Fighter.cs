using UnityEngine;

public class Fighter : Hero {
	// temporary value given
	void Start() {
		id = 1;
		team = Team.Friendly;
		state = CharacterState.Idle;
		maxHp = 10;
		hp = 10;
		speed = 2.5f;
	}
}

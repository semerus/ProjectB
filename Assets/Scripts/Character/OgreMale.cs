using UnityEngine;

public class OgreMale : Enemy {
	void Start() {
		id = 3;
		team = Team.Hostile;
		state = CharacterState.Idle;
		maxHp = 15000;
		hp = 15000;
		speed_x = 1f;
		speed_y = 1f;
	}
}

﻿using UnityEngine;

public class Fighter : Hero {
	
	void Start() {
		// temporary value given
		id = 1;
		team = Team.Friendly;
		state = CharacterState.Idle;
		maxHp = 10;
		hp = 10;
		speed_x = 2.57f;
		speed_y = 1.4f;

		// temporary spawning -> should be moved to BattleManager
		Spawn();

        // 
	}
}

using System;

public enum SkillState {
	Ready,
	Channeling,
	InProcess,
	OnCoolDown
}

public enum Team {
	Hostile,
	Neutral,
	Friendly
}

public enum CharacterState {
	Idle,
	Moving,
	Channeling,
	Dead
}

public enum AreaState {
	Ready,
	Active,
	Disabled
}
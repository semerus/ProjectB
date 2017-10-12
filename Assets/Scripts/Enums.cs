using System;

public enum Team {
	Hostile,
	Neutral,
	Friendly
}

/// <summary>
/// Used for animation and Character movement
/// </summary>
public enum CharacterAction {
	Idle = 0,
	Moving = 1,
	Attacking = 2,
	Jumping = 3,
	Dead = 4,
	Channeling = 5,
	Active1 = 6,
	Active2 = 7,
	Active3 = 8,
    Attacked =9
}

public enum AreaState {
	Ready,
	Active,
	Disabled
}

public enum SkillTrait
{
    None,
    Left,
    Right
}


public enum LinkerState
{
    OnLink,
    willBreak,
    OffLink
}

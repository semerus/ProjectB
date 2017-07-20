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
	Dead = 3,
	Channeling = 4,


	// special case
	Jumping = 5
}

public enum AreaState {
	Ready,
	Active,
	Disabled
}

/// <summary>
/// None, Before, On, After
/// </summary>
public enum AttackMotionState
{
    None,
    beforeAttack,
    OnAttack,
    afterAttack
}

/// <summary>
/// None, 
/// </summary>
public enum SkillTrait
{
    None,
    Left,
    Right
}

public enum AttackType
{
    None,
    Melee,
    Range
}

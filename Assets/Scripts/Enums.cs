using System;

public enum SkillState {
	Ready,
	Channeling,
	OnCoolDown
}

public enum Team {
	Hostile,
	Neutral,
	Friendly
}

/// <summary>
/// Idle, Moving, Channeling, Dead
/// </summary>
public enum CharacterState {
    None,
    Idle,
	Moving,
    AutoAttaking,
	Channeling,
	Dead
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


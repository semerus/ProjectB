﻿using System;

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

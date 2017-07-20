using UnityEngine;

public sealed class CharacterStatus {

	public const int Idle = 0;
	public const int Silenced = 4;
	public const int Rooted = 2;
	public const int Stunned = 7;
	public const int Slowed = 8;
	public const int Blind = 16;
	public const int Immune = 32;

	public const int NotOrderableMask = 1;
	public const int IsRootedMask = 1 << 1;
	public const int IsSilencedMask = 1 << 2;
	public const int IsSlowedMask = 1 << 3;
	public const int IsBlindMask = 1 << 4;
	public const int IsImmuneMask = 1 << 5;
}

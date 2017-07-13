using UnityEngine;

public sealed class Status {
	public const int Idle = 0;
	public const int Moving = 2;
	public const int Channeling = 13;
	public const int Dead = 25;
	public const int Silenced = 16;
	public const int Rooted = 8;
	public const int Stunned = 25;
	public const int Slowed = 32;
	public const int Blind = 64;

	public const int NotOrderableMask = 1;
	public const int IsMovingMask = 1 << 1;
	public const int IsChannelingMask = 1 << 2;
	public const int IsRootedMask = 1 << 3;
	public const int IsStunnedMask = 1 << 4;
	public const int IsSlowedMask = 1 << 5;
	public const int IsBlindMask = 1 << 6;
}

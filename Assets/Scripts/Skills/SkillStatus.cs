using System.Collections;
using System.Collections.Generic;

public sealed class SkillStatus {

	public const int All = 14;

	public const int ReadyOn = 1;
	public const int ProcessOn = 1 << 1;
	public const int OnCoolDownOn = 1 << 2;
	public const int ChannelingOn = 1 << 3;

	public const int ProcessOff = All - 1 << 1;
	public const int OnCoolDownOff = All << 2;
	public const int ChannelingOff = All << 3;

	public const int ReadyMask = 1;
	public const int ProcessMask = 1 << 1;
	public const int OnCoolDownMask = 1 << 2;
	public const int ChannelingMask = 1 << 3;
}

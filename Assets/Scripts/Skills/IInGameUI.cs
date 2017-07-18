using UnityEngine;

public interface IInGameUI {
	Sprite InGameUI { get; }
	float CurCoolDown { get; }
	float MaxCoolDown { get; }
}

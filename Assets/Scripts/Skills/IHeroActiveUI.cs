using UnityEngine;

public interface IHeroActiveUI {
	Sprite InGameUI { get; }
	float CurCoolDown { get; }
	float MaxCoolDown { get; }
	int Status { get; }

	void OnCast();
}

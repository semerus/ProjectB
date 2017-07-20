using UnityEngine;

public interface IChanneling {

	float ChannelTime { get;}
	float Timer_Channeling { get; set;}

	// please add TimeSystem.AddTimer during OnChanneling
	void OnChanneling();
	void OnInterrupt(IBattleHandler interrupter);
}

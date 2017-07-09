using System.Collections.Generic;
using UnityEngine;

public sealed class TimeSystem : MonoBehaviour {
	// singleton
	private static TimeSystem instance;

	public static TimeSystem GetTimeSystem() {
		if (!instance) {
			instance = GameObject.FindObjectOfType (typeof(TimeSystem)) as TimeSystem;
			if (!instance)
				Debug.LogError ("No active TimeSystem in the scene");
		}
		return instance;
	}

	// list of timers
	private List<ITimeHandler> timers = new List<ITimeHandler>();

	void Update() {
		RunTime ();
	}

	private void RunTime() {
		for (int i = 0; i < timers.Count; i++) {
			timers [i].RunTime ();
		}
	}

	public void AddTimer (ITimeHandler handler) {
		timers.Add (handler);
	}

	public void DeleteTimer(ITimeHandler handler) {
		timers.Remove (handler);
	}
}

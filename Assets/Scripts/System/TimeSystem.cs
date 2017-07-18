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
    [SerializeField] // for debug easy
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
		if (CheckTimer (handler))
			return;
		else
			timers.Add (handler);
	}

	public bool CheckTimer(ITimeHandler handler) {
		if (timers.Contains (handler))
			return true;
		else
			return false;
	}

	public void DeleteTimer(ITimeHandler handler) {
		if (timers.Contains (handler)) {
			timers.Remove (handler);
		}
	}
}

using UnityEngine;

public class Meteor : Area, ITimeHandler {

	AreaState state;

	float readyTime = 1.5f;
	//float activeTime = 0.1f;
	float timer_ready;

	#region ITimeHandler implementation

	public void RunTime ()
	{
		switch (state) {
		case AreaState.Ready:
			timer_ready += Time.deltaTime;
			if (timer_ready >= readyTime) {
				DestroyMeteor ();
			}
			break;
		default:
			break;
		}
	}

	#endregion

	public void SetMeteor() {
		state = AreaState.Ready;
		timer_ready = 0f;
		TimeSystem.GetTimeSystem ().AddTimer (this);
	}

	public void DestroyMeteor() {
		ContactFilter2D filter = new ContactFilter2D ();
	}
}

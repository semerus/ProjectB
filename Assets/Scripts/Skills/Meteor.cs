using UnityEngine;

public class Meteor : Area, ITimeHandler {

	AreaState state;

	float readyTime = 1.5f;
	float activeTime = 0.1f;
	float timer_ready;

	#region implemented abstract members of Area
	protected override void OnTriggerStay2D (Collider2D other)
	{
		// damage
		throw new System.NotImplementedException ();
	}
	#endregion

	#region ITimeHandler implementation

	public void RunTime ()
	{
		switch (state) {
		case AreaState.Ready:
			timer_ready += Time.deltaTime;
			break;
		case AreaState.Active:
			
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
		
	}
}

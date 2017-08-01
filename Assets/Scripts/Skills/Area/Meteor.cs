using UnityEngine;

public class Meteor : Area, ITimeHandler {

	AreaState state;

	int damage = 100;
	float readyTime = 2f;
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

	public void SetMeteor(Character caster, Vector3 target) {
		this.caster = caster;
		gameObject.SetActive (true);
		transform.position = target;
		state = AreaState.Ready;
		timer_ready = 0f;
		TimeSystem.GetTimeSystem ().AddTimer (this);
	}

	public void DestroyMeteor() {
		LayerMask mask = new LayerMask ();
		ContactFilter2D filter = new ContactFilter2D ();
		Collider2D[] others = new Collider2D[20];

		mask.value = 1 << 11;
		filter.SetLayerMask (mask);
		filter.useTriggers = true;

		int overlapNum = col.OverlapCollider (filter, others);

		for (int i = 0; i < overlapNum; i++) {
			IBattleHandler b = others [i].transform.root.GetComponent<IBattleHandler> ();
			if (b != null) {
				if (b.Team != caster.Team) {
					caster.AttackTarget (b, damage);
				}
			}
		}

		TimeSystem.GetTimeSystem ().DeleteTimer (this);
		gameObject.SetActive (false);
	}
}

using UnityEngine;

public class AnimationController : MonoBehaviour {
	protected SpriteRenderer[] rends;
	protected bool isAnimFacingLeft = true;

	void Awake() {
		rends = GetComponentsInChildren<SpriteRenderer> ();
	}

	public void UpdateFacing(bool isFacingLeft) {
		if (isFacingLeft == isAnimFacingLeft) {
			return;
		}
		isAnimFacingLeft = isFacingLeft;

		if (isAnimFacingLeft) {
			transform.rotation = Quaternion.identity;
		} else {
			transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
		}
	}

	public void UpdateSortingLayer() {
		foreach (var n in rends) {
			n.sortingOrder = -(int)(transform.root.position.y * 100f);
		}
	}

	// call on change state
	public void UpdateAnimation() {
		// uses character state
		// idle
		// attack
		// channeling
		// run
		// dead

		// special occasions
		// jump
		// used by skills
	}
}

using UnityEngine;

public class AnimationController : MonoBehaviour {
	protected Character character;
	protected Animator anim;
	protected SpriteRenderer[] rends;
	protected int[] offsets;
	protected bool isAnimFacingLeft = true;

	void Awake() {
		character = transform.root.GetComponent<Character> ();
		anim = GetComponentInChildren<Animator> ();
		rends = GetComponentsInChildren<SpriteRenderer> ();
		offsets = new int[rends.Length];
		for (int i = 0; i < rends.Length; i++) {
			offsets [i] = rends [i].sortingOrder;
		}
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
		for (int i = 0; i < rends.Length; i++) {
			rends[i].sortingOrder = -(int)(transform.root.position.y * 100f) * 100 + offsets[i];
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
		if (anim == null)
			return;

		switch (character.Action) {
		case CharacterAction.Idle:
			anim.SetInteger ("Action", 0);
			break;
		case CharacterAction.Moving:
			anim.SetInteger ("Action", 1);
			break;
		case CharacterAction.Attacking:
			anim.SetInteger ("Action", 2);
			break;
		default:
			Debug.LogError ("Animation not implemented yet");
			break;
		}

		// special occasions
		// jump
		// used by skills
	}
}

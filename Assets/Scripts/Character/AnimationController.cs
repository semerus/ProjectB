using UnityEngine;

public class AnimationController : MonoBehaviour {
	protected Character character;
	protected Animator anim;
	protected SpriteRenderer[] rends;
	protected int[] offsets;
	protected bool isAnimFacingLeft = true;
	
	public delegate void OnCue();
	public OnCue onCue;

	void Awake() {
		character = transform.root.GetComponent<Character> ();
		anim = GetComponent<Animator> ();
		rends = GetComponentsInChildren<SpriteRenderer> ();
		offsets = new int[rends.Length];
		for (int i = 0; i < rends.Length; i++) {
			rends [i].sortingLayerName = "Character";
			offsets [i] = rends [i].sortingOrder;
		}
	}

	public void UpdateFacing(bool isFacingLeft) {
		if (isFacingLeft == isAnimFacingLeft) {
			return;
		}
		isAnimFacingLeft = isFacingLeft;

		if (isAnimFacingLeft) {
			transform.parent.rotation = Quaternion.identity;
		} else {
			transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
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

	public void OnAnimEvent() {
		//onCue();
		Debug.Log("Will hit at this moment");
	}

	public void PauseAnimation() {
		if (anim == null)
			return;
		anim.enabled = false;
	}

	public void UnPauseAnimation() {
		if (anim == null)
			return;
		anim.enabled = true;
	}
}

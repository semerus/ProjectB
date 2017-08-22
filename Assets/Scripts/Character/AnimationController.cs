/*
 * Written by Insung Kim
 * Updated: 2017.08.17
 */
using UnityEngine;

public class AnimationController : MonoBehaviour {
	
	protected Animator anim;
	protected SpriteRenderer[] rends;
	protected int[] offsets;
	protected bool isAnimFacingLeft = true;
	
	public delegate void OnCue();
	public OnCue onCue;

	protected virtual void Awake() {
		
		anim = GetComponent<Animator> ();
		rends = GetComponentsInChildren<SpriteRenderer> ();
		offsets = new int[rends.Length];
		for (int i = 0; i < rends.Length; i++) {
			rends [i].sortingLayerName = "Character";
			offsets [i] = rends [i].sortingOrder;
		}
	}

	// call on change state
	public virtual void UpdateAnimation() {}

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
		
	public void OnAnimEvent() {
		if (onCue != null) {
			onCue();
		}
		//Debug.Log("Will hit at this moment");
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

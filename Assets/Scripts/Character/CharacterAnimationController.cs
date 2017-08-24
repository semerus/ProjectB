/*
 * Written by Insung Kim
 * Updated: 2017.08.17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : AnimationController {
	protected Character character;

	protected override void Awake ()
	{
		character = transform.root.GetComponent<Character> ();
		base.Awake ();
	}

	public override void UpdateAnimation ()
	{
		base.UpdateAnimation ();
		if (anim == null)
			return;
		
		anim.SetInteger ("Action", (int)character.Action);
		/*
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
		case CharacterAction.Jumping:
			anim.SetInteger ("Action", 3);
			break;
		case CharacterAction.Dead:
			anim.SetInteger ("Action", 4);
			break;
		case CharacterAction.Channeling:
			anim.SetInteger ("Action", 5);
			break;
		default:
			Debug.LogError ("Animation not implemented yet");
			break;
		}
		*/
	}
}

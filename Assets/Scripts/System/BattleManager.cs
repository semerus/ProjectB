using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {
	// active throughout battle scene

	// singleton
	private static BattleManager instance;

	public static BattleManager GetBattleManager() {
		if (!instance) {
			instance = GameObject.FindObjectOfType (typeof(BattleManager)) as BattleManager;
			if (!instance)
				Debug.LogError ("No active BattleManager in the scene");
		}
		return instance;
	}

	// list of IBattleHandler
	List<IBattleHandler> friendly = new List<IBattleHandler> ();
	List<IBattleHandler> neutral = new List<IBattleHandler> ();
	List<IBattleHandler> hostile = new List<IBattleHandler> ();

	// list of rigidbody
	// game state

	void Start() {
		// spawn all necessary characters

	}

	// add entity state
	public void AddEntity(IBattleHandler entity) {
		switch (entity.Team) {
		case Team.Friendly:
			if (!friendly.Contains (entity))
				friendly.Add (entity);
			break;
		case Team.Neutral:
			if (!neutral.Contains (entity))
				neutral.Add (entity);
			break;
		case Team.Hostile:
			if (!hostile.Contains (entity))
				neutral.Add (entity);
			break;
		}
	}

	public IBattleHandler[] GetEntities(Team team) {
		IBattleHandler[] arr;
		switch (team) {
		case Team.Friendly:
			friendly.CopyTo (arr);
			break;
		case Team.Hostile:
			hostile.CopyTo (arr);
			break;
		case Team.Neutral:
			neutral.CopyTo (arr);
			break;
		}
		return arr;
	}

	public void AttackByAllFriendly(IBattleHandler target) {
		if (target.Team == Team.Friendly) {
			Debug.Log ("Wrong target(friendly target)");
			return;
		}
		for (int i = 0; i < friendly.Count; i++) {
			// change condition here later
			if (friendly [i].State != CharacterState.Dead) {
				Hero hero = friendly [i] as Hero;
				hero.AutoAttack (target);
			}
		}
	}

	public void MoveAllFriendly(Vector3 target) {
		for (int i = 0; i < friendly.Count; i++) {
			// change condition here later
			if (friendly [i].State != CharacterState.Dead) {
				Hero hero = friendly [i] as Hero;
				hero.Move (target);
			}
		}
	}

	// check game
	public void CheckGame() {
		bool classifier = true;
		// win scenario
		for (int i = 0; i < hostile.Count; i++) {
			classifier = classifier && hostile [i].State == CharacterState.Dead; 
		}
		if (classifier) {
			Debug.Log ("Win");
			return;
		}

		// lose scenario
		classifier = true;
		for (int i = 0; i < friendly.Count; i++) {
			classifier = classifier && friendly [i].State == CharacterState.Dead; 
		}
		if (classifier) {
			Debug.Log ("Lose");
			return;
		}
	}

	// pause game

	// unpause game
}

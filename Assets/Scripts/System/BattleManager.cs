/*
 * Written by Insung Kim
 * Updated: 2017.09.11
 */
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour, ITimeHandler {
	// active throughout battle scene

	private enum GameState {
		Loading,
		Playing,
		Paused,
		Ended
	}

	// singleton
	private static BattleManager instance;
	private GameState gameState = GameState.Playing;
	private float gameTime;
	private Vector3[] heroPos = new Vector3[]{new Vector3(-5.7f, 0.65f), new Vector3(-7.5f, -0.3f), new Vector3(-3.75f, -0.3f), new Vector3(-5.7f, -2.1f)};
	private Vector3[] bossPos = new Vector3[]{new Vector3(6.17f, 0f), new Vector3(6.17f, -2.5f)};
	private Vector3[] movePos = new Vector3[]{new Vector3(0f, 1f), new Vector3(-1f, 0f), new Vector3(1f, 0f)};

	// list of all animators
	AnimationController[] animControllers;
	// game state

	// list of IBattleHandler
	List<IBattleHandler> friendly = new List<IBattleHandler> ();
	List<IBattleHandler> neutral = new List<IBattleHandler> ();
	List<IBattleHandler> hostile = new List<IBattleHandler> ();

	public static BattleManager GetBattleManager() {
		if (!instance) {
			instance = GameObject.FindObjectOfType (typeof(BattleManager)) as BattleManager;
			if (!instance)
				Debug.LogError ("No active BattleManager in the scene");
		}
		return instance;
	}

	#region ITimeHandler implementation

	public void RunTime ()
	{
		gameTime += Time.deltaTime;
		UIManager.GetUIManager ().UpdateTime (gameTime);
	}

	#endregion

	void Awake() {
		// Instantiate all necessary prefabs


		// add bool here to signal fully loaded
	}

	void Start() {
		SetGame ();
		StartGame ();
		animControllers = GameObject.FindObjectsOfType<AnimationController> ();
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
				hostile.Add (entity);
			break;
		}
	}

	public IBattleHandler[] GetEntities(Team team) {
		IBattleHandler[] arr;
		List<IBattleHandler> temp = new List<IBattleHandler> ();
		switch (team) {
		case Team.Friendly:
			foreach (var item in friendly) {
				if (item.Action != CharacterAction.Dead)
					temp.Add (item);
			}
			arr = new IBattleHandler[temp.Count];
			temp.CopyTo (arr);
			break;
		case Team.Hostile:
			foreach (var item in hostile) {
				if (item.Action != CharacterAction.Dead)
					temp.Add (item);
			}
			arr = new IBattleHandler[temp.Count];
			temp.CopyTo (arr);
			break;
		case Team.Neutral:
			foreach (var item in neutral) {
				if (item.Action != CharacterAction.Dead)
					temp.Add (item);
			}
			arr = new IBattleHandler[temp.Count];
			temp.CopyTo (arr);
			break;
		default:
			arr = null;
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
			if (friendly[i].Action != CharacterAction.Dead) {
				Hero hero = friendly [i] as Hero;
				hero.AutoAttack (target);
			}
		}
	}

	public void MoveAllFriendly(Vector3 target) {
		for (int i = 0; i < friendly.Count; i++) {
			// change condition here later
			if (friendly[i].Action != CharacterAction.Dead) {
				Hero hero = friendly [i] as Hero;
				hero.StopMove ();
				hero.BeginMove (target + movePos[i]);
                hero.RemoveTarget();
            }
		}
	}

	// check game
	public void CheckGame() {
		bool classifier = true;

		// win scenario
		for (int i = 0; i < hostile.Count; i++) {
			classifier = classifier && hostile [i].Action == CharacterAction.Dead;
		}
		if (classifier) {
			Debug.Log ("Win");
			Debugging.DebugWindow ("Player Wins!");
			return;
		}

		// lose scenario
		classifier = true;
		for (int i = 0; i < friendly.Count; i++) {
			classifier = classifier && friendly [i].Action == CharacterAction.Dead;
		}
		if (classifier) {
			Debug.Log ("Lose");
			Debugging.DebugWindow ("Boss Wins!");
			return;
		}
	}

	public void StartGame() {
		gameTime = 0f;
		TimeSystem.GetTimeSystem ().AddTimer (this);
	}

	// pause game
	public void PauseGame() {
		// temporary animation pausing system
		animControllers = GameObject.FindObjectsOfType<AnimationController> ();

		switch (gameState) {
		case GameState.Playing:
			Debug.Log ("Game Paused");
			// pause time
			TimeSystem.GetTimeSystem().PauseTime();
			// pause animation
			foreach (var item in animControllers) {
				item.PauseAnimation ();
			}
			gameState = GameState.Paused;
			break;
		case GameState.Paused:
			Debug.Log ("Game UnPaused");
			// unpause time
			TimeSystem.GetTimeSystem().UnPauseTime();
			// unpause animation
			foreach (var item in animControllers) {
				item.UnPauseAnimation ();
			}
			gameState = GameState.Playing;
			break;
		default:
			break;
		}
	}
		
	// pause bosses
	bool isBossPaused = false;

	public void SwitchBossPause() {
		if (!isBossPaused) {
			Debug.Log ("Pause Boss");
			isBossPaused = true;
			IBattleHandler[] enemies = GetEntities (Team.Hostile);
			for (int i = 0; i < enemies.Length; i++) {
				Character c = enemies [i] as Character;
				if (c != null) {
					TimeSystem.GetTimeSystem ().DeleteTimer (c);
				}
			}
		} else {
			Debug.Log ("Unpause Boss");
			isBossPaused = false;
			IBattleHandler[] enemies = GetEntities (Team.Hostile);
			for (int i = 0; i < enemies.Length; i++) {
				Character c = enemies [i] as Character;
				if (c != null) {
					TimeSystem.GetTimeSystem ().AddTimer (c);
				}
			}
		}
	}

	// end game(timer)

	private void SetGame() {
		// temporary id values
		int[] hs = {1000, 1001, 1002};
		int[] bs = {2000, 2001};
		int[][] hss0 = new int[][] {
			new int[] { 100030, 100040, 100000, 100010, 100020 },
			new int[] { 100130, 100100, 100110, 100120 },
			new int[] { 100230, 100240, 100200, 100210, 100220 }
		};

		int[][] bss0 = new int[][] {
			new int[] { 200000, 200010, 200020},
			new int[] { 200100, 200110, 200120, 200130, 200140}
		};
		////////////////////

		if (GameObject.Find ("Projectiles") == null) {
			GameObject projectile = new GameObject ();
			projectile.transform.name = "Projectiles";
		}

		for (int i = 0; i < hs.Length; i++) {
			Dictionary<string, object> param = (Dictionary<string, object>)LoadManager.Instance.HeroData [hs [i]];
			GameObject prefab = Resources.Load<GameObject> (param ["path_prefab"].ToString ());
			Hero hero = Instantiate (prefab).GetComponent<Hero> ();
			hero.transform.position = heroPos [i];
			hero.Spawn (param, hss0[i]);
		}

		for (int i = 0; i < bs.Length; i++) {
			Dictionary<string, object> param = (Dictionary<string, object>)LoadManager.Instance.BossData [bs [i]];
			GameObject prefab = Resources.Load<GameObject> (param ["path_prefab"].ToString ());
			Enemy enemy = Instantiate (prefab).GetComponent<Enemy> ();
			enemy.transform.position = bossPos [i];
			enemy.Spawn (param, bss0[i]);
		}

		Debug.Log ("Scene creation complete");
	}
}


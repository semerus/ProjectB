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

	// temporary -> will be loaded by gamemanager
	public GameObject map;
	public GameObject[] heroes;
	public GameObject[] bosses;


	// singleton
	private static BattleManager instance;
	private GameState gameState = GameState.Playing;
	private float gameTime;
	private new Vector3[] heroPos = new Vector3[]{new Vector3(-5.7f, 0.65f), new Vector3(-7.5f, -0.3f), new Vector3(-3.75f, -0.3f), new Vector3(-5.7f, -2.1f)};
	private new Vector3[] bossPos = new Vector3[]{new Vector3(6.17f, 0f), new Vector3(6.17f, -2.5f)};

	public static BattleManager GetBattleManager() {
		if (!instance) {
			instance = GameObject.FindObjectOfType (typeof(BattleManager)) as BattleManager;
			if (!instance)
				Debug.LogError ("No active BattleManager in the scene");
		}
		return instance;
	}

	// list of all animators
	AnimationController[] animControllers;
	// game state

	// list of IBattleHandler
	List<IBattleHandler> friendly = new List<IBattleHandler> ();
	List<IBattleHandler> neutral = new List<IBattleHandler> ();
	List<IBattleHandler> hostile = new List<IBattleHandler> ();

	#region ITimeHandler implementation

	public void RunTime ()
	{
		gameTime += Time.deltaTime;
		UIManager.GetUIManager ().UpdateTime (gameTime);
	}

	#endregion

	void Start() {
		// temporary load info
		string[,] skills = new string[1, 2];
		skills [0,0] = "Fighter_Attack";
		skills [0,1] = "Figher_MeowPunch";

		animControllers = GameObject.FindObjectsOfType<AnimationController> ();

		BattleInfo info = new BattleInfo(map, heroes, skills, bosses);
		StartGame (info);
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
		switch (team) {
		case Team.Friendly:
			arr = new IBattleHandler[friendly.Count];
			friendly.CopyTo (arr);
			break;
		case Team.Hostile:
			arr = new IBattleHandler[hostile.Count];
			hostile.CopyTo (arr);
			break;
		case Team.Neutral:
			arr = new IBattleHandler[hostile.Count];
			neutral.CopyTo (arr);
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
				hero.BeginMove (target);
                hero.RemoveTarget();
            }
		}
	}

	// check game
	public void CheckGame() {
		bool classifier = true;

		Debug.Log ("sss: " + hostile.Count);
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

	public void StartGame(BattleInfo info) {
		gameTime = 0f;
		TimeSystem.GetTimeSystem ().AddTimer (this);

//		gameObject.AddComponent (Type.GetType (info.heroSkills [0, 0]));
	}

	// pause game
	public void PauseGame() {
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
	// unpause game

	// end game(timer)
}


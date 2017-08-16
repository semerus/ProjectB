/*
 * Written by Insung Kim
 * Updated: 2017.08.13
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	// singleton

	// active throughout the whole game
	public string version;
	private int phase = 0;
	private static GameManager instance;

	private LoadManager load = LoadManager.Instance;

	public static GameManager GetGameManager() {
		if (!instance) {
			instance = GameObject.FindObjectOfType (typeof(GameManager)) as GameManager;
			if (!instance)
				Debug.LogError ("No active GameManager in the scene");
		}
		return instance;
	}

    void Awake()
    {
		GetGameManager ();

		if (instance != this) {
			Destroy (this.gameObject);
		}

		DontDestroyOnLoad (gameObject);

		//Set Scene view as horizontal
        Screen.orientation = ScreenOrientation.LandscapeLeft;

		phase = SceneManager.GetActiveScene().buildIndex;

		LoadData ();
    }

	void Start() {
		
	}

	void Update() {
		switch(phase) {
		case 0: // title scene
			#if UNITY_EDITOR
			if (Input.GetMouseButtonUp (0)) {
				phase = 1;
				SceneManager.LoadScene ("Battlefield");
			}
			#endif

			#if UNITY_ANDROID
			if (Input.touchCount > 0) {
				phase = 1;
				SceneManager.LoadScene ("Battlefield");
			}
			#endif
			break;
		case 1: // battle scene
			break;
		default:
			break;
		}
	}

	void LoadData() {
		load.LoadData ();
	}
}

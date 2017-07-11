using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debugging : MonoBehaviour {

	public static Text window;

	void Start() {
		window = GameObject.Find ("Debugging").GetComponent<Text> ();
	}

	public static void DebugWindow(string log) {
		window.text = log;
	}

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(0);
    }
}

using UnityEngine.UI;
using UnityEngine;

public class Debugging : MonoBehaviour {

	public static Text window;

	void Start() {
		window = GameObject.Find ("Debugging").GetComponent<Text> ();
	}

	public static void DebugWindow(string log) {
		window.text = log;
	}
}

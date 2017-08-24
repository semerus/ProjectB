using UnityEngine;
using UnityEngine.UI;

public class BlinkText : MonoBehaviour {

	public GameObject blinkText;
	public Text version;
	private float timer = 0f;

	void Start() {
		version.text = "Ver " + GameManager.GetGameManager ().version;
	}

	void Update() {
		timer += Time.deltaTime;

		if (timer > 0.65f) {
			blinkText.SetActive (!blinkText.activeSelf);
			timer = 0f;
		}
	}
}

using UnityEngine;
using UnityEngine.UI;

public sealed class UIManager : MonoBehaviour {

	private static UIManager instance;
	public GameObject timer;
	private SkillUI skillPanel;

	public SkillUI SkillPanel {
		get {
			return skillPanel;
		}
	}

	public static UIManager GetUIManager() {
		if (!instance) {
			instance = GameObject.FindObjectOfType (typeof(UIManager)) as UIManager;
			if (!instance)
				Debug.LogError ("No active UIManager in the scene");
		}
		return instance; 
	}

	void Awake() {
		GetUIManager ();
		skillPanel = SkillUI.GetSkillUI ();
	}

	// update timer
	public void UpdateTime(float time) {
		Text text = timer.GetComponentInChildren<Text> ();
		string minutes = ((int)(time / 60f)).ToString("D2");
		string seconds = ((int)(time % 60f)).ToString("D2");
		string temp = minutes + ":" + seconds;
		text.text = temp;
	}
}

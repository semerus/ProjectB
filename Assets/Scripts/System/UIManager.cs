using UnityEngine;
using UnityEngine.UI;

public sealed class UIManager : MonoBehaviour {

	private static UIManager instance;
	public GameObject timer;
	public GameObject skillPanel;

	private SkillButton[] skills;

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
		skills = skillPanel.GetComponentsInChildren<SkillButton> ();
	}

	// update timer
	public void UpdateTime(float time) {
		Text text = timer.GetComponentInChildren<Text> ();
		string minutes = ((int)(time / 60f)).ToString("D2");
		string seconds = ((int)(time % 60f)).ToString("D2");
		string temp = minutes + ":" + seconds;
		text.text = temp;
	}

	public void SetSkills(IInGameUI[] uis) {
		for (int i = 0; i < skills.Length; i++) {
			skills [i].button.image.sprite = uis [i].InGameUI;
			skills [i].OnReady ();
		}
	}

	// update skill cooldowns using IInGameUI[]
	public void UpdateSkills(IInGameUI[] uis) {
		for (int i = 0; i < skills.Length; i++) {
			if (uis [i].CurCoolDown == 0)
				skills [i].OnReady ();
			else
				skills [i].UpdateCooldown (uis [i].CurCoolDown / uis [i].MaxCoolDown, (int)uis [i].CurCoolDown);
		}
	}
}

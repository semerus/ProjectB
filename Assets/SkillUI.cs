using UnityEngine;

public class SkillUI : MonoBehaviour {

	private static SkillUI instance;
	SkillButton[] buttons;

	public static SkillUI GetSkillUI() {
		if (!instance) {
			instance = GameObject.FindObjectOfType (typeof(SkillUI)) as SkillUI;
			if (!instance)
				Debug.LogError ("No active SkillUI in the scene");
		}
		return instance; 
	}

	void Awake() {
		GetSkillUI ();
		buttons = GetComponentsInChildren<SkillButton> ();
		CloseSkills ();
	}

	/// <summary>
	/// Open up skill hud
	/// </summary>
	public void OpenSkills(IHeroActiveUI[] active) {
		if (!gameObject.activeSelf) {
			gameObject.SetActive (true);
		}
		for (int i = 0; i < buttons.Length; i++) {
			buttons [i].LinkSkill (active [i]);
		}
	}

	/// <summary>
	/// Closes the skill hud
	/// </summary>
	public void CloseSkills() {
		if (gameObject.activeSelf) {
			gameObject.SetActive (false);
		}
	}
}

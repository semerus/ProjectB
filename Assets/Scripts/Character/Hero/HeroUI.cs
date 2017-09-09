/*
 * Written by Insung Kim
 * Updated: 2017.08.16
 */
using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour {

	protected float maxLength = 1.2f;
	public HpBar hpBar;
	public Text ccBar;

	void Start() {
		hpBar = GetComponentInChildren<HpBar> ();
		ccBar = GetComponentInChildren<Text> ();

		// separate this later for customization for each hero(different sizes of hp bars)
		SetUI (maxLength);
	}

	public void SetUI(float length) {
		hpBar.SetHp (length);
	}

	public void UpdateHp(float percent) {
		hpBar.UpdateHp (percent);
	}

	public void UpdateCC(int status) {
		switch (status) {
		case CharacterStatus.Silenced:
			ccBar.text = "Silenced";
			break;
		case CharacterStatus.Rooted:
			ccBar.text = "Rooted";
			break;
		case CharacterStatus.Stunned:
			ccBar.text = "Stunned";
			break;
		case CharacterStatus.Idle:
			ccBar.text = "";
			break;
		case CharacterStatus.Immune:
			ccBar.text = "Immune";
			break;
		default:
			break;
		}
	}
}

using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour {

	public RectTransform curHpBar;
	public Text ccBar;

	public void UpdateHp(float percent) {
		// change 1f later
		curHpBar.sizeDelta = new Vector2(1f * percent, curHpBar.rect.height);
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

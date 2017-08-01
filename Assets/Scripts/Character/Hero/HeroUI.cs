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
			break;
		case CharacterStatus.Rooted:
			break;
		case CharacterStatus.Stunned:
			break;
		case CharacterStatus.Idle:
			break;
		case CharacterStatus.Immune:
			break;
		default:
			break;
		}
	}
}

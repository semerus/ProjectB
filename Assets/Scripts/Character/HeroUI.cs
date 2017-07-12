using UnityEngine;

public class HeroUI : MonoBehaviour {

	public RectTransform curHpBar;

	public void UpdateHp(float percent) {
		// change 1f later
		curHpBar.sizeDelta = new Vector2(1f * percent, curHpBar.rect.height);
	}

	public void ShowSkill() {
		
	}
}

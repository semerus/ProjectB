using UnityEngine;

public class EnemyUI : MonoBehaviour {

	public RectTransform curHpBar;

	public void UpdateHp(float percent) {
		// change 1f later
		curHpBar.sizeDelta = new Vector2(700f * percent, curHpBar.rect.height);
	}
}

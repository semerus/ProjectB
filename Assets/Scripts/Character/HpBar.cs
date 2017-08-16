/*
 * Written by Insung Kim
 * Updated: 2017.08.16
 */
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour {
	protected float maxLength;
	public RectTransform maxBar;
	public RectTransform curBar;

	void Awake() {
		maxBar = transform.GetChild (0).GetComponent<RectTransform> ();
		curBar = transform.GetChild (1).GetComponent<RectTransform> ();
	}

	public void SetHp(float length) {
		maxLength = length;
		maxBar.sizeDelta = new Vector2(maxLength, curBar.rect.height);
		curBar.sizeDelta = new Vector2(maxLength, curBar.rect.height);
	}

	public void UpdateHp(float percent) {
		curBar.sizeDelta = new Vector2(maxLength * percent, curBar.rect.height);
	}
}

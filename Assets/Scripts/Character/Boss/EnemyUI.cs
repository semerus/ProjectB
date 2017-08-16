/*
 * Written by Insung Kim
 * Updated: 2017.08.16
 */
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour {

	protected float maxLength = 700f;
	public HpBar hpBar;
	public Text ccBar;

	void Start() {
		SetEnemyUI ();
	}

	public void SetEnemyUI() {
		if (!EnemyHp.GetEnemyHp ().AssignHpBar (maxLength, out hpBar)) {
			Debug.LogError ("Enemy hpBar not assigned");
		}
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

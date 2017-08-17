/*
 * Written by Insung Kim
 * Updated: 2017.08.16
 */
using UnityEngine;
using UnityEngine.UI;

public class EnemyHp : MonoBehaviour {
	private static EnemyHp instance;

	public static EnemyHp GetEnemyHp() {
		if (!instance) {
			instance = GameObject.FindObjectOfType (typeof(EnemyHp)) as EnemyHp;
			if (!instance)
				Debug.LogError ("No active EnemyHp in the scene");
		}
		return instance;
	}

	private int bossCount = 0;
	public HpBar[] hpBars;

	void Awake() {
		GetEnemyHp ();
	}

	void Start() {
		hpBars = GetComponentsInChildren<HpBar> ();
		for (int i = 0; i < hpBars.Length; i++) {
			hpBars [i].gameObject.SetActive (false);
		}
	}

	public bool AssignHpBar(float length, out HpBar hpBar) {
		if (bossCount >= hpBars.Length) {
			hpBar = null;
			return false;
		} else {
			hpBar = hpBars [bossCount++];
			hpBar.gameObject.SetActive (true);
			hpBar.SetHp (length);
			return true;
		}
	}
}

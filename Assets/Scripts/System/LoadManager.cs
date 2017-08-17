/*
 * Written by Insung Kim
 * Updated: 2017.08.11
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager {

	const string hero_data_path = "Data/hero2";
	const string boss_data_path = "Data/boss";
	const string skill_data_path = "Data/skill";

	private Dictionary<int, object> heroData;
	private Dictionary<int, object> bossData;
	private Dictionary<int, object> skillData;


	private static LoadManager instance;

	private LoadManager() {	}

	public static LoadManager Instance {
		get {
			if (instance == null) {
				instance = new LoadManager ();
			}
			return instance;
		}
	}

	public Dictionary<int, object> HeroData {
		get {
			return heroData;
		}
	}

	public Dictionary<int, object> BossData {
		get {
			return bossData;
		}
	}

	public Dictionary<int, object> SkillData {
		get {
			return skillData;
		}
	}

	public void LoadData() {
		JsonManager jm = new JsonManager ();
		heroData = jm.LoadData (hero_data_path);
		bossData = jm.LoadData (boss_data_path);
		skillData = jm.LoadData (skill_data_path);

		Debug.Log ("Data ID dictionary loaded");
	}
}

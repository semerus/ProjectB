using UnityEngine;

public struct BattleInfo {
	public GameObject map;
	public GameObject[] heroes;
	public string[,] heroSkills;
	public GameObject[] bosses;

	public BattleInfo(GameObject map, GameObject[] heroes, string[,] heroSkills, GameObject[] bosses) {
		this.map = map;
		this.heroes = heroes;
		this.heroSkills = heroSkills;
		this.bosses = bosses;
	}
}

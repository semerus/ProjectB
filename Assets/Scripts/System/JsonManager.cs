/*
 * Written by Insung Kim
 * Updated: 2017.08.11
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;

public class JsonManager {

	public Dictionary<int, object> LoadData(string path) {
		Dictionary<string, object> raw;
		Dictionary<string, object>[] param;
		Dictionary<int, object> dic = new Dictionary<int, object>();
		TextAsset data = Resources.Load<TextAsset> (path);
		string json = data.ToString ();
		raw = JsonReader.Deserialize<Dictionary<string, object>> (json);
		param = (Dictionary<string, object>[])raw ["data"];

		for (int i = 0; i < param.Length; ++i) {
			int id = (int)param [i] ["id"];
			dic.Add (id, param [i]);
		}

		return dic;
	}
}

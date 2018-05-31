using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

static public class MonsterType {
	static public bool loaded = false;
	static public JsonData data;

	static public void LoadMonsterData() {
		if(!loaded) {
			string jsonString = File.ReadAllText(Application.dataPath + "/data/MonsterData.json");
			data = JsonMapper.ToObject(jsonString);
			loaded = true;
		}
	}
}

public enum MONSTER_RACE {
	ANIMAL
}
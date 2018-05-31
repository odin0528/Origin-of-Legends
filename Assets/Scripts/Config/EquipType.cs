using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

static public class EquipType {
	static public bool loaded = false;
	static public JsonData equipType;

	static public EQUIP_CATEGORY category;
	static public EQUIP_SORT sort;
	static public EQUIP_TYPE typ;

	static public void LoadEquipData() {
		if(!loaded) {
			string jsonString = File.ReadAllText(Application.dataPath + "/data/EquipData.json");
			equipType = JsonMapper.ToObject(jsonString);
			loaded = true;
		}
	}
}


public enum EQUIP_CATEGORY {
	WEAPON,
	ARMOR
}

public enum EQUIP_SORT {
	WEAPON,
	HEAD,
	CHEST,
	HAND,
	FEET
}

public enum EQUIP_TYPE {
	STEEL,
	LEATHER,
	CLOTH
}
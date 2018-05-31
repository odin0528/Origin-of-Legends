using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Item {
	public string title;
	public List<string> modDesc;
	public int dbId, level, rarity, legendId, itemLv, itemAffixLv, hp;

	virtual public void init() {

	}
}

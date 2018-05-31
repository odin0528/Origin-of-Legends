using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
[Serializable]
public class HeroInfo {
	public CHARACTER character;
	public int dbId;
	public int lv;

	public Dictionary<CHARACTER_EQUIP_SORT, int> equipment = new Dictionary<CHARACTER_EQUIP_SORT, int>();
	//public List<SKILL_LIST> skillDeck = new List<SKILL_LIST>();
}

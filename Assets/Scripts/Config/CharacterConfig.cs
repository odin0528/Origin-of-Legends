using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

static public class CharacterConfig {
	static public Dictionary<CHARACTER, string> prefabPath = new Dictionary<CHARACTER, string>() {
		{ CHARACTER.ASTER, "Aster"},
		{ CHARACTER.OLRUN, "Olrun"}
	};
}


public enum CHARACTER {
	ASTER,
	OLRUN
}

public enum CHARACTER_EQUIP_SORT {
	WEAPON,
	HEAD,
	CHEST,
	HAND,
	FEET
}
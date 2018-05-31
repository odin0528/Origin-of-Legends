using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

/*static public class SkillConfig {
	static public Dictionary<SKILL_LIST, string> prefabPath = new Dictionary<SKILL_LIST, string>() {
		{ SKILL_LIST.CLEAVE, "Prefabs/Skills/Cleave"},
		{ SKILL_LIST.GROUND_SLAM, "Prefabs/Skills/GroundSlam"},
		{ SKILL_LIST.ARC, "Prefabs/Skills/Arc"},
		{ SKILL_LIST.FIRE_BOLT, "Prefabs/Skills/FireBolt"}
	};
}*/

public enum SKILL_TYPE {
	ATTACK,
	SPELL
}


public enum RUNE_LIST {
	// A,
	C,
	// F,
	H,
	M,
	S,
	//Y
}

/*public enum SKILL_LIST {
	CLEAVE,
	GROUND_SLAM,
	ARC,
	FIRE_BOLT
}*/

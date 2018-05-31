using UnityEngine;
using System.Collections;
using System.Collections.Generic;

static public class EquipMod {
	static public Dictionary<MOD_TYPE, System.Type> list = new Dictionary<MOD_TYPE, System.Type>() {
		{ MOD_TYPE.WEAPON_PHYSIC_POINT_DAMAGE,		typeof(ModWeaponPhysicPointDamage)},
		{ MOD_TYPE.WEAPON_PHYSIC_DAMAGE,			typeof(ModWeaponPhysicDamage)},
		{ MOD_TYPE.WEAPON_SPELL_POINT_DAMAGE,		typeof(ModWeaponSpellPointDamage)},
		{ MOD_TYPE.WEAPON_SPELL_DAMAGE,				typeof(ModWeaponSpellDamage)},
		{ MOD_TYPE.HEATH,							typeof(ModHeath)},
		{ MOD_TYPE.WEAPON_ATTACK_SPEED,				typeof(ModWeaponAttackSpeed)},
		{ MOD_TYPE.HEAL_ON_HIT,						typeof(ModHealOnHit)}
	};

	static public string descInfo = "{0}",
		descNumber = "{0} <color=lightblue>{1}</color>\n",
		descPercent = "{0} <color=lightblue>{1}%</color>\n";
}

public enum MOD_TYPE {
	NONE,
	WEAPON_PHYSIC_POINT_DAMAGE,
	WEAPON_PHYSIC_DAMAGE,
	WEAPON_SPELL_POINT_DAMAGE,
	WEAPON_SPELL_DAMAGE,
	WEAPON_ATTACK_SPEED,
	HEAL_ON_HIT,
	HEATH,
}

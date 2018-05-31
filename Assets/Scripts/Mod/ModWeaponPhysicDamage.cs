using System;
using UnityEngine;
using System.Collections.Generic;

class ModWeaponPhysicDamage:Mod {

	public ModWeaponPhysicDamage() {
		title = new String[5] {"堅固", "堅硬", "強壯", "鋼鐵", "硬硬DER"};
		desc = "增加 {0}% 物理傷害";
	}

	override public void ApplyToEquip(Equip equip) {
		Debug.Log(title[affixLv]);
		equip.prop.weaponPhysicDamage += value[0];
		equip.modDesc.Add(String.Format(desc, value[0] * 100));
	}
}

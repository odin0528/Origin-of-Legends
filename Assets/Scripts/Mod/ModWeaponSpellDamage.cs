using System;
using System.Collections.Generic;

class ModWeaponSpellDamage:Mod {

	public ModWeaponSpellDamage() {
		desc = "增加 {0}% 法術傷害";
	}

	override public void ApplyToEquip(Equip equip) {
		equip.prop.weaponSpellDamage += value[0];
		equip.modDesc.Add(String.Format(desc, value[0] * 100));
	}
}

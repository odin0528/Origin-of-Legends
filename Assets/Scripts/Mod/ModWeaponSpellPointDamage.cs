using System;
using System.Collections.Generic;

class ModWeaponSpellPointDamage: Mod{

	public ModWeaponSpellPointDamage() {
		desc = "增加 {0} - {1} 法術傷害";
	}

	override public void ApplyToEquip(Equip equip) {
		equip.spellMinDamage += (int) value[0];
		equip.spellMaxDamage += (int) value[1];
		equip.modDesc.Add(String.Format(desc, value[0], value[1]));
	}
}

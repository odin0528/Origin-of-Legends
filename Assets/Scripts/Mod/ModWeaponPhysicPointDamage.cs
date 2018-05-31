using System;
using System.Collections.Generic;

class ModWeaponPhysicPointDamage: Mod{

	public ModWeaponPhysicPointDamage() {
		desc = "增加 {0} - {1} 物理傷害";
	}

	override public void ApplyToEquip(Equip equip) {
		equip.physicMinDamage += (int) value[0];
		equip.physicMaxDamage += (int) value[1];
		equip.modDesc.Add(String.Format(desc, value[0], value[1]));
	}
}

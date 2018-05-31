using System;
using System.Collections.Generic;

class ModHealOnHit: Mod{

	public ModHealOnHit() {
		desc = "每擊中一個敵人回復 {0} 生命";
	}

	override public void ApplyToEquip(Equip equip) {
		equip.prop.healOnHit += (int) value[0];
		equip.modDesc.Add(String.Format(desc, value[0]));
	}
}

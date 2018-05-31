using System;
using System.Collections.Generic;

class ModHeath : Mod{

	public ModHeath() {
		desc = "+ {0} 最大生命";
	}

	override public void ApplyToEquip(Equip equip) {
		equip.prop.hp += (int) value[0];
		equip.modDesc.Add(String.Format(desc, value[0]));
	}
}

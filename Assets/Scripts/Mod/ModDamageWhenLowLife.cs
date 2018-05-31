using UnityEngine;
using System;
using System.Collections.Generic;

class ModDamageWhenLowLife: Mod{

	public ModDamageWhenLowLife() {
		desc = "瀕死時增加 {0}% 傷害";
	}

	override public void ApplyToEquip(Equip equip) {
		equip.BeforeCalc += BeforeCalc;
		equip.prop.hp += (int) value[0];
		equip.modDesc.Add(String.Format(desc, value[0] * 100));
	}

	public void BeforeCalc(Property prop) {
		if(prop.isLowLife) {
			prop.increaseDamage += value[0];
		}
	}
}

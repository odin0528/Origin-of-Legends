using System;
using System.Collections.Generic;

class ModWeaponAttackSpeed : Mod{

	public ModWeaponAttackSpeed() {
		desc = "增加 {0}% 攻擊速度";
	}

	override public void ApplyToEquip(Equip equip) {
		equip.weaponSpeed = (float) Math.Round(equip.weaponSpeed / (1 + value[0]), 2);
		equip.modDesc.Add(String.Format(desc, value[0] * 100));
	}
}

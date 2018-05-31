using UnityEngine;
using System.Collections;

public class Redbeak : Equip {
	override public void Init() {
		base.Init();

		title = "紅喙短刃";
		skin = "Image/Weapon/Sword/Unique/OneHandSword1Unique";

		ApplyMod(new ModWeaponPhysicPointDamage() { value = new float[] { 2, 6 } });
		ApplyMod(new ModWeaponPhysicDamage() { value = new float[] { 0.5f } });
		ApplyMod(new ModDamageWhenLowLife() { value = new float[] { 1 } });
		ApplyMod(new ModWeaponAttackSpeed() { value = new float[] { 0.1f } });
		ApplyMod(new ModHealOnHit() { value = new float[] { 2 } });

	}
}

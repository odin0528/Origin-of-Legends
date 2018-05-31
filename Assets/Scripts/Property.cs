using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
[SerializeField]
public class Property {
	public float hp;

	public bool isComa = false;
	public bool isLowLife = false;

	public float physicMinDamage, physicMaxDamage, 
					fireMinDamage, fireMaxDamage,
					iceMinDamage, iceMaxDamage,
					thunderMinDamage, thunderMaxDamage,
					spellMinDamage, spellMaxDamage,
					spellFireMinDamage, spellFireMaxDamage,
					spellIceMinDamage, spellIceMaxDamage,
					spellThunderMinDamage, spellThunderMaxDamage,
					weaponCrit, weaponSpeed, increasedWeaponSpeed;

	//速度相關
	public float increasedAttackSpeed, castSpeed;

	//法傷相關
	public float weaponSpellDamage, increaseSpellDamage;

	//攻擊傷害相關
	public float weaponPhysicDamage, increasePhysicDamage;

	//全域計算相關
	public float spellCritRate, globalCritRate, critMultiplier, increaseDamage;
	public float hit, armor, dodge;

	public float healOnHit;

	public Property Clone() {
		Property newProp = (Property) this.MemberwiseClone();
		return newProp;
	}
}

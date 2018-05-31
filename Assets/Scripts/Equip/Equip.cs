using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Equip {
	public string baseName, title, style;
	public List<string> modDesc;
	public int dbId, level, rarity, legendId, itemLv, itemAffixLv, hp;
	public CHARACTER character;		//限定的角色
	public EQUIP_CATEGORY category;
	public EQUIP_SORT sort;
	public EQUIP_TYPE type;
	//public float speed, crit;
	public Mod baseMod;
	public List<Mod> modList = new List<Mod>();
	public Property prop = new Property();
	public float physicMinDamage, physicMaxDamage, spellMinDamage, spellMaxDamage, weaponSpeed, weaponCrit;
	public string skin;

	//裝備需求數值
	public int nlv, nstr, ndex, nint;

	
	public event BattleManager.calculateDelegate BeforeCalc;
	public event BattleManager.attackDelegate BeforeAttack;

	virtual public void Init() {
		string[] typeArray = style.Split('-');

		category = (EQUIP_CATEGORY) int.Parse(typeArray[0]);
		if(category == EQUIP_CATEGORY.WEAPON) {
			character = (CHARACTER) int.Parse(typeArray[1]);

			string _cate = Enum.GetName(typeof(EQUIP_CATEGORY), category);
			string _character = Enum.GetName(typeof(CHARACTER), character);
			JsonUtility.FromJsonOverwrite(EquipType.equipType[_cate][_character][level - 1].ToJson(), this);
		} else {
			sort = (EQUIP_SORT) int.Parse(typeArray[1]);
			type = (EQUIP_TYPE) int.Parse(typeArray[2]);

			string _cate = Enum.GetName(typeof(EQUIP_CATEGORY), category);
			string _sort = Enum.GetName(typeof(EQUIP_SORT), sort);
			string _type = Enum.GetName(typeof(EQUIP_TYPE), type);
			JsonUtility.FromJsonOverwrite(EquipType.equipType[_cate][_sort][_type][level - 1].ToJson(), this);
		}
		//EQUIP_BASE
		//if(EquipType.equipType[_cate][_sort][_type][level - 1]["base"] != null) {
			//baseMod
		//}
		//ModInfo modInfo = new ModInfo();
	}

	//套用裝備基底詞綴效果
	public void ApplyBaseMod(int[] value) {
		//Mod modInfo = new Mod();
	}

	//套用詞綴效果
	public void ApplyMod(Mod mod) {
		mod.ApplyToEquip(this);
		modList.Add(mod);
	}

	//增加新詞綴時來這邊加公式
	public void ApplyToHero(Hero hero) {
		hero.oriProp.increasedWeaponSpeed += prop.increasedWeaponSpeed;
		hero.oriProp.increasedAttackSpeed += prop.increasedAttackSpeed;

		hero.oriProp.hp += prop.hp;
		hero.oriProp.healOnHit += prop.healOnHit;
	}

	public void ApplyToHeroAsAttack(Hero hero) {
		hero.oriProp.physicMinDamage = physicMinDamage;
		hero.oriProp.physicMaxDamage = physicMaxDamage;
		hero.oriProp.spellMinDamage = spellMinDamage;
		hero.oriProp.spellMaxDamage = spellMaxDamage;
		hero.oriProp.weaponCrit = weaponCrit;
		hero.oriProp.weaponSpeed = weaponSpeed;
		hero.attackTimer = 0.0f;
		ApplyToHero(hero);
	}
	//進行攻擊 或 技能時進來跑裝備的效果
	public void PreCalc(Property prop, string equipSort) {
		/*if(beforeCalc != null)
			beforeCalc(prop, equipSort);*/
	}

	//計算裝備數值
	public void Calculate() {
		physicMinDamage = Mathf.RoundToInt(physicMinDamage * (1 + prop.weaponPhysicDamage));
		physicMaxDamage = Mathf.RoundToInt(physicMaxDamage * (1 + prop.weaponPhysicDamage));
		spellMinDamage = Mathf.RoundToInt(spellMinDamage * (1 + prop.weaponSpellDamage));
		spellMaxDamage = Mathf.RoundToInt(spellMaxDamage * (1 + prop.weaponSpellDamage));
#if UNITY_EDITOR
		GetDesc();
#endif
	}

	public string GetTitle(){
		if(title != null)
			return String.Format(EquipMod.descInfo, title + ", " + baseName);
		else
			return String.Format(EquipMod.descInfo, baseName);
	}

	public string GetDesc() {
		string output = "";
		output += String.Format(EquipMod.descNumber, "物理傷害：", physicMinDamage + "-" + physicMaxDamage);
		if(spellMinDamage > 0 && spellMaxDamage > 0)
			output += String.Format(EquipMod.descNumber, "法術傷害：", spellMinDamage + "-" + spellMaxDamage);
		output += String.Format(EquipMod.descPercent, "暴擊機率：", weaponCrit * 100);
		output += String.Format(EquipMod.descNumber, "攻擊速度：", weaponSpeed);

#if UNITY_EDITOR
		// Debug.Log("<size=20>" + output + "</size>");
#endif
		return output;
	}

	public string GetModDesc(){
		string output = "";
		foreach(string desc in modDesc) {
			output += desc + "\n";
		}
		return output;
	}
}

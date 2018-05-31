using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

abstract public class Skill : MonoBehaviour {
	protected Unit unit;
	protected List<Unit> target = new List<Unit>();
	public string title;
	public List<string> needRune = new List<string>();
	public List<CardInfo> usedCard = new List<CardInfo>();
	public int[,] range;
	public SKILL_TYPE type = SKILL_TYPE.ATTACK;
	public float damageRate;
	//法術暴擊率 只有法術需要設
	public float spellCrit;
	public float convertToPhysic, convertToFire, convertToIce, convertToThunder;
	
	public List<Buff> givenBuff = new List<Buff>();
	public List<Buff> getBuff = new List<Buff>();

	public bool isMoveCharacterToTarget = true;
	public bool isJump = false;

	public GameObject effectPrefab;
	protected DamageInfo damageInfo;

	void Awake() {
		unit = GetComponent<Unit>();
	}

	public void Cast() {
		target.Clear();     //把目標先清空
		BeforeCast();

		DoCast();

		PlayCastAnimation();

		if(effectPrefab != null)
			PlayEffect();

		AfterCast();
		unit.AfterCast();
	}

	public void CalcDamage() {
		Property prop = unit.prop;
		damageInfo = new DamageInfo() {
			caster = unit,
			title = title,
			critMultiplier = prop.critMultiplier
		};

		if(type == SKILL_TYPE.ATTACK) {
			damageInfo.critRate = prop.weaponCrit * (1 + prop.globalCritRate);

			//最小傷 = 最小傷 * 技能傷害系數 * (1 + 增加物理傷害 + 增加傷害)
			int physicMinDamage = Mathf.RoundToInt(prop.physicMinDamage * damageRate * (1 + prop.increasePhysicDamage + prop.increaseDamage));
			int physicMaxDamage = Mathf.RoundToInt(prop.physicMaxDamage * damageRate * (1 + prop.increasePhysicDamage + prop.increaseDamage));

			//最後判斷是否有暴擊
			damageInfo.physicDamage = Mathf.RoundToInt(Random.Range(physicMinDamage, physicMaxDamage));
		} else {
			damageInfo.critRate = spellCrit * (1 + (prop.globalCritRate + prop.spellCritRate) / 100);

			//先算出基礎法傷
			int spellMinDamage = Mathf.RoundToInt(prop.spellMinDamage * damageRate * (1 + (prop.increaseSpellDamage + prop.increaseDamage) / 100));
			int spellMaxDamage = Mathf.RoundToInt(prop.spellMaxDamage * damageRate * (1 + (prop.increaseSpellDamage + prop.increaseDamage) / 100));
			int spellDamage = Mathf.RoundToInt(Random.Range(spellMinDamage, spellMaxDamage));
			damageInfo.physicDamage = Mathf.RoundToInt(spellDamage * convertToPhysic);
			damageInfo.fireDamage = Mathf.RoundToInt(spellDamage * convertToFire);
			damageInfo.iceDamage = Mathf.RoundToInt(spellDamage * convertToIce);
			damageInfo.thunderDamage = Mathf.RoundToInt(spellDamage * convertToThunder);
		}
		damageInfo.totalDamage = damageInfo.physicDamage + damageInfo.fireDamage + damageInfo.iceDamage + damageInfo.thunderDamage;
	}

	protected void TargetTakeDamage() {
		if(damageRate > 0) {
			damageInfo = new DamageInfo() {
				caster = unit,
				title = title
			};

			foreach(Unit t in target) {
				CalcDamage();
				AfterCalcDamage();
				t.TakeDamage(damageInfo);
			}
		}
	}

	protected void TargetApplyBuff(Unit caster, List<Buff> buffs) {
		foreach(Unit t in target) {
			if(!t.isDead)
				t.ApplyBuff(caster, buffs);
		}
	}

	virtual public void PlayCastAnimation() {
		unit.avatar.transform.DOKill(true);
		if(isMoveCharacterToTarget) {
			if(isJump)
				unit.avatar.transform.DOJump(target[0].transform.position, 1f, 1, 0.25f).SetLoops(2, LoopType.Yoyo);
			else
				unit.avatar.transform.DOMove(target[0].transform.position, 0.25f).SetLoops(2, LoopType.Yoyo);
		} else {
			unit.avatar.transform.DOJump(unit.avatar.transform.position, 0.5f, 1, 0.25f);
		}
	}

	protected void GetSelfTarget() {
		target.Add(unit);
	}

	virtual protected void BeforeCalcDamage() { }
	virtual protected void BeforeCast() { }
	virtual protected void AfterCalcDamage() { }
	virtual protected void AfterCast() { }
	virtual protected void PlayEffect() { }
	abstract protected void GetEnemyTarget();
	abstract protected void GetAllyTarget();
	abstract protected void DoCast();
}
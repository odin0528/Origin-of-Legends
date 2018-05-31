using UnityEngine;
using System.Collections;
using System;

class WindBlade : SingleSkill {
	protected override void AfterCast() {
		unit.powerAttckEffectCounter += usedCard[0].power;
	}

	protected override void PlayEffect() {
		GameObject effect = Instantiate<GameObject>(effectPrefab);
		effect.transform.position = transform.position;
		ProjectileEffect skillEffect = effect.GetComponent<ProjectileEffect>();
		skillEffect.responseList = damageInfo.responseList;
		skillEffect.endPos = target[0].transform.position;
	}

	protected override void DoCast() {
		GetEnemyTarget();
		TargetTakeDamage();
	}
}

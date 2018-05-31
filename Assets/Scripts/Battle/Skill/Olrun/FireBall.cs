using UnityEngine;
using System.Collections;
using System;

class FireBall : SingleSkill {

	bool isCharge = false;

	protected override void AfterCalcDamage() {
		if(unit.powerAttckEffectCounter >= 15) {
			damageInfo.Multiply(1.5f);
			unit.powerAttckEffectCounter -= 15;
			isCharge = true;
		}
	}

	protected override void PlayEffect() {
		GameObject effect = Instantiate<GameObject>(effectPrefab);
		effect.transform.position = transform.position;
		ProjectileEffect skillEffect = effect.GetComponent<ProjectileEffect>();
		skillEffect.responseList = damageInfo.responseList;
		skillEffect.endPos = target[0].transform.position;
		if(isCharge) {
			effect.transform.localScale = new Vector3(4.0f, 4.0f);
			isCharge = false;
		}
	}

	protected override void DoCast() {
		GetEnemyTarget();
		TargetTakeDamage();
	}
}

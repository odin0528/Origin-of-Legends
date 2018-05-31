using UnityEngine;
using System.Collections;
using System;

class WeaponAttack : SingleSkill {
	protected override void PlayEffect() {
		GameObject effect = Instantiate<GameObject>(effectPrefab);
		effect.transform.position = target[0].transform.position;
		OneShotEffect skillEffect = effect.GetComponent<OneShotEffect>();
		skillEffect.responseList = damageInfo.responseList;
	}

	protected override void DoCast() {
		GetEnemyTarget();
		TargetTakeDamage();
	}
}

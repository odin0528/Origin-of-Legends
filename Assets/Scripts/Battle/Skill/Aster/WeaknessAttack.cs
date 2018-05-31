using UnityEngine;
using System.Collections;

class WeaknessAttack: SingleSkill {

	protected override void AfterCast() {
		if(unit.powerAttckEffectCounter < 5)
			unit.powerAttckEffectCounter++;
	}

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

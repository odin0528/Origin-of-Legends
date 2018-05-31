using UnityEngine;
using System.Collections.Generic;

class Puncture: SingleSkill {
	protected override void PlayEffect() {
		foreach(Unit unit in target) {
			GameObject effect = Instantiate<GameObject>(effectPrefab);
			effect.transform.position = unit.transform.position;
			OneShotEffect skillEffect = effect.GetComponent<OneShotEffect>();
			skillEffect.responseList = damageInfo.responseList;
		}
	}

	protected override void DoCast() {
		GetEnemyTarget();

		//算好傷害等下丟給buff用
		CalcDamage();

		Bleeding bleeding = new Bleeding(unit) {
			dotDamage = damageInfo.physicDamage
		};

		TargetApplyBuff(unit, new List<Buff>() { bleeding });
	}
}

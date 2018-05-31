using UnityEngine;
using System.Collections.Generic;

class Arc:ChainSkill {
	void Start() {
		range = new int[3, 3] {
			{ 0, 1, 0 },
			{ 1, 0, 1 },
			{ 0, 1, 0 }
		};
	}

	protected override void PlayEffect() {
		GameObject effect = Instantiate<GameObject>(effectPrefab);
		effect.transform.position = transform.position;
		ArcEffect skillEffect = effect.GetComponent<ArcEffect>();
		skillEffect.responseList = damageInfo.responseList;
		skillEffect.chainLength = target.Count;
		for(int i = 0;i < target.Count;i++) {
			skillEffect.SetTarget(target[i].transform.position);
		}
	}

	protected override void DoCast() {
		GetEnemyTarget();
		TargetTakeDamage();

		if(unit.powerAttckEffectCounter >= 15) {
			Numb numb = new Numb(unit);
			TargetApplyBuff(unit, new List<Buff>() { numb });
			unit.powerAttckEffectCounter -= 15;
		}
	}
}

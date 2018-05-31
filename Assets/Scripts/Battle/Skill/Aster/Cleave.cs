using UnityEngine;
using System.Collections;

class Cleave: MultipleSkill {
	void Start() {
		range = new int[3, 3] { 
			{ 0, 1, 0 }, 
			{ 0, 1, 0 }, 
			{ 0, 1, 0 }
		};
	}

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
		TargetTakeDamage();
	}
}

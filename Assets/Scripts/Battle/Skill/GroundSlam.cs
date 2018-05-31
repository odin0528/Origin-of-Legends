using UnityEngine;
using System.Collections;

class GroundSlam: MultipleSkill {
	void Start() {
		range = new int[3, 3] { 
			{ 0, 0, 1 }, 
			{ 0, 1, 1 }, 
			{ 0, 0, 1 }
		};
	}
	protected override void PlayEffect() {
		foreach(DamageResponse response in damageInfo.responseList) {
			GameObject effect = Instantiate<GameObject>(effectPrefab);
			effect.transform.position = response.target.transform.position;
			OneShotEffect skillEffect = effect.GetComponent<OneShotEffect>();
			skillEffect.responseList.Add(response);
		}
	}

	protected override void DoCast() {
		GetEnemyTarget();
		TargetTakeDamage();
	}
}

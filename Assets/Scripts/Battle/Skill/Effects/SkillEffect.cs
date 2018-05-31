using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillEffect : MonoBehaviour {
	public List<DamageResponse> responseList = new List<DamageResponse>();
	public Unit[] target;

	virtual protected void ShowDamage() {}

	protected void Remove(float seconds = 0.0f) {
		if(seconds == 0.0f) {
			Delete();
		} else {
			Invoke("Delete", seconds);
		}
	}

	protected void Delete() {
		Destroy(gameObject);
		Destroy(this);
	}
}

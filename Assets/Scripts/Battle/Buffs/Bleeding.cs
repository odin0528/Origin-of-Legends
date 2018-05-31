using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleeding:Buff {
	public Bleeding(Unit unit):base(unit) {
		buffId = 3;
	}

	override public void BuffEffect() {

		DamageInfo damageInfo = new DamageInfo() {
			caster = caster,
			title = title,
			physicDamage = dotDamage
		};
		Debug.Log(dotDamage);
		owner.TakeDamage(damageInfo);
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class QuickPace: SingleSkill {
	protected override void DoCast() {
		GetSelfTarget();
		TargetApplyBuff(unit, new List<Buff>() { new Haste(unit) });
	}
}

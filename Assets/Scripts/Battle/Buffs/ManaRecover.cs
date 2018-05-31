using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRecover :Buff {
	public ManaRecover(Unit unit):base(unit) {
		buffId = 2;
	}

	override public void BuffEffect() {
		owner.powerAttckEffectCounter += (int) BattleManager.Instance.buffDataList.buffList[buffId].parameter[0];
	}
}
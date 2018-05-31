using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Haste : Buff {
	public Haste(Unit unit):base(unit) {
		buffId = 0;
	}

	override public void BuffBegin() {
		owner.BuffCalc += BuffCalc;
	}

	override public void BuffEnd() {
		owner.BuffCalc -= BuffCalc;
	}

	override public void BuffCalc(Property prop) {
		owner.prop.increasedAttackSpeed += BattleManager.Instance.buffDataList.buffList[buffId].parameter[0];
	}
}
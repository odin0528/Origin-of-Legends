using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numb : Buff {
	public Numb(Unit unit):base(unit) {
		buffId = 1;
	}

	override public void BuffBegin() {
		owner.BuffCalc += BuffCalc;
	}

	override public void BuffEnd() {
		owner.BuffCalc -= BuffCalc;
	}

	override public void BuffCalc(Property prop) {
		prop.isComa = true;
	}
}
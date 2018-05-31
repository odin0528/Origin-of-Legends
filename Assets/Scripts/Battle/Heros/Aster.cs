using UnityEngine;
using System.Collections;

public class Aster: Hero {
	public float aattackHaste = 0.2f;
	protected override void Init() {
		oriProp.increasedAttackSpeed += aattackHaste;
	}
}

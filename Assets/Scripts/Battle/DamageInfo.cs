using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageInfo {
	public string title;
	public Unit caster;

	public int physicDamage, fireDamage, iceDamage, thunderDamage, totalDamage;
	public float critRate, critMultiplier = 0f;
	public List<DamageResponse> responseList = new List<DamageResponse>();

	//總傷加乘
	public void Multiply(float radio) {
		physicDamage = Mathf.RoundToInt(physicDamage * radio);
		fireDamage = Mathf.RoundToInt(fireDamage * radio);
		iceDamage = Mathf.RoundToInt(iceDamage * radio);
		thunderDamage = Mathf.RoundToInt(thunderDamage * radio);
		totalDamage = physicDamage + fireDamage + iceDamage + thunderDamage;
	}
}

//受到傷害的結果
public class DamageResponse {
	public Unit target;
	public bool isDead = false,     //是否擊殺
				isCrit = false,		//是否造成暴擊
				isLowLife = false;  //是否造成瀕血
	public int takeDamage;          //最後造成的傷害
}

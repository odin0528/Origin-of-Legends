using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//一次性顯示的特效
public class OneShotEffect:SkillEffect {
	override protected void ShowDamage() {
		foreach(DamageResponse response in responseList) {
			GameObject damagePopup = (GameObject) Instantiate(BattleManager.Instance.damagePopup);
			damagePopup.transform.SetParent(GameObject.Find("Canvas").transform, false);
			damagePopup.transform.position = Camera.main.WorldToScreenPoint(response.target.transform.position);
			damagePopup.GetComponent<DamagePopup>().damage = response.takeDamage;
		}
	}
}

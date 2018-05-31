using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProjectileEffect: SkillEffect {
	public Vector3 endPos;
	public int damage;

	void Update() {
		if(transform.position != endPos)
			transform.position = Vector2.MoveTowards(transform.position, endPos, Time.deltaTime * 20);
		else {
			ShowDamage();
			Destroy(gameObject);
			Destroy(this);
		}
	}

	override protected void ShowDamage() {
		foreach(DamageResponse response in responseList) { 
			GameObject damagePopup = (GameObject) Instantiate(BattleManager.Instance.damagePopup);
			damagePopup.transform.SetParent(GameObject.Find("Canvas").transform, false);
			damagePopup.transform.position = Camera.main.WorldToScreenPoint(endPos);
			damagePopup.GetComponent<DamagePopup>().damage = response.takeDamage;
		}
	}
}

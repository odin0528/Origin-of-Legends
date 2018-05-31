using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

abstract public class Buff {
	public int buffId;
	public string title;
	public GameObject effectPrefab;

	public bool isDuration = true, isDot = false, isStackable = false;
	public float duration;
	public int triggerCount = 1;

	public Unit owner, caster;
	public int dotDamage;

	virtual public void BuffCalc(Property prop) { }
	virtual public void BuffBegin() { }
	virtual public void BuffEnd() { }
	virtual public void BuffEffect() { }

	public Buff(Unit unit) {
		SetCaster(unit);
	}

	public void Init() {
		PlayEffect();
		title = BattleManager.Instance.buffDataList.buffList[buffId].title;
		duration = BattleManager.Instance.buffDataList.buffList[buffId].duration;
		triggerCount = BattleManager.Instance.buffDataList.buffList[buffId].triggerCount;
		isStackable = BattleManager.Instance.buffDataList.buffList[buffId].isStackable;
		isDot = BattleManager.Instance.buffDataList.buffList[buffId].isDot;
		BattleManager.Instance.FrameUpdate += BuffConuter;
		BuffBegin();
	}

	public void BuffConuter() {
		duration -= Time.deltaTime;
		if(duration <= 0) {
			if(triggerCount > 0) {
				BuffEffect();
				duration = BattleManager.Instance.buffDataList.buffList[buffId].duration;
				triggerCount--;
			} else {
				Remove();
			}
		}
	}

	public void PlayEffect() {
		effectPrefab = Object.Instantiate(BattleManager.Instance.buffDataList.buffList[buffId].effectPrefab);
		effectPrefab.transform.position = owner.transform.position;
	}

	/*public void ShowDamage() {
		
			GameObject damagePopup = (GameObject) Object.Instantiate(BattleManager.Instance.damagePopup);
			damagePopup.transform.SetParent(GameObject.Find("Canvas").transform, false);
			damagePopup.transform.position = Camera.main.WorldToScreenPoint(response.target.transform.position);
			damagePopup.GetComponent<DamagePopup>().damage = response.takeDamage;
	}*/

	public void SetOwner(Unit unit) {
		owner = unit;
	}

	public void SetCaster(Unit unit) {
		caster = unit;
	}

	public void Restart() {
		duration = BattleManager.Instance.buffDataList.buffList[buffId].duration;
		triggerCount = BattleManager.Instance.buffDataList.buffList[buffId].triggerCount;

		if(!isDuration) {
			PlayEffect();
			BattleManager.Instance.FrameUpdate += BuffConuter;
		}
	}

	public void Remove() {
		BuffEnd();
		isDuration = false;
		BattleManager.Instance.FrameUpdate -= BuffConuter;
		Object.Destroy(effectPrefab);

		owner.ReCalcProperty();
	}

	public Buff Clone() {
		Buff newBuff = (Buff) this.MemberwiseClone();
		return newBuff;
	}
}
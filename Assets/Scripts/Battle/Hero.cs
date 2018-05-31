using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using DG.Tweening;

abstract public class Hero:Unit {
	public CHARACTER character;

	void Start() {
		Init();
		GetEquip();
		SetStatusBar();
	}

	public void CheckSkill() {
		CardInfoList pickupCardInfo = new CardInfoList(BattleManager.Instance.pickupCardInfo);
		CardInfoList _pickupCardInfo;
		CardInfoList restPickupCardInfo = new CardInfoList(pickupCardInfo);
		Skill prepareCastSkill = null;

		foreach(Skill skill in skills) {
			//如果需求數大於 選取的符文數，就跳過吧
			if(skill.needRune.Count > pickupCardInfo.Count)
				continue;

			//先清掉使用的牌
			skill.usedCard.Clear();

			bool isMatch = true;
			_pickupCardInfo = new CardInfoList(pickupCardInfo);
			for(int i=0; i < skill.needRune.Count; i++) {

				int index = _pickupCardInfo.FindIndexByRune(skill.needRune[i]);
				if(index > -1) {
					//有找到需要的符文就直接加到技能使用的卡牌中
					CardInfo newCard = new CardInfo() {
						power = pickupCardInfo[i].power,
						rune = pickupCardInfo[i].rune
					};
					skill.usedCard.Add(newCard);
					_pickupCardInfo.RemoveAt(index);     //若需要的符文包含在選擇的符文中，就先從LIST中刪掉
				} else {        //任何一個必需符文找不到 就中斷掉
					isMatch = false;
					break;
				}
			}

			if(isMatch) {
				//用需要符文的數量來決定要使用的技能，數量多的優先用
				if(prepareCastSkill == null || skill.needRune.Count > prepareCastSkill.needRune.Count) {
					prepareCastSkill = skill;

					//把剩下的卡牌存起來
					restPickupCardInfo = new CardInfoList(_pickupCardInfo);
				}
			}
		}

		if(prepareCastSkill != null) {
			prepareCastSkill.Cast();
		}

		//如果沒有拿去放技能的卡牌，全部放到強力攻擊的堆疊裡
		for(int i = 0;i < restPickupCardInfo.Count;i++) {
			powerAttackQueue.Enqueue(restPickupCardInfo[i]);
		}
	}

	void GetEquip() {
		foreach(KeyValuePair<CHARACTER_EQUIP_SORT, int> item in GM.heroList[character].equipment) {
			if(	item.Key == CHARACTER_EQUIP_SORT.WEAPON	)
				GM.equipList[item.Value].ApplyToHeroAsAttack(this);
			else
				GM.equipList[item.Value].ApplyToHero(this);
		}
		//_prop = prop.clone();
		ReCalcProperty();
	}

	override protected void SetStatusBar() {
		GameObject characterHpBar = Instantiate( Resources.Load<GameObject>("Prefabs/UI/Battlefield/HPBar"));
		characterHpBar.transform.SetParent(GameObject.Find("/Canvas/HeroPanel/" + sn.ToString()).transform, false);
		hpbar = characterHpBar.GetComponent<Slider>();
		hpText = characterHpBar.transform.Find("HpText").GetComponent<RectTransform>();
	}

	override public void DoDead() {
		BM.hero[sn].isDead = true;
		BM.FrameUpdate -= AttackConuter;

		if(BM.hero[0].isDead && BM.hero[1].isDead)
			BM.Gameover();
	}

	override protected void Remove() {
		avatar.transform.DOKill(true);
		avatar.DOKill(true);
		StopAllCoroutines();
		Destroy(hpbar.transform.parent.gameObject);
		Destroy(gameObject);
		Destroy(this);
	}
}
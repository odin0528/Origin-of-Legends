using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using DG.Tweening;
using LitJson;
[SerializeField]
public class Monster : Unit {
	public bool isMainTarget;
	public MONSTER_RACE race;
	public GameObject hpbarPrefabs;
	protected Tweener turnColor;

	void Start () {
		Init();
		SetStatusBar();
		AfterDead += BM.MonsterDead;
	}

	public void SetMonsterData() {

		MonsterDataList monsterDataList = BattleManager.Instance.monsterDataList;

		race = MONSTER_RACE.ANIMAL;
		title = BattleManager.Instance.monsterDataList.monsterList[0].title;
		attackSpeed = monsterDataList.monsterList[0].speed;
		attackTimer = 0.0f;
		oriProp.weaponSpeed = monsterDataList.monsterList[0].speed;
		oriProp.physicMinDamage = Mathf.RoundToInt(monsterDataList.monsterList[0].atk + (lv - 1) * monsterDataList.monsterList[0].atkRate);
		oriProp.physicMaxDamage = Mathf.RoundToInt(monsterDataList.monsterList[0].atk + (lv - 1) * monsterDataList.monsterList[0].atkRate);
		oriProp.weaponCrit = 5f;
		_hp = Mathf.RoundToInt(monsterDataList.monsterList[0].hp + (lv - 1) * monsterDataList.monsterList[0].hpRate);
		maxHp = _hp;

		//_prop = prop.clone();
		ReCalcProperty();
	}

	override protected void SetStatusBar() {
		GameObject hpbarObject = Instantiate(hpbarPrefabs, GameObject.Find("/Canvas/EnemyHpBar").transform) as GameObject;
		hpbarObject.transform.position = Camera.main.WorldToScreenPoint(transform.Find("HpBarPos").position);
		hpbar = hpbarObject.GetComponent<Slider>();
		hpText = hpbarObject.transform.Find("HpText").GetComponent<RectTransform>();
	}

	void TurnColor() {
		turnColor = avatar.DOColor(Color.yellow, 0.25f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear).OnComplete(() => turnColor.Restart(false));
	}

	public void CancelMainTarget() {
		isMainTarget = false;
		if(turnColor != null) {
			turnColor.Kill(true);
		}
	}

	void OnMouseDown() {
		BattleManager.Instance.OnChangeTarget(this);
		isMainTarget = true;
		TurnColor();
	}

	/*override public void attack(int index) {
		DamageInfo damageInfo = new DamageInfo();
		normalAttack.calcDamage(this, damageInfo, index);
	}*/

	override public void DoDead() {
		BM.FrameUpdate -= AttackConuter;
		BM.FrameUpdate -= TurnColor;
		//秀完傷害數字才消失，移除改去特效那邊處理
	}

	override protected void Remove() {
		avatar.transform.DOKill(true);
		avatar.DOKill(true);
		StopAllCoroutines();
		Destroy(hpbar.gameObject);
		Destroy(gameObject);
		Destroy(this);
	}

}

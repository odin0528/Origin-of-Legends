using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

abstract public class Unit:MonoBehaviour {
	protected GameManager GM;
	protected BattleManager BM;
	public int sn;
	public int x, y;

	public bool isDead = false;
	public bool isHpChange = false;
	public float hpTextHideTime = 0.5f;

	public int id;
	public int lv = 3, maxHp, def;
	public int _hp;
	public int hp {
		get {
			return _hp;
		}
		set {
			_hp = value;
			UpdateHpBar();
		}
	}
	public float hpPer;
	public float attackTimer, attackSpeed = 3.0f;

	public Property oriProp = new Property();	//算完裝備，跟角色天賦後的原始數值
	public Property prop = new Property();		//copy自oriProp 每次屬性有變化時跑ReCalcProperty 重新計算
	//protected Property _prop;

	public Queue<CardInfo> powerAttackQueue = new Queue<CardInfo>();
	public int _powerAttckEffectCounter = 0;
	virtual public int powerAttckEffectCounter {
		get {
			return _powerAttckEffectCounter;
		}
		set {
			_powerAttckEffectCounter = value;
		}
	}
	public Skill normalAttack;
	public Skill powerAttack;
	public Skill[] skills;

	public List<Buff> buffList = new List<Buff>();

	public string title;

	protected Slider hpbar;
	protected RectTransform hpText;
	public SpriteRenderer avatar;
	protected Tweener takeDamageTurnColor;

	public event BattleManager.calculateDelegate EquipCalc, BuffCalc;
	public event BattleManager.attackDelegate BeforeAttack;
	public event BattleManager.actionDelegate BeforeDead, AfterDead;

	virtual protected void Init() { }
	virtual public void DoDead() { }
	virtual public void AfterCast() { }
	abstract protected void SetStatusBar();
	abstract protected void Remove();

	void Awake() {
		GM = GameManager.Instance;
		BM = BattleManager.Instance;

		avatar = transform.Find("Avatar").GetComponent<SpriteRenderer>();

		//判斷平台
		Input.multiTouchEnabled = false;
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		//input = MobileInput;
#else
		//input = DesktopInput;
#endif
	}

	void Update() {
		
	}

	public void SetSn(int index) {	sn = index; }

	public void AttackConuter() {
		if(!prop.isComa) {
			attackTimer += Time.deltaTime;
			if(attackTimer >= attackSpeed) {
				attackTimer = 0.0f;
				Attack();
			}
		}
	}

	public void TakeDamage(DamageInfo damageInfo) {
		DamageResponse response = new DamageResponse() { target = this};
		int currentHp = hp;

		if(Random.Range(0f, 100f) <= damageInfo.critRate)
			response.isCrit = true;

		if(response.isCrit) {
			damageInfo.physicDamage *= (int) (2.0f + damageInfo.critMultiplier);
			damageInfo.fireDamage *= (int) (2.0f + damageInfo.critMultiplier);
			damageInfo.iceDamage *= (int) (2.0f + damageInfo.critMultiplier);
			damageInfo.thunderDamage *= (int) (2.0f + damageInfo.critMultiplier);
		}
		damageInfo.totalDamage = damageInfo.physicDamage + damageInfo.fireDamage + damageInfo.iceDamage + damageInfo.thunderDamage;

		hp -= damageInfo.totalDamage;
		if((float) hp / maxHp < 0.3f) {
			prop.isLowLife = true;
			response.isLowLife = true;
		}
#if UNITY_EDITOR
		string output = damageInfo.caster.title + " 對 " + title + " 造成了 ";
		if(damageInfo.physicDamage > 0)
			output += string.Format(" {0} ", damageInfo.physicDamage);
		if(damageInfo.fireDamage > 0)
			output += string.Format(" <color=red>{0}</color> ", damageInfo.fireDamage);
		if(damageInfo.iceDamage > 0)
			output += string.Format(" <color=blue>{0}</color> ", damageInfo.iceDamage);
		if(damageInfo.thunderDamage > 0)
			output += string.Format(" <color=yellow>{0}</color> ", damageInfo.thunderDamage);

		if(response.isCrit)
			output = "<b><color=red>" + output + "</color></b>";
		//Debug.Log(output);
#endif

		response.takeDamage = currentHp - hp;
		if(hp <= 0) {
			hp = 0;
			Dead();
			response.isDead = true;
		} else {
			TakeDamageTurnColor();      //受傷的閃爍特效
		}
		damageInfo.responseList.Add(response);
	}

	public void ApplyBuff(Unit caster, List<Buff> buffs) {
		for(int i = 0;i < buffs.Count;i++) {
			int isExist = -1;
			for(int j=0; j<buffList.Count;j++) {
				if(buffs[i].GetType() == buffList[j].GetType()) {
					isExist = j;
					break;
				}
			}

			if(isExist == -1) {
				Buff buff = (Buff) buffs[i].Clone();

				buff.SetOwner(this);
				buff.Init();
				buffList.Add(buff);
			} else {
				if(buffs[i].isStackable && buffs[i].isDot)
					buffList[isExist].dotDamage += buffs[i].dotDamage;
				buffList[isExist].Restart();
			}
		}

		ReCalcProperty();
	}

	public void RemoveAllBuff() {
		for(int i = 0;i < buffList.Count;i++) {
			buffList[i].Remove();
		}

		if(!isDead)
			ReCalcProperty();
	}

	virtual public void UpdateHpBar() {
		isHpChange = true;
		StartCoroutine("HpChanging");
	}

	protected void UpdateHpText() {
		int ten = (int) hpbar.value / 10;
		int digit = (int) hpbar.value % 10;

		hpText.position = new Vector3(hpbar.transform.Find("Handle Slide Area/Handle").position.x + 5.0f, hpText.position.y);

		if(ten == 10) {
			hpbar.transform.Find("HpText/0").gameObject.GetComponent<Image>().gameObject.SetActive(true);
			hpbar.transform.Find("HpText/1").gameObject.GetComponent<Image>().gameObject.SetActive(true);
			hpbar.transform.Find("HpText/0").gameObject.GetComponent<Image>().sprite = BattleManager.Instance.hpText[1];
			hpbar.transform.Find("HpText/1").gameObject.GetComponent<Image>().sprite = BattleManager.Instance.hpText[0];
			hpbar.transform.Find("HpText/2").gameObject.GetComponent<Image>().sprite = BattleManager.Instance.hpText[0];
		} else if(ten > 0) {
			hpbar.transform.Find("HpText/0").gameObject.GetComponent<Image>().gameObject.SetActive(false);
			hpbar.transform.Find("HpText/1").gameObject.GetComponent<Image>().gameObject.SetActive(true);
			hpbar.transform.Find("HpText/1").gameObject.GetComponent<Image>().sprite = BattleManager.Instance.hpText[ten];
			hpbar.transform.Find("HpText/2").gameObject.GetComponent<Image>().sprite = BattleManager.Instance.hpText[digit];
		} else {
			hpbar.transform.Find("HpText/0").gameObject.GetComponent<Image>().gameObject.SetActive(false);
			hpbar.transform.Find("HpText/1").gameObject.GetComponent<Image>().gameObject.SetActive(false);
			hpbar.transform.Find("HpText/2").gameObject.GetComponent<Image>().sprite = BattleManager.Instance.hpText[digit];
		}
	}

	IEnumerator HpChanging() {
		StopCoroutine("HideHpText");
		hpbar.gameObject.SetActive(true);
		while(isHpChange) {
			int hpPercent = (int) Mathf.Ceil(((float) hp / (float)maxHp) * 100.0f);
			if(hpbar.value > hpPercent) {
				hpbar.value--;
			} else if(hpbar.value < hpPercent) {
				hpbar.value++;
			} else {
				isHpChange = false;
				StartCoroutine("HideHpText");
			}
			UpdateHpText();
			yield return null;
		}
	}

	IEnumerator HideHpText() {
		yield return new WaitForSeconds(hpTextHideTime);
		hpbar.gameObject.SetActive(false);
	}

	void TakeDamageTurnColor() {
		if(takeDamageTurnColor != null)
			takeDamageTurnColor.Kill(true);
		takeDamageTurnColor = avatar.DOColor(Color.red, 0.25f).SetLoops(4, LoopType.Yoyo).SetEase(Ease.Linear);
	}

	/*public Property GetProperty() {
		Property returnProp = prop.Clone();   //還原數值

		if(this is Hero) {
			//英雄的話計算傷害前跑一下裝備的各種效果 buff equip之類的
			foreach(KeyValuePair<string, int> item in GM.heroList[BM.heroId].equipment) {
				GM.equipList[item.Value].preCalc(returnProp, sort, item.Key);
			}
		}

		//計算傷害前跑一下各種效果 buff 之類的
		if(BeforeCalc != null)
			BeforeCalc();

		return returnProp;
	}*/

	public void ReCalcProperty() {
		prop = oriProp.Clone();
		if(EquipCalc != null)
			EquipCalc(prop);
		if(BuffCalc != null)
			BuffCalc(prop);

		SetAttackSpeed();
	}

	public void SetAttackSpeed() {
		//換算目前攻速計時器的百分比，換算給新的攻速
		float radio = attackTimer / attackSpeed;
		attackSpeed = prop.weaponSpeed / (1 + prop.increasedWeaponSpeed) / (1 + prop.increasedAttackSpeed);
		attackTimer = attackSpeed * radio;
	}

	virtual public void Attack() {
		//交給技能算傷害
		if(powerAttack != null && powerAttackQueue.Count > 0) {
			CardInfo info = powerAttackQueue.Dequeue();
			powerAttack.usedCard.Clear();
			powerAttack.usedCard.Add(info);
			powerAttack.Cast();
		} else {
			normalAttack.Cast();
		}
	}

	virtual public void Dead() {
		isDead = true;
		avatar.DOKill(true);
		avatar.DOColor(Color.clear, 1.0f).OnComplete(() => Remove());

		RemoveAllBuff();

		DoDead();
		/*if(beforeDead != null)
			beforeDead();*/

		if(AfterDead != null)
			AfterDead();
	}
}

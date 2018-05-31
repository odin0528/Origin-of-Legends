using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using LitJson;

public class BattleManager : MonoBehaviour {
	public static BattleManager Instance;
	public GameManager GM;
	public Dictionary<int, int> heroId = new Dictionary<int, int>();
	public Hero[] hero = new Hero[2];
	public bool isOver = false, isPause = false, isDrawCardTimerRunning = false;
	public float battleSpeed = 1.0f, drawCardCounter = 0.0f;
	public int monsterCounter = 0, timerCount = 180;
	private System.Action runningMethod;
	public Monster[,] map = new Monster[3,3];
	private int[] getTargetOrder = new int[9] {4,7,1,5,8,2,6,9,3};
	private Monster _mainTarget;
	public Monster mainTarget {
		get {
			if(_mainTarget == null || _mainTarget.isDead)
				_mainTarget = GetTargetByOrder();
			return _mainTarget;
		}
		set {
			_mainTarget = value;
		}
	}

	public delegate void _frameUpdate();
	public event _frameUpdate FrameUpdate;
	//sort : 使用技能時使用的部位  type：這件裝備的部位
	public delegate void calculateDelegate(Property prop);
	public delegate void attackDelegate(Unit unit, DamageInfo damageInfo);
	public delegate void actionDelegate();

	public Sprite[] hpText = new Sprite[10];
	public Sprite[] timerText = new Sprite[10];
	public Image[] timer = new Image[4];

	//卡牌相關
	public GameObject cardPrefab;
	public GameObject cardPanel;
	public List<Card> deck = new List<Card>();
	public List<CardInfo> pickupCardInfo = new List<CardInfo>();

	//抽卡計時器
	public Image drawCardTimerImage;

	//傷害顯示元件
	public GameObject damagePopup;

	//scriptable object
	public MonsterDataList monsterDataList;
	public BuffDataList buffDataList;

	void Awake () {
		Instance = this;
		GM = GameManager.Instance;
	}

	void Start() {
		int index = 0;
		GameObject charPrefab;

		/*var request = UnityWebRequest.GetAssetBundle("file://" + Application.dataPath + "/AssetBundles/hero");
		AssetBundle myLoadedAssetBundle = DownloadHandlerAssetBundle.GetContent(request);
		//AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/hero");
		if(myLoadedAssetBundle == null) {
			Debug.Log("Failed to load AssetBundle!");
			return;
		}
		charPrefab = myLoadedAssetBundle.LoadAsset<GameObject>(CharacterConfig.prefabPath[item.Key]);*/


		//設定英雄
		foreach(KeyValuePair<CHARACTER, HeroInfo> item in GM.heroList) {
			charPrefab = Resources.Load<GameObject>("Prefabs/Heros/" + CharacterConfig.prefabPath[item.Key]);

			GameObject newHero = Instantiate(
				charPrefab,
				GameObject.Find("/Heros/" + index.ToString()).transform,
				false
			) as GameObject;

			hero[index] = newHero.GetComponent<Hero>();
			hero[index].SetSn(index);
			FrameUpdate += hero[index].AttackConuter;
			index++;
		}

		//載入怪物
		string jsonString = File.ReadAllText(Application.dataPath + "/data/MonsterTest.json");
		JsonData jsonData = JsonMapper.ToObject(jsonString);
		for(int i = 0;i < jsonData["monsterInfo"].Count;i++) {
			//print(monsterDataList.monsterList[(int) jsonData["monsterInfo"][i]["monsterId"] - 1].title);

			int x = ((int) jsonData["monsterInfo"][i]["pos"] - 1) % 3;
			int y = ((int) jsonData["monsterInfo"][i]["pos"] - 1) / 3;
			GameObject newMonster = Instantiate(
				monsterDataList.monsterList[(int) jsonData["monsterInfo"][i]["monsterId"] - 1].prefab,
				GameObject.Find("/Enemy/" + jsonData["monsterInfo"][i]["pos"]).transform,
				false
			) as GameObject;
			map[x, y] = newMonster.GetComponent<Monster>();
			map[x, y].SetMonsterData();
			map[x, y].x = x;
			map[x, y].y = y;
			FrameUpdate += map[x, y].AttackConuter;
		}
		monsterCounter = jsonData["monsterInfo"].Count;

		SetBeginDeck();
		StartDrawCardTimer();
		StartCoroutine("Countdown");

		//myLoadedAssetBundle.Unload(false);
	}
	
	void Update () {
		FrameUpdate();
	}

	void Nothing() { }

	IEnumerator Countdown() {
		while(!isOver) {
			yield return new WaitForSeconds(1.0f);
			timerCount--;
			UpdateTimer();
			if(timerCount == 0)
				Gameover();
		}
	}

	public void UpdateTimer() {
		timer[0].sprite = timerText[timerCount / 600];
		timer[1].sprite = timerText[timerCount / 60];
		timer[2].sprite = timerText[timerCount % 60 / 10];
		timer[3].sprite = timerText[timerCount % 60 % 10];
	}

	public void OnChangeTarget(Monster monster) {
		mainTarget.CancelMainTarget();
		mainTarget = monster;
	}

	public void MonsterDead() {
		if(--monsterCounter == 0)
			Gameover();
	}

	public void Gameover() {
		Debug.Log("game over");
		FrameUpdate = Nothing;
		isOver = true;
		StopAllCoroutines();
	}

	public Monster GetTargetByOrder() {
		foreach(int target in getTargetOrder) {
			GameObject unit = GameObject.Find("/Enemy/" + target.ToString());
			if(unit.transform.childCount > 0 && !unit.transform.GetChild(0).GetComponent<Monster>().isDead) {
				return unit.transform.GetChild(0).GetComponent<Monster>();
			}
		}
		return null;
	}

	public Unit GetRandomHero() {
		int targetSn = -1;
		if(!BattleManager.Instance.hero[0].isDead && !BattleManager.Instance.hero[1].isDead)
			targetSn = Random.Range(0, 2);
		else if(BattleManager.Instance.hero[1] != null && !BattleManager.Instance.hero[1].isDead)
			targetSn = 1;
		else if(BattleManager.Instance.hero[0] != null && !BattleManager.Instance.hero[0].isDead)
			targetSn = 0;

		if(targetSn >= 0)
			return BattleManager.Instance.hero[targetSn];
		else
			return null;
	}

	public List<Unit> GetRandomEnemy(int number =1, Unit exceptUnit = null) {
		List<Unit> returnMonsterList = new List<Unit>();
		List<Unit> monsterList = new List<Unit>();
		for(int y =0; y < 3; y++) {
			for(int x = 0;x < 3;x++) {
				if(map[x, y] != null && !map[x, y].isDead) {
					if(exceptUnit == null || exceptUnit.x != x || exceptUnit.y != y) {
						monsterList.Add(map[x, y]);
					}
				}
			}
		}

		for(int i=number; i >0; i--) {
			int index = Random.Range(0, monsterList.Count);

			returnMonsterList.Add(monsterList[index]);
			monsterList.RemoveAt(index);
		}

		return returnMonsterList;
	}

	public List<Unit> GetLiveHero() {
		List<Unit> targetList = new List<Unit>();
		if(BattleManager.Instance.hero[0] != null && !BattleManager.Instance.hero[0].isDead) {
			targetList.Add(BattleManager.Instance.hero[1]);
		}
		if(BattleManager.Instance.hero[1] != null && !BattleManager.Instance.hero[1].isDead) {
			targetList.Add(BattleManager.Instance.hero[1]);
		}
		return targetList;
	}


	//卡牌相關函式
	void StartDrawCardTimer() {
		if(!isDrawCardTimerRunning) {
			FrameUpdate += UpdateDrawCardTimer;
			isDrawCardTimerRunning = true;
		}
	}

	void StopDrawCardTimer() {
		if(isDrawCardTimerRunning) {
			FrameUpdate -= UpdateDrawCardTimer;
			isDrawCardTimerRunning = false;
		}
	}

	void UpdateDrawCardTimer() {
		drawCardCounter += Time.deltaTime;
		if(drawCardCounter >= 3.0f) {
			DrawCard();
			drawCardCounter = 0.0f;
		}
		drawCardTimerImage.fillAmount = drawCardCounter / 3.0f;
	}

	public void SetBeginDeck() {
		/*int startCardAmount = 6;
		for(int i = 0; i < startCardAmount; i++) {
			DrawCard();
		}*/
		DrawCard(1, "C");
		DrawCard(1, "M");
		DrawCard(1, "C");
		DrawCard(1, "M");
		DrawCard(1, "C");
		DrawCard(1, "M");
	}

	public void DrawCard() {
		GameObject newCard = Instantiate<GameObject>(cardPrefab);
		newCard.transform.SetParent(cardPanel.transform, false);
		Card card = newCard.GetComponent<Card>();
		card.CreateRandom();
		deck.Add(card);

		if(deck.Count == 7) {
			StopDrawCardTimer();
		}
	}

	public void DrawCard(int power = -1, string rune = null) {
		if(deck.Count < 7) {
			GameObject newCard = Instantiate<GameObject>(cardPrefab);
			newCard.transform.SetParent(cardPanel.transform, false);
			Card card = newCard.GetComponent<Card>();
			card.Create(power, rune);
			deck.Add(card);
		}

		if(deck.Count == 7) {
			StopDrawCardTimer();
		}
	}

	public void PlayCard() {
		Stack<int> pickupCardIndex = new Stack<int>();
		for(int i = 0; i < deck.Count; i++) {
			if(deck[i].isPickup)
				pickupCardIndex.Push(i);
		}

		while(pickupCardIndex.Count > 0) {
			int index = pickupCardIndex.Pop();
			pickupCardInfo.Add(deck[index].info);
			deck[index].Play();
			deck.RemoveAt(index);
		}

		if(hero[0] != null && !hero[0].isDead)
			hero[0].CheckSkill();
		if(hero[1] != null && !hero[1].isDead)
			hero[1].CheckSkill();

		pickupCardInfo.Clear();
		StartDrawCardTimer();
	}
	
	public List<Skill> ShuffleList(List<Skill> inputList) {
		List<Skill> randomList = new List<Skill>();

		int randomIndex = 0;
		while(inputList.Count > 0) {
			randomIndex = Random.Range(0, inputList.Count);
			randomList.Add(inputList[randomIndex]);
			inputList.RemoveAt(randomIndex);
		}

		return randomList;
	}
}

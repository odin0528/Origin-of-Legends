using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
	public CardInfo info = new CardInfo();
	public bool isDragging = false;
	public bool isPickup = false;
	public bool isAnimation = false;
	public Image runeSprite;
	public Image powerSprite;
	public Image cardBG;
	public Sprite originBG;
	public Sprite pickupBG;
	public Sprite[] runeImage;
	public Sprite[] powerImage;
	private Vector2 pos;

	void Update() {
		if(isAnimation) {
			if(isPickup) {
				gameObject.GetComponent<RectTransform>().localScale = gameObject.GetComponent<RectTransform>().localScale + new Vector3(3.0f * Time.deltaTime, 3.0f * Time.deltaTime);
				if(gameObject.GetComponent<RectTransform>().localScale.x >= 1.3f) {
					gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.3f, 1.3f);
					isAnimation = false;
				}
			} else {
				gameObject.GetComponent<RectTransform>().localScale = gameObject.GetComponent<RectTransform>().localScale - new Vector3(3.0f * Time.deltaTime, 3.0f * Time.deltaTime);
				if(gameObject.GetComponent<RectTransform>().localScale.x <= 1.0f) {
					gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f);
					isAnimation = false;
				}
			}
		}
	}

	public void CreateRandom() {
		info.power = Random.Range(0, 6);
		info.rune = Random.Range(0, System.Enum.GetValues(typeof(RUNE_LIST)).Length);
		Render();
	}

	public void Create(int power = -1, string rune = null) {
		info.power = (power == -1) ? Random.Range(0, 6) : power;
		info.rune = (rune == null) ? Random.Range(0, System.Enum.GetValues(typeof(RUNE_LIST)).Length) : (int) System.Enum.Parse(typeof(RUNE_LIST), rune);
		Render();
	}

	public void Render() {
		runeSprite.sprite = runeImage[info.rune];
		powerSprite.sprite = powerImage[info.power];
	}

	public void Play() {
		GetComponent<Animation>().Play();
		GetComponent<Image>().raycastTarget = false;
	}

	public void Remove() {
		Destroy(gameObject);
	}

	//UI 事件相關
	public void WhenClick() {
		if(!isPickup) {
			cardBG.sprite = pickupBG;
			isPickup = true;
			isAnimation = true;
		} else {
			cardBG.sprite = originBG;
			isPickup = false;
			isAnimation = true;
		}
	}

	public void WhenDragEnd() {
		isDragging = false;
	}

	public void WhenDragStart() {
		isDragging = true;
		pos = Input.mousePosition;
	}

	public void WhenDrag() {
		float distance = Mathf.Abs( Input.mousePosition.y - pos.y);
		if(isDragging && distance > 100) {
			WhenDragEnd();
			BattleManager.Instance.PlayCard();
		}
	}
}

public class CardInfo{
	public int power;
	public int rune;
}

public class CardInfoList:List<CardInfo> {

	public CardInfoList(List<CardInfo> cardInfoList) {
		for(int i = 0;i < cardInfoList.Count;i++) {
			CardInfo cardInfo = new CardInfo();
			cardInfo.rune = cardInfoList[i].rune;
			cardInfo.power = cardInfoList[i].power;
			this.Add(cardInfo);
		}
	}

	public CardInfoList(CardInfoList cardInfoList) {
		for(int i = 0;i < cardInfoList.Count;i++) {
			CardInfo cardInfo = new CardInfo();
			cardInfo.rune = cardInfoList[i].rune;
			cardInfo.power = cardInfoList[i].power;
			this.Add(cardInfo);
		}
	}

	public int FindIndexByRune(int rune) {
		for(int i=0; i < this.Count; i++) {
			if(this[i].rune == rune)
				return i;
		}

		return -1;
	}

	public int FindIndexByRune(string rune) {
		return FindIndexByRune((int) System.Enum.Parse(typeof(RUNE_LIST), rune));
	}
}
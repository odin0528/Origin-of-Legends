using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour {
	public static CraftManager Instance;
	private GameManager GM;
	public GameObject equipListUI, equipUI;
	public RectTransform scrollView;
	public Scrollbar scrollBar;

	private Image equipSkin;
	public Text equipTitle;
	public Text modDesc;
	private Text equipDesc;

	void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		GM = GameManager.Instance;
		GM.pickMode = PickMode.CRAFT;

		equipSkin = equipUI.transform.Find("Skin").GetComponent<Image>();
		equipDesc = equipUI.transform.Find("EquipDesc").GetComponent<Text>();

		loadEquip();
		pick(GM.equipList.First().Key);
	}

	public void pick(int equipDbId) {
		equipSkin.sprite = Resources.Load<Sprite>(GM.equipList[equipDbId].skin + "_b");
		equipTitle.text = GM.equipList[equipDbId].GetTitle();
		equipDesc.text = GM.equipList[equipDbId].GetDesc();
		modDesc.text = GM.equipList[equipDbId].GetModDesc();
	}

	void loadEquip() {
		GameObject ItemColumn = Resources.Load<GameObject>("Prefabs/UI/ItemColumn");
		foreach(Equip equip in GM.equipList.Values) {
			GameObject item = Instantiate(ItemColumn);
			item.transform.Find("Skin").GetComponent<Image>().sprite = Resources.Load<Sprite>(equip.skin);
			item.GetComponent<Image>().color = Config.rarityColor[equip.rarity];
			item.transform.SetParent(equipListUI.transform);

			//設定道具屬性
			ItemRemote itemRemote = item.GetComponent<ItemRemote>();
			itemRemote.scrollView = scrollView;
			itemRemote.scrollBar = scrollBar;
			itemRemote.dbId = equip.dbId;
			itemRemote.itemType = ItemType.EQUIP;
		}
	}
}

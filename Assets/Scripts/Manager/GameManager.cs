using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class GameManager:MonoBehaviour {
	public static GameManager Instance;
	public Dictionary<CHARACTER, HeroInfo> heroList = new Dictionary<CHARACTER, HeroInfo>();
	public Dictionary<int, Equip> equipList = new Dictionary<int, Equip>();
	public Dictionary<int, Item> itemList = new Dictionary<int, Item>();
	public PickMode pickMode;

	void Awake() {
		Instance = this;
	}

	void Start() {
		string jsonString;
		JsonData jsonData;

		EquipType.LoadEquipData();
		MonsterType.LoadMonsterData();

		//載入所有裝備
		jsonString = File.ReadAllText(Application.dataPath + "/data/equipTest.json");
		jsonData = JsonMapper.ToObject(jsonString);
		for(int i = 0;i < jsonData["equipInfo"].Count;i++) {
			Equip equip;
			IDictionary tdictionary = jsonData["equipInfo"][i] as IDictionary;
			if(tdictionary.Contains("legendId")) {
				equip = (Equip) System.Activator.CreateInstance(LegendaryEquip.list[(int) jsonData["equipInfo"][i]["legendId"]]);
			} else {
				equip = new Equip();
			}
			JsonUtility.FromJsonOverwrite(jsonData["equipInfo"][i].ToJson(), equip);
			equip.Init();

			//套用詞綴效果到裝備
			if(tdictionary.Contains("mod")) {
				for(int j = 0;j < jsonData["equipInfo"][i]["mod"].Count;j++) {
					MOD_TYPE type = (MOD_TYPE) Enum.Parse(typeof(MOD_TYPE), jsonData["equipInfo"][i]["mod"][j]["affix"].ToString());
					Mod mod = (Mod) System.Activator.CreateInstance(EquipMod.list[type]);
					JsonUtility.FromJsonOverwrite(jsonData["equipInfo"][i]["mod"][j].ToJson(), mod);
					equip.ApplyMod(mod);
				}
			}

			equip.Calculate();
			equipList.Add(Int32.Parse(jsonData["equipInfo"][i]["dbId"].ToString()), equip);
		}

		//載入所有英雄
		jsonString = File.ReadAllText(Application.dataPath + "/data/heroTest.json");
		jsonData = JsonMapper.ToObject(jsonString);
		for(int i = 0;i < jsonData["heroInfo"].Count;i++) {
			HeroInfo heroInfo = new HeroInfo();
			JsonUtility.FromJsonOverwrite(jsonData["heroInfo"][i].ToJson(), heroInfo);
			heroInfo.character = (CHARACTER) Enum.Parse(typeof(CHARACTER), jsonData["heroInfo"][i]["type"].ToString());
			Dictionary<string, int> equipment = JsonMapper.ToObject<Dictionary<string, int>>(jsonData["heroInfo"][i]["equipment"].ToJson());
			//List<string> skills = JsonMapper.ToObject<List<string>>(jsonData["heroInfo"][i]["skills"].ToJson());

			//設定英雄裝備
			foreach(KeyValuePair<string, int> item in equipment) {
				heroInfo.equipment.Add(
					(CHARACTER_EQUIP_SORT) Enum.Parse(typeof(CHARACTER_EQUIP_SORT), item.Key.ToUpper()),
					item.Value
				);
			}

			heroList.Add(heroInfo.character, heroInfo);
		}
	}

	/*IEnumerator getDataFromServer() {
		string bundleUrl = "file://C:/projects/game/unity/Origin of Legends/Assets/AssetBundles/gamedata";
		WWW www = new WWW(bundleUrl);
		//string bundleUrl = "http://127.0.0.1/test/AssetBundles/test";
		//WWW www = WWW.LoadFromCacheOrDownload(bundleUrl, 3);
		print("pre return www");
		yield return new WaitForSeconds(2.0f);
		print("return www");
		AssetBundle bundle = www.assetBundle;
		string jsonString = bundle.LoadAsset<Object>("EquipData").ToString();
		JsonData jsonData = JsonMapper.ToObject(jsonString);
	}*/
}
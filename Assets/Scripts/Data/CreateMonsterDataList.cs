using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateMonsterDataList {
	[MenuItem("Assets/Create/Monster Data List")]
	public static MonsterDataList Create() {
		MonsterDataList asset = ScriptableObject.CreateInstance<MonsterDataList>();

		AssetDatabase.CreateAsset(asset, "Assets/MonsterDataList.asset");
		AssetDatabase.SaveAssets();
		return asset;
	}
}
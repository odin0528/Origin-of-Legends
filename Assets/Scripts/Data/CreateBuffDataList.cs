using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateBuffDataList {
	[MenuItem("Assets/Create/Buff Data List")]
	public static BuffDataList Create() {
		BuffDataList asset = ScriptableObject.CreateInstance<BuffDataList>();

		AssetDatabase.CreateAsset(asset, "Assets/BuffDataList.asset");
		AssetDatabase.SaveAssets();
		return asset;
	}
}
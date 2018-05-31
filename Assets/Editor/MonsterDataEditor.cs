using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MonstarDataEditor:EditorWindow {

	public MonsterDataList monsterDataList;
	private int viewIndex = 1;

	[MenuItem("Window/Monstar Data Editor %#e")]
	static void Init() {
		EditorWindow.GetWindow(typeof(MonstarDataEditor));
	}

	void OnEnable() {
		if(EditorPrefs.HasKey("ObjectPath")) {
			string objectPath = EditorPrefs.GetString("ObjectPath");
			monsterDataList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(MonsterDataList)) as MonsterDataList;
		}

	}

	void OnGUI() {
		GUILayout.BeginHorizontal();
		GUILayout.Label("Monster Data Editor", EditorStyles.boldLabel);
		if(monsterDataList != null) {
			if(GUILayout.Button("Show Monster List")) {
				//EditorUtility.FocusProjectWindow();
				Selection.activeObject = monsterDataList;
			}
		}
		if(GUILayout.Button("Open Monster List")) {
			OpenMonsterList();
		}
		if(GUILayout.Button("New Monster List")) {
			//EditorUtility.FocusProjectWindow();
			Selection.activeObject = monsterDataList;
		}
		GUILayout.EndHorizontal();

		if(monsterDataList == null) {
			GUILayout.BeginHorizontal();
			GUILayout.Space(10);
			if(GUILayout.Button("Create New Monster List", GUILayout.ExpandWidth(false))) {
				CreateNewMonsterList();
			}
			if(GUILayout.Button("Open Existing Monster List", GUILayout.ExpandWidth(false))) {
				OpenMonsterList();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.Space(20);

		if(monsterDataList != null) {
			GUILayout.BeginHorizontal();

			GUILayout.Space(10);

			if(GUILayout.Button("Prev", GUILayout.ExpandWidth(false))) {
				if(viewIndex > 1)
					viewIndex--;
			}
			GUILayout.Space(5);
			if(GUILayout.Button("Next", GUILayout.ExpandWidth(false))) {
				if(viewIndex < monsterDataList.monsterList.Count) {
					viewIndex++;
				}
			}

			GUILayout.Space(60);

			if(GUILayout.Button("Add Monster", GUILayout.ExpandWidth(false))) {
				AddMonster();
			}
			if(GUILayout.Button("Delete Monster", GUILayout.ExpandWidth(false))) {
				DeleteMonster(viewIndex - 1);
			}

			GUILayout.EndHorizontal();
			if(monsterDataList.monsterList == null)
				Debug.Log("wtf");
			if(monsterDataList.monsterList.Count > 0) {
				GUILayout.BeginHorizontal();
				viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Monster", viewIndex, GUILayout.ExpandWidth(false)), 1, monsterDataList.monsterList.Count);
				//Mathf.Clamp (viewIndex, 1, monsterDataList.monsterList.Count);
				EditorGUILayout.LabelField("of   " + monsterDataList.monsterList.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
				GUILayout.EndHorizontal();

				monsterDataList.monsterList[viewIndex - 1].title = EditorGUILayout.TextField("Monster Title", monsterDataList.monsterList[viewIndex - 1].title as string);
				monsterDataList.monsterList[viewIndex - 1].hp = EditorGUILayout.IntSlider("Monster Hp", monsterDataList.monsterList[viewIndex - 1].hp, 0, 100);
				monsterDataList.monsterList[viewIndex - 1].hpRate = EditorGUILayout.FloatField("Monster Hp Rate", monsterDataList.monsterList[viewIndex - 1].hpRate);

				/*GUILayout.Space(10);

				GUILayout.BeginHorizontal();
				monsterDataList.monsterList[viewIndex - 1].isUnique = (bool) EditorGUILayout.Toggle("Unique", monsterDataList.monsterList[viewIndex - 1].isUnique, GUILayout.ExpandWidth(false));
				monsterDataList.monsterList[viewIndex - 1].isIndestructible = (bool) EditorGUILayout.Toggle("Indestructable", monsterDataList.monsterList[viewIndex - 1].isIndestructible, GUILayout.ExpandWidth(false));
				monsterDataList.monsterList[viewIndex - 1].isQuestMonster = (bool) EditorGUILayout.Toggle("QuestMonster", monsterDataList.monsterList[viewIndex - 1].isQuestMonster, GUILayout.ExpandWidth(false));
				GUILayout.EndHorizontal();

				GUILayout.Space(10);

				GUILayout.BeginHorizontal();
				monsterDataList.monsterList[viewIndex - 1].isStackable = (bool) EditorGUILayout.Toggle("Stackable ", monsterDataList.monsterList[viewIndex - 1].isStackable, GUILayout.ExpandWidth(false));
				monsterDataList.monsterList[viewIndex - 1].destroyOnUse = (bool) EditorGUILayout.Toggle("Destroy On Use", monsterDataList.monsterList[viewIndex - 1].destroyOnUse, GUILayout.ExpandWidth(false));
				monsterDataList.monsterList[viewIndex - 1].encumbranceValue = EditorGUILayout.FloatField("Encumberance", monsterDataList.monsterList[viewIndex - 1].encumbranceValue, GUILayout.ExpandWidth(false));
				GUILayout.EndHorizontal();*/

				GUILayout.Space(10);

			} else {
				GUILayout.Label("This Inventory List is Empty.");
			}
		}
		if(GUI.changed) {
			EditorUtility.SetDirty(monsterDataList);
		}
	}

	void CreateNewMonsterList() {
		// There is no overwrite protection here!
		// There is No "Are you sure you want to overwrite your existing object?" if it exists.
		// This should probably get a string from the user to create a new name and pass it ...
		/*viewIndex = 1;
		monsterDataList = CreateMonsterDataList.Create();
		if(monsterDataList) {
			monsterDataList.monsterList = new List<MonsterData>();
			string relPath = AssetDatabase.GetAssetPath(monsterDataList);
			EditorPrefs.SetString("ObjectPath", relPath);
		}*/
	}

	void OpenMonsterList() {
		string absPath = EditorUtility.OpenFilePanel("Select Monster Data List", "", "");
		if(absPath.StartsWith(Application.dataPath)) {
			string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
			monsterDataList = AssetDatabase.LoadAssetAtPath(relPath, typeof(MonsterDataList)) as MonsterDataList;
			if(monsterDataList.monsterList == null)
				monsterDataList.monsterList = new List<MonsterData>();
			if(monsterDataList) {
				EditorPrefs.SetString("ObjectPath", relPath);
			}
		}
	}

	void AddMonster() {
		MonsterData newMonster = new MonsterData() {
			title = "New Monster"
		};
		monsterDataList.monsterList.Add(newMonster);
		viewIndex = monsterDataList.monsterList.Count;
	}

	void DeleteMonster(int index) {
		monsterDataList.monsterList.RemoveAt(index);
	}
}
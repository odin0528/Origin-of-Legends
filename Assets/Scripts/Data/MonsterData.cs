using UnityEngine;
using System.Collections;

[System.Serializable]
public class MonsterData {
	public string title = "New Monster";
	public int hp = 50;
	[Range(0, 50f)]
	public float hpRate = 8f;
	public int atk = 10;
	public float atkRate = 8f;
	public int def = 0;
	[Range(1.0f, 10f)]
	public float speed = 3.0f;
	public GameObject prefab;
}
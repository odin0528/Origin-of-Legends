using UnityEngine;
using System.Collections;

[System.Serializable]
public class BuffData {
	public string title = "New Buff";
	public GameObject effectPrefab;
	public float duration;
	public bool isStackable = false;
	public bool isDot = false;
	public int triggerCount = 1;
	public float[] parameter;
}
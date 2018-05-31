using UnityEngine;
using System.Collections;

public abstract class Mod {
	protected string[] title;		//顯示在UI上的詞綴名
	public int affixLv;
	public float[] value;
	protected string desc;		//點上去以後出現的說明
	abstract public void ApplyToEquip(Equip equip);
}
using UnityEngine;
public enum PickMode {
	CRAFT,
	EQUIP
}

public enum ItemType {
	EQUIP,
	ITEM
}

static class Config {
	public static Color32[] rarityColor = new Color32[6] {
		Color.white,
		new Color32(0,102,204,255),
		Color.yellow,
		Color.green,
		new Color32(255,0,255,255),
		new Color32(255,170,0,255)
	};
}
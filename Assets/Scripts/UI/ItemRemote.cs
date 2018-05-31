using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemRemote:MonoBehaviour {
	public ItemType itemType;
	public int dbId;
	public int rarity = 0;
	public bool isDragging = false;
	public RectTransform scrollView;
	public Scrollbar scrollBar;
	private Vector2 pos;

	public void whenClick() {
		if(!isDragging) {
			switch(GameManager.Instance.pickMode) {
				case PickMode.CRAFT:
					CraftManager.Instance.pick(dbId);
					break;
				default:
					break;
			}
		}
	}

	public void whenDragEnd() {
		isDragging = false;
	}

	public void whenDragStart() {
		pos = Input.mousePosition;
		isDragging = true;
	}

	public void whenDrag() {
		float distance = Input.mousePosition.y - pos.y;
		if(distance < 0 && scrollBar.value < 1 ||
			distance > 0 && scrollBar.value > 0) {
			pos = Input.mousePosition;
			scrollView.localPosition = new Vector3(scrollView.localPosition.x, scrollView.localPosition.y + distance, scrollView.localPosition.z);
		}
	}
}

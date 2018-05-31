using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Olrun: Hero {
	public float increaseSpellDamage = 20f;
	public Slider enBar;

	override public int powerAttckEffectCounter {
		get {
			return _powerAttckEffectCounter;
		}
		set {
			_powerAttckEffectCounter = value;
			enBar.value = powerAttckEffectCounter;
		}
	}

	override protected void Init() {
		prop.increaseSpellDamage += increaseSpellDamage;
	}

	override protected void SetStatusBar() {
		base.SetStatusBar();
		GameObject characterEnBar = Instantiate(Resources.Load<GameObject>("Prefabs/UI/Battlefield/EnBar"));
		characterEnBar.transform.SetParent(GameObject.Find("/Canvas/HeroPanel/" + sn.ToString()).transform, false);
		enBar = characterEnBar.GetComponent<Slider>();
	}

	public override void AfterCast() {
		enBar.value = powerAttckEffectCounter;
	}
}

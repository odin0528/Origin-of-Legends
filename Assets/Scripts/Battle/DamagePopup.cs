using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamagePopup : MonoBehaviour {
	//public ActionInfo info;
	public int damage;

	public float freeTime=2.0F;
//	public Transform damageText;
	// Use this for initialization
	void Start () {
		StartCoroutine("Disappear");
		transform.GetComponent<Text>().text = damage.ToString();
		/*if(info.heal > 0){
			transform.GetComponent<Text>().color = Color.green;
			transform.GetComponent<Text>().text = info.heal.ToString();
		}else{
			transform.GetComponent<Text>().text = info.damage.ToString();
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.up * 50.0F * Time.deltaTime);
		Color color = transform.GetComponent<Text> ().color;
		color.a -= 0.005f;
		transform.GetComponent<Text> ().color = color;
	}

	IEnumerator Disappear(){
		yield return new WaitForSeconds(freeTime);
		Destroy(this.gameObject);
	}
}

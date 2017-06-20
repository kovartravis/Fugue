using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthNode : MonoBehaviour{
	public Sprite healthBarEmpty;
	
	void Start(){
		gameObject.transform.SetParent(GameObject.Find ("Canvas").transform, false);
	}
	
	public void setSpriteFull(){
		gameObject.GetComponent<Image>().overrideSprite = null;
	}
	public void setSpriteEmpty(){
		gameObject.GetComponent<Image>().overrideSprite = healthBarEmpty;
	}
}

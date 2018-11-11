using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

	//private Color myColor;
	public Renderer rend;
	Color neg = Color.red;
	Color pos = Color.green;
	public float lerp = 0.5f;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		//rend.material.color = Color.green;
		//myColor = gameObject.GetComponent<Material>().Color();
	}
	
	// Update is called once per frame
	void Update () {
		//myColor = Color.red;
		
		rend.material.color = Color.Lerp(neg, pos, lerp);
	}
	
	void SetSentimentLerp(float lerp){
		Debug.Log("Chaning lerp");
		this.lerp = lerp;
	}
}

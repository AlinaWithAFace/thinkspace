using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditText : MonoBehaviour {

public GameObject myObject;
TextMeshPro myMesh;

public String myString;

	// Use this for initialization
	void Start () {
		myMesh = myObject.GetComponentInChildren<TextMeshPro>();
	}
	
	// Update is called once per frame
	void Update () {
		//myMesh.text = myString;
		//Debug.Log(myMesh.text);
	}
}
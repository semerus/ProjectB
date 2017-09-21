using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changealpha : MonoBehaviour {

	public float alphaLevel = .5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, alphaLevel);
		
	}
}

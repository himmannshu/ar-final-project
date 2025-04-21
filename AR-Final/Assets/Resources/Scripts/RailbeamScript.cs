using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailbeamScript : MonoBehaviour {
	public int damage = 120,
			   cooldown = 240;
	
	float initialRed, initialGreen, initialBlue, initialAlpha;
	float elapsed, animationDuration = 1f;
	
	void Start() {
		//initialize animation stuff
		Color initialColor = gameObject.GetComponent<Renderer>().material.color;
		initialRed = initialColor.r;
		initialGreen = initialColor.g;
		initialBlue = initialColor.b;
		initialAlpha = initialColor.a;
		elapsed = 0f;
		
		//make sure it's pointing the right way
		transform.Rotate(90, 0, 0);
		
		//destroy the collider before the entire object
		//Destroy(gameObject.GetComponent<Rigidbody>(), 0.05f);		//FIXME
		//Destroy(gameObject, animationDuration);
		Destroy(gameObject, 0.05f);
	}
	
	void Update() {
		//become more transparent
		float a = animation(elapsed / animationDuration) * initialAlpha;
		gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(initialRed, initialGreen, initialBlue, a));		//FIXME
		elapsed += Time.deltaTime;
	}
	
	float animation(float x) {
		if(x < 0.30) return 1f;
		if(x < 1.00) return 1 - (x - 0.3f) / 0.7f;
		else return 0f;
	}
}

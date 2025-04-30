using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailbeamScript : MonoBehaviour {
	public int damage = 70,
			   cooldown = 240;
	
	MeshRenderer mr;
	float initialRed, initialGreen, initialBlue, initialAlpha;
	float elapsed, animationDuration = 0.5f;
	
	void Start() {
		//initialize animation stuff
		mr = gameObject.GetComponent<MeshRenderer>();
		
		Color initialColor = mr.materials[0].color;
		initialRed = initialColor.r;
		initialGreen = initialColor.g;
		initialBlue = initialColor.b;
		initialAlpha = initialColor.a;
		
		elapsed = 0f;
		
		//make sure it's pointing the right way
		transform.Rotate(90, 0, 0);
		
		//destroy the collider before the entire object
		Destroy(gameObject.GetComponent<CapsuleCollider>(), 0.05f);
		Destroy(gameObject.GetComponent<Rigidbody>(), 0.05f);
		Destroy(gameObject, animationDuration);
		
		//Destroy(gameObject, 0.05f);
		
		//FIXME: replace this with a raycast
		//(if we're trying to obstruct augmented elements)
	}
	
	void Update() {
		//become more transparent
		float a = animation(elapsed / animationDuration) * initialAlpha;
		Material[] mats = mr.materials;
		mats[0].color = new Color(initialRed, initialGreen, initialBlue, a);
		mr.materials = mats;
		elapsed += Time.deltaTime;
	}
	
	float animation(float x) {
		if(x < 1) return 1 - x;
		else return 0f;
	}
}

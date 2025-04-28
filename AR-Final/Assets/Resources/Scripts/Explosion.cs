using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {
	public int damage = 90;
	
	float initialScale;
	float elapsed, animationDuration = 0.7f;
	
	public float GetMaxDist() {
		return initialScale;
	}
	
	void Start() {
		//animation stuff
		initialScale = transform.localScale.x;
		elapsed = 0f;
		
		//delete
		Destroy(gameObject, animationDuration);
	}
	
	void Update() {
		//animation
		float s = animation(elapsed / animationDuration) * initialScale;
		transform.localScale = new Vector3(s, s, s);
		elapsed += Time.deltaTime;
		//TODO: animate alpha, remove light
	}
	
	float animation(float x) {
		if(x < 0.1) return 0.9f * (x / 0.1f);
		if(x < 0.2) return 0.9f + (x - 0.1f);
		if(x < 0.7) return 1.0f - (x - 0.2f) / 0.5f;
		else return 0f;
	}
}

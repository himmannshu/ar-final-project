using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour {
	float initialScale;
	float elapsed;
	
	void Start() {
		initialScale = transform.localScale.x;
		elapsed = 0f;
		Destroy(gameObject, 0.5f);
	}
	
	void Update() {
		float s = initialScale * scaleAnimation(elapsed / 0.5f);
		transform.localScale = new Vector3(s, s, s);
		elapsed += Time.deltaTime;
	}
	
	private float scaleAnimation(float x) {
		if(x < 0.05) return 20 * x;
		if(x < 0.20) return 1f;
		if(x < 0.50) return (1f / 3f) * (5 - 10 * x);
		else return 0f;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberScript : MonoBehaviour {
	float initialScale;
	float elapsed, animationDuration = 0.5f;
	
	void Start() {
		initialScale = transform.localScale.x;
		elapsed = 0f;
		Destroy(gameObject, animationDuration);
	}
	
	void Update() {
		float s = animation(elapsed / animationDuration) * initialScale;
		transform.localScale = new Vector3(s, s, s);
		elapsed += Time.deltaTime;
	}
	
	float animation(float x) {
		if(x < 0.05) return 20 * x;
		if(x < 0.20) return 1f;
		if(x < 0.50) return (1f / 3f) * (5 - 10 * x);
		else return 0f;
	}
}

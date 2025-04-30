using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour {
	public GameObject explosionPrefab;
	public int damage = 45,
			   cooldown = 60;
	public float baseSpeed = 8f;
	
	float initialScale;
	float elapsed, animationDuration = 20f;
	
	void Start() {
		//animation stuff
		initialScale = transform.localScale.x;
		elapsed = 0f;
		
		//delete
		Destroy(gameObject, animationDuration);
	}
	
	void OnCollisionEnter(Collision collision) {
		GameObject other = collision.gameObject;
		
		if(other.tag == "Railbeam") {
			//explode
			Explode(transform.position);
			return;
		}
		
		Destroy(gameObject);
	}
	
	void Update() {
		//animation
		float s = animation(elapsed / animationDuration) * initialScale;
		transform.localScale = new Vector3(s, s, s);
		elapsed += Time.deltaTime;
	}
	
	float animation(float x) {
		if(x < 1) return 1 - Mathf.Sin(x * 360) * (1 - x) * 0.1f;
		else return 0f;
	}
	
	public void Explode(Vector3 x) {
		Instantiate(explosionPrefab, x, Quaternion.identity);
		Destroy(gameObject);
	}
}

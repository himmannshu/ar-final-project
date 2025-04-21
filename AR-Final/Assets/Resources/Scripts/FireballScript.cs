using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour {
	public GameObject explosionPrefab;
	public int damage = 60,
			   cooldown = 60;
	
	float baseSpeed = 8f;
	Rigidbody rb;
	
	float initialScale;
	float elapsed, animationDuration = 20f;
	
	public void Explode(Vector3 x) {
		Instantiate(explosionPrefab, x, Quaternion.identity);
		Destroy(gameObject);
	}
	
	void Start() {
		//animation stuff
		initialScale = transform.localScale.x;
		elapsed = 0f;
		
		//start moving
		rb = GetComponent<Rigidbody>();
		rb.AddForce(baseSpeed * transform.forward, ForceMode.VelocityChange);
		
		//delete
		Destroy(gameObject, animationDuration);
	}
	
	void OnCollisionEnter(Collision collision) {
		GameObject other = collision.gameObject;
		
		if(other.tag == "Railbeam") {
			//explode
			Explode(transform.position);
		}
	}
	
	void Update() {
		//animation
		float s = animation(elapsed / animationDuration) * initialScale;
		transform.localScale = new Vector3(s, s, s);
		elapsed += Time.deltaTime;
	}
	
	float animation(float x) {
		if(x < 1) return 1 - Mathf.Sin((0.25f * animationDuration) * x * 360) * (1 - x) * 0.1f;
		else return 0f;
	}
}

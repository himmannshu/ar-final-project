using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
	public int baseHealth = 100;
	
	int health;
	
	void Start() {
		health = baseHealth;
	}
	
	void OnCollisionEnter(Collision collision) {
		GameObject other = collision.gameObject;
		
		if(other.tag == "Bullet") {
			Destroy(other);
			
			health -= other.GetComponent<BulletScript>().damage;
		}
		
		if(health <= 0) {
			Destroy(gameObject);
		}
	}
}

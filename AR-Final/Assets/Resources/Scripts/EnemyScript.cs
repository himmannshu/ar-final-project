using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyScript : MonoBehaviour {
	public int baseHealth = 100;
	public GameObject damageNumberPrefab;
	
	int health;
	
	void Start() {
		health = baseHealth;
	}
	
	void OnCollisionEnter(Collision collision) {
		GameObject other = collision.gameObject;
		
		if(other.tag == "Bullet") {
			//destroy bullet
			Destroy(other);
			
			//reduce health
			int bulletDamage = other.GetComponent<BulletScript>().damage;
			health -= bulletDamage;
			
			//spawn damage number thing
			ContactPoint contact = collision.GetContact(0);
			GameObject damageText = Instantiate(
				damageNumberPrefab,
				contact.point,
				Quaternion.LookRotation(contact.normal, Vector3.up));
			damageText.GetComponent<TextMeshPro>().SetText($"{bulletDamage}");
		}
		
		if(health <= 0) {
			//kill this one
			Destroy(gameObject);
		}
	}
}

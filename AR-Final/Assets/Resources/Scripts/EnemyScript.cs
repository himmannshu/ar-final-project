using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyScript : MonoBehaviour {
	public GameObject damageNumberPrefab;
	public int baseHealth = 100;
	
	int health;
	
	void Start() {
		health = baseHealth;
	}
	
	void OnCollisionEnter(Collision collision) {
		GameObject other = collision.gameObject;
		
		if(other.tag == "Railbeam") {
			//reduce health
			int damage = other.GetComponent<RailbeamScript>().damage;
			health -= damage;
			
			//need to make sure to only be damaged by the beam once...
			//maybe have collider be deleted?
			
			//spawn damage number thing
			spawnDamageNumber(collision.GetContact(0), damage);
			//FIXME: it spawns the number, just sort the collision contacts by nearest to player
		}
		
		if(other.tag == "Fireball") {
			//reduce health
			int damage = other.GetComponent<FireballScript>().damage;
			health -= damage;
			
			//make it explode (?)
			//other.GetComponent<FireballScript>().Explode(collision.GetContact(0).point);
			Destroy(other);	//FIXME: check if this is desired function
			
			//spawn damage number thing
			spawnDamageNumber(collision.GetContact(0), damage);
		}
		
		if(other.tag == "Explosion") {
			//reduce health
			//FIXME: make sure this makesense. also, replace the constant with the initialScale value
			float maxDist = other.GetComponent<ExplosionScript>().GetMaxDist(),
				  dist = Vector3.Distance(other.transform.position, collision.GetContact(0).point);
			int damage = (int)((float)other.GetComponent<ExplosionScript>().damage * (maxDist - dist) / maxDist);
			health -= damage;
			
			//spawn damage number thing
			spawnDamageNumber(collision.GetContact(0), damage);
		}
		
		//destroy enemy
		if(health <= 0) {
			//kill this one
			Destroy(gameObject);
			GameManager.Instance.AddScore(1);
		}
	}
	
	void spawnDamageNumber(ContactPoint contact, int damage) {
		GameObject damageText = Instantiate(
			damageNumberPrefab,
			contact.point,
			Quaternion.LookRotation(contact.normal, Vector3.up));
		damageText.GetComponent<TextMeshPro>().SetText($"{damage}");
	}
}

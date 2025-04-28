using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class EnemyDamage : MonoBehaviour {
	public DeathEffect deathEffect;
	//public UnityEvent<int> splitEvent;
	public GameObject damageNumberPrefab;
	public int baseHealth = 100,
			   score = 1;
	public float fireballWeakness = 0.0f,
				 railbeamWeakness = 0.0f;
	
	GameObject camera;
	int health;
	
	void Start() {
		camera = GameObject.Find("Main Camera");
		health = baseHealth;
	}
	
	void OnCollisionEnter(Collision collision) {
		GameObject other = collision.gameObject;
		
		if(other.tag == "Railbeam") {
			//reduce health
			int damage = (int)(other.GetComponent<RailbeamScript>().damage * (1 + railbeamWeakness));
			health -= damage;
			
			//need to make sure to only be damaged by the beam once...
			//maybe have collider be deleted?
			
			//spawn damage number thing
			spawnDamageNumber(collision.GetContact(0), damage);
			//FIXME: it spawns the number, just sort the collision contacts by nearest to player
		}
		
		if(other.tag == "Fireball") {
			//reduce health
			int damage = (int)(other.GetComponent<FireballScript>().damage * (1 + fireballWeakness));
			health -= damage;
			
			//delete fireball
			Destroy(other);
			
			//spawn damage number thing
			spawnDamageNumber(collision.GetContact(0), damage);
		}
		
		if(other.tag == "Explosion") {
			//reduce health
			//FIXME: make sure this makesense. also, replace the constant with the initialScale value
			float maxDist = other.GetComponent<ExplosionScript>().GetMaxDist(),
				  dist = Vector3.Distance(other.transform.position, collision.GetContact(0).point);
			int damage = (int)(other.GetComponent<ExplosionScript>().damage * (maxDist - dist) / maxDist);
			health -= damage;
			
			//spawn damage number thing
			spawnDamageNumber(collision.GetContact(0), damage);
		}
		
		//destroy enemy
		if(health <= 0) {
			if(deathEffect != null) {
				deathEffect.go();
			}
			
			//for splitting
			/*
			if(gameObject.CompareTag("EnemySplitter")) {
				splitEvent?.Invoke(gameObject.GetComponent<Splitting>().splitsRemaining);
			}
			*/
			
			Destroy(gameObject);
			GameManager.Instance.AddScore(score);
		}
	}
	
	void spawnDamageNumber(ContactPoint contact, int damage) {
		GameObject damageText = Instantiate(
			damageNumberPrefab,
			contact.point,
			Quaternion.LookRotation(contact.point - camera.transform.position, Vector3.up));
		damageText.GetComponent<TextMeshPro>().SetText($"{damage}");
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class EnemyDamage : MonoBehaviour {
	public DeathEffect deathEffect;
	public GameObject damageNumberPrefab;
	public int baseHealth = 100,
			   score = 1,
			   continuousDamageTickDelay = 5;
	public float fireballWeakness  = 0.0f,
				 railbeamWeakness  = 0.0f,
				 lightningWeakness = 0.0f;
	
	GameObject camera;
	int health, continuousDamage, tick;
	
	void Start() {
		camera = GameObject.Find("Main Camera");
		health = baseHealth;
		tick = 0;
	}
	
	void FixedUpdate() {
		if(tick-- <= 0 && continuousDamage > 0) {
			//reduce health
			health -= continuousDamage;
			
			//spawn damage number thing
			spawnDamageNumber(transform.position + 0.5f * (Vector3.up + (camera.transform.position - transform.position).normalized), continuousDamage);
			
			//reset tick delay
			tick = continuousDamageTickDelay;
		}
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
			spawnDamageNumber(collision.GetContact(0).point, damage);
			//FIXME: it spawns the number, just sort the collision contacts by nearest to player
		}
		
		if(other.tag == "Fireball") {
			//reduce health
			int damage = (int)(other.GetComponent<FireballScript>().damage * (1 + fireballWeakness));
			health -= damage;
			
			//delete fireball
			Destroy(other);
			
			//spawn damage number thing
			spawnDamageNumber(collision.GetContact(0).point, damage);
		}
		
		if(other.tag == "Explosion") {
			//reduce health
			//FIXME: make sure this makesense. also, replace the constant with the initialScale value
			float maxDist = other.GetComponent<ExplosionScript>().GetMaxDist(),
				  dist = Vector3.Distance(other.transform.position, collision.GetContact(0).point);
			int damage = (int)(other.GetComponent<ExplosionScript>().damage * (maxDist - dist) / maxDist);
			health -= damage;
			
			//spawn damage number thing
			spawnDamageNumber(collision.GetContact(0).point, damage);
		}
		
		if(other.tag == "Lightning") {
			//update continuous damage
			int damage = (int)(other.GetComponent<LightningScript>().damage * (1 + lightningWeakness));
			continuousDamage += damage;
			
			//spawn damage number thing
			spawnDamageNumber(collision.GetContact(0).point, damage);
		}
		
		//destroy enemy
		if(health <= 0) {
			if(deathEffect != null) {
				deathEffect.go();
			}
			
			Destroy(gameObject);
			GameManager.Instance.AddScore(score);
		}
	}
	
	void OnCollisionExit(Collision collision) {
		GameObject other = collision.gameObject;
		
		if(other.tag == "Lightning") {
			//update continuous damage
			continuousDamage -= (int)(other.GetComponent<LightningScript>().damage * (1 + lightningWeakness));
		}
	}
	
	void spawnDamageNumber(Vector3 point, int damage) {
		GameObject damageText = Instantiate(
			damageNumberPrefab,
			point,
			Quaternion.LookRotation(point - camera.transform.position, Vector3.up));
		damageText.GetComponent<TextMeshPro>().SetText($"{damage}");
	}
}

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
			   continuousDamageTickDelay = 15;
	public float fireballWeakness  = 0.0f,
				 railbeamWeakness  = 0.0f,
				 lightningWeakness = 0.0f;
	
	GameObject camera;
	int health, continuousDamage, tick;
	bool dead = false;
	
	float initialScale, initialRotx, initialRoty, initialRotz;
	float elapsed, animationDuration = 1.0f;
	
	void Start() {
		//text orientation
		camera = GameObject.Find("Main Camera");
		
		//health and damage
		dead = false;
		health = baseHealth;
		tick = 0;
		
		//death shrink animation
		elapsed = 0;
		initialScale = transform.localScale.x;
	}
	
	void FixedUpdate() {
		if(dead) return;
		
		//destroy enemy
		if(health <= 0) {
			if(deathEffect != null) {
				deathEffect.go();
			}
			
			dead = true;
			
			initialRotx = transform.rotation.x;
			initialRoty = transform.rotation.y;
			initialRotz = transform.rotation.z;
			
			GameManager.Instance.AddScore(score);
			
			Destroy(gameObject, 1f);
			
			//gameObject.SetActive(false);
		}
	}
	
	void Update() {
		if(!dead) return;
		
		//animate shrinking
		float s = animationScale(elapsed / animationDuration) * initialScale;
		transform.localScale = new Vector3(s, s, s);
		
		//animation rotation
		//Vector3 axis = Vector3.Cross(player.position - transform.position, Vector3.up).normalized;
		float a = animationRotation(elapsed / animationDuration);
		transform.rotation = Quaternion.Euler(initialRotx - a, initialRoty, initialRotz);
		
		elapsed += Time.deltaTime;
	}
	
	void OnCollisionEnter(Collision collision) {
		if(dead) return;
		
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
	}
	
	void OnCollisionStay(Collision collision) {
		if(dead) return;
		
		GameObject other = collision.gameObject;
		
		if(--tick <= 0 && other.tag == "Lightning") {
			//reduce health
			int damage = (int)(other.GetComponent<LightningScript>().damage * (1 + lightningWeakness));
			health -= damage;
			
			//spawn damage number thing
			spawnDamageNumber(transform.position + 0.4f * (Vector3.up + (camera.transform.position - transform.position).normalized), damage);
			
			//reset tick delay
			tick = continuousDamageTickDelay;
		}
	}
	
	void spawnDamageNumber(Vector3 point, int damage) {
		GameObject damageText = Instantiate(
			damageNumberPrefab,
			point,
			Quaternion.LookRotation(point - camera.transform.position, Vector3.up));
		damageText.GetComponent<TextMeshPro>().SetText($"{damage}");
	}
	
	float animationScale(float x) {
		if(x < 0.5f) return 1;
		if(x < 1.0f) return 1 - x;
		return 0;
	}
	
	float animationRotation(float x) {
		if(x < 0.5f) return 90 * x;
		return 90;
	}
}

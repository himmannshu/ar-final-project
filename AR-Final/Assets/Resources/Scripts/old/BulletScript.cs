using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	public int damage = 5;
	
	float baseSpeed = 11f;
	
	Rigidbody rb;
	
	void Start() {
		rb = GetComponent<Rigidbody>();
		rb.AddForce(baseSpeed * transform.forward, ForceMode.VelocityChange);
	}
}

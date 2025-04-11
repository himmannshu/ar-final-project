using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	Rigidbody rb;
	float baseSpeed = 15f;
	
	void Start() {
		rb = GetComponent<Rigidbody>();
		rb.AddForce(baseSpeed * transform.forward, ForceMode.VelocityChange);
	}
}

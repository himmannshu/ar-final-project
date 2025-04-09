using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	Rigidbody rb;
	float baseSpeed = 10f;
	float speed;
	
	void Start() {
		rb = GetComponent<Rigidbody>();
		speed = baseSpeed * OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);
		rb.AddForce(speed * transform.forward, ForceMode.VelocityChange);
	}
}

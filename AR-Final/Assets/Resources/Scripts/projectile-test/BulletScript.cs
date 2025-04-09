using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
	Rigidbody rb;
	
	//Start is called before the first frame update
	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	//Update is called once per frame
	//https://developers.meta.com/horizon/documentation/unity/unity-tutorial-basic-controller-input#reference-script
	void Update() {
		transform.rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
		rb.AddForce(10 * OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) * transform.forward);
	}
}

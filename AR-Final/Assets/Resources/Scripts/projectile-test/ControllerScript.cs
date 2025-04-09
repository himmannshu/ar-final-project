using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllers : MonoBehaviour {
	Rigidbody rb;
	
	//Start is called before the first frame update
	void Start() {
		rb = GetComponent<Rigidbody>();
	}

	//Update is called once per frame
	//https://developers.meta.com/horizon/documentation/unity/unity-tutorial-basic-controller-input#reference-script
	void Update() {
		if(OVRInput.Get(OVRInput.Button.One)) {
			rb.AddForce(0, 20, 0, ForceMode.Force);
		}
	}
}

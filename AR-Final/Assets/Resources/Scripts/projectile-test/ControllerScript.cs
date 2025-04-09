using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {
	public GameObject bulletPrefab;
	public int spawnDelay = 10;
	int a;
	
	void Start() {
		a = spawnDelay;
	}
	
	void FixedUpdate() {
		if(a <= 0 && OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.5f) {
			Instantiate(
				bulletPrefab,
				OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch) + (OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward),
				OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));
			a = spawnDelay;
		}
		a--;
	}
}

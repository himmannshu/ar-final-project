using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {
	public GameObject bulletPrefab;
	public int spawnDelay = 5;
	int a, b;
	
	void Start() {
		a = spawnDelay;
		b = spawnDelay;
	}
	
	void FixedUpdate() {
		if(a <= 0 && OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > 0.5f) {
			Instantiate(
				bulletPrefab,
				OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch) + 0.2f * (OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch) * Vector3.forward),
				OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch));
			a = spawnDelay;
		}
		if(b <= 0 && OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > 0.5f) {
			Instantiate(
				bulletPrefab,
				OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch) + 0.2f * (OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) * Vector3.forward),
				OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch));
			b = spawnDelay;
		}
		a--;
		b--;
	}
}

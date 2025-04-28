using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {
	public GameObject fireballPrefab, railbeamPrefab;
	
	OVRInput.Controller left, right;
	Vector3 leftPos, rightPos,
			leftForw, rightForw;
	Quaternion leftRot, rightRot;
	
	int fireballCooldown, fireballDelay,
		railgunCooldown, railgunDelay;
	
	void Start() {
		//controller shorthand
		left = OVRInput.Controller.LTouch;
		right = OVRInput.Controller.RTouch;
		
		//spell cooldowns and delays
		fireballCooldown = fireballPrefab.GetComponent<FireballScript>().cooldown;
		fireballDelay = fireballCooldown;
		
		railgunCooldown = railbeamPrefab.GetComponent<RailbeamScript>().cooldown;
		railgunDelay = railgunCooldown;
	}
	
	void FixedUpdate() {
		//controller properties
		leftPos = OVRInput.GetLocalControllerPosition(left);
		leftRot = OVRInput.GetLocalControllerRotation(left);
		leftForw = leftRot * Vector3.forward;
		
		rightPos = OVRInput.GetLocalControllerPosition(right);
		rightRot = OVRInput.GetLocalControllerRotation(right);
		rightForw = rightRot * Vector3.forward;
		
		//left hand, shoot fireball
		if(fireballDelay <= 0 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, left) > 0.5f) {
			//cooldown
			fireballDelay = fireballCooldown;
			
			//spawn
			GameObject fireball = Instantiate(fireballPrefab, leftPos + 0.1f * leftForw, leftRot);
			//TODO: throwing?
		}
		
		//right hand, shoot railgun
		if(railgunDelay <= 0 && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, right) > 0.5f) {
			//cooldown
			railgunDelay = railgunCooldown;
			
			//spawn
			GameObject railbeam = Instantiate(railbeamPrefab, rightPos + (100.1f * rightForw), rightRot);
		}
		
		//get ready to shoot again
		if(fireballDelay > 0) fireballDelay--;
		if(railgunDelay > 0) railgunDelay--;
	}
}

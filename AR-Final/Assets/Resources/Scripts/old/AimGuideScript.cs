using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGuideScript : MonoBehaviour {
	public bool rightNotLeft = false;
	
	OVRInput.Controller ctrl;
	Vector3 pos, forw;
	Quaternion rot;
	
	LineRenderer guide;
	
	void Start() {
		//controller shorthand
		ctrl = (rightNotLeft) ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch;
		
		//aim guide
		guide = gameObject.GetComponent<LineRenderer>();
	}
	
	void Update() {
		//controller properties
		pos = OVRInput.GetLocalControllerPosition(ctrl);
		rot = OVRInput.GetLocalControllerRotation(ctrl);
		forw = rot * Vector3.forward;
		
		//aim guides
		guide.SetPosition(0, pos);
		guide.SetPosition(1, pos + forw);
	}
}

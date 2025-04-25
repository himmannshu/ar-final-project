using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class TestScript : MonoBehaviour {
	public GameObject xrOrigin, camera, fireballPrefab;
	public float minimumVelocity = 0.03f;
	
	const int averageWindow = 180;
	
	bool gestureActive = false;
	Vector3 palmPosition = new Vector3(0f, 0f, 0f),
			palmVelocity = new Vector3(0f, 0f, 0f);
	Vector3[] palmVelocities = new Vector3[averageWindow];
	int pvi = 0;
	
	//https://docs.unity3d.com/Packages/com.unity.xr.hands@1.1/manual/hand-data/xr-hand-access-data.html
	void Start() {
		XRHandSubsystem m_Subsystem = 
			XRGeneralSettings.Instance?
			.Manager?
			.activeLoader?
			.GetLoadedSubsystem<XRHandSubsystem>();

		if(m_Subsystem != null) {
			m_Subsystem.updatedHands += OnHandUpdate;
		}
	}
	
	void OnHandUpdate(XRHandSubsystem subsystem,
					  XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags,
					  XRHandSubsystem.UpdateType updateType) {
		switch(updateType) {
			case XRHandSubsystem.UpdateType.Dynamic:
				//Update game logic that uses hand data
				
				UpdateTransform(subsystem.leftHand);
				
				break;
			case XRHandSubsystem.UpdateType.BeforeRender: 
				//Update visual objects that use hand data
				break;
		}
	}
	
	void UpdateTransform(XRHand hand) {
		//https://docs.unity3d.com/Packages/com.unity.xr.hands@1.1/api/UnityEngine.XR.Hands.XRHand.html#UnityEngine_XR_Hands_XRHand_GetJoint_UnityEngine_XR_Hands_XRHandJointID_
		var joint = hand.GetJoint(XRHandJointID.Palm);
		
		//https://docs.unity3d.com/Packages/com.unity.xr.hands@1.1/api/UnityEngine.XR.Hands.XRHandJoint.html#UnityEngine_XR_Hands_XRHandJoint_TryGetPose_UnityEngine_Pose__
		if(joint.TryGetPose(out Pose pose)) {
			palmPosition = pose.GetTransformedBy(new Pose(xrOrigin.transform.position, xrOrigin.transform.rotation)).position;
		}
		if(joint.TryGetLinearVelocity(out Vector3 linearVelocity)) {
			//keep track of past velocities...
			palmVelocities[pvi++] = linearVelocity;
			if(pvi >= averageWindow) pvi = 0;
			
			//... to compute average velocity
			palmVelocity = new Vector3(0f, 0f, 0f);
			for(int i = 0; i < palmVelocities.Length; i++) {
				palmVelocity += palmVelocities[i];
			}
			palmVelocity /= (float)averageWindow;
			
			//check if the palm gesture is detected, moving away from the xrorigin, and moving fast enough
			if(gestureActive
			   && (palmPosition + palmVelocity - camera.transform.position).magnitude > (palmPosition - camera.transform.position).magnitude
			   && palmVelocity.magnitude >= minimumVelocity) {
				//shoot only one
				gestureActive = false;
				Instantiate(fireballPrefab, palmPosition, Quaternion.LookRotation(palmVelocity, Vector3.up));
			}
		}
	}
	
	public void OnGesturePerformed() {
		gestureActive = true;
	}
	
	public void OnGestureEnded() {
		gestureActive = false;
	}
}

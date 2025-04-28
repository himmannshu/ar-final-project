using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class Spellcasting : MonoBehaviour {
	public bool left;
	public GameObject xrOrigin, camera, fireballPrefab, railbeamPrefab;
	public float minimumVelocity = 0.03f;
	
	const int averageWindow = 180;
	
	Pose xrOriginPose;
	
	//for fireball
	bool fireballActive = false;
	Vector3 palmPosition = new Vector3(0f, 0f, 0f),
			palmVelocity = new Vector3(0f, 0f, 0f);
	Vector3[] palmVelocities = new Vector3[averageWindow];
	int pvi = 0;
	
	//for railgun
	bool railgunActive = false;
	Vector3 indexTipPosition = new Vector3(0f, 0f, 0f),
			indexProximalPosition = new Vector3(0f, 0f, 0f);
	
	
	void Start() {
		//https://docs.unity3d.com/Packages/com.unity.xr.hands@1.1/manual/hand-data/xr-hand-access-data.html
		XRHandSubsystem m_Subsystem = 
			XRGeneralSettings.Instance?
			.Manager?
			.activeLoader?
			.GetLoadedSubsystem<XRHandSubsystem>();

		if(m_Subsystem != null) {
			m_Subsystem.updatedHands += OnHandUpdate;
		}
		
		//for pose transform
		xrOriginPose = new Pose(xrOrigin.transform.position, xrOrigin.transform.rotation);
	}
	
	void OnHandUpdate(XRHandSubsystem subsystem,
					  XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags,
					  XRHandSubsystem.UpdateType updateType) {
		switch(updateType) {
			case XRHandSubsystem.UpdateType.Dynamic:
				//Update game logic that uses hand data
				
				UpdateTransform(left ? subsystem.leftHand : subsystem.rightHand);
				
				break;
			case XRHandSubsystem.UpdateType.BeforeRender: 
				//Update visual objects that use hand data
				break;
		}
	}
	
	void UpdateTransform(XRHand hand) {
		//https://docs.unity3d.com/Packages/com.unity.xr.hands@1.1/api/UnityEngine.XR.Hands.XRHand.html#UnityEngine_XR_Hands_XRHand_GetJoint_UnityEngine_XR_Hands_XRHandJointID_
		//https://docs.unity3d.com/Packages/com.unity.xr.hands@1.1/api/UnityEngine.XR.Hands.XRHandJoint.html#UnityEngine_XR_Hands_XRHandJoint_TryGetPose_UnityEngine_Pose__
		
		//for fireball
		var palmJoint = hand.GetJoint(XRHandJointID.Palm);
		
		if(palmJoint.TryGetPose(out Pose palmPose)) {
			palmPosition = palmPose.GetTransformedBy(xrOriginPose).position;
		}
		
		if(palmJoint.TryGetLinearVelocity(out Vector3 palmLinearVelocity)) {
			//keep track of past velocities...
			palmVelocities[pvi++] = palmLinearVelocity;
			if(pvi >= averageWindow) pvi = 0;
			
			//... to compute average velocity
			palmVelocity = new Vector3(0f, 0f, 0f);
			for(int i = 0; i < palmVelocities.Length; i++) {
				palmVelocity += palmVelocities[i];
			}
			palmVelocity /= (float)averageWindow;
		}
		
		//fireball check
		//check if the palm gesture is detected, moving away from the xrorigin, and moving fast enough
		if(fireballActive
		   && (palmPosition + palmVelocity - camera.transform.position).magnitude > (palmPosition - camera.transform.position).magnitude
		   && palmVelocity.magnitude >= minimumVelocity) {
			//shoot only one
			fireballActive = false;
			
			GameObject fireball = Instantiate(fireballPrefab, palmPosition, Quaternion.LookRotation(palmVelocity, Vector3.up));
			float baseSpeed = fireball.GetComponent<FireballScript>().baseSpeed;
			Rigidbody rb = fireball.GetComponent<Rigidbody>();
			//rb.AddForce(palmVelocity * baseSpeed, ForceMode.VelocityChange);
			rb.AddForce(baseSpeed * palmVelocity.normalized, ForceMode.VelocityChange);
			//FIXME
		}
		
		
		//for railgun
		var indexTipJoint = hand.GetJoint(XRHandJointID.IndexTip);
		var indexProximalJoint = hand.GetJoint(XRHandJointID.IndexProximal);
		
		if(indexTipJoint.TryGetPose(out Pose indexTipPose)) {
			indexTipPosition = indexTipPose.GetTransformedBy(xrOriginPose).position;
		}
		if(indexProximalJoint.TryGetPose(out Pose indexProximalPose)) {
			indexProximalPosition = indexProximalPose.GetTransformedBy(xrOriginPose).position;
		}
		
		//railgun check
		if(railgunActive) {
			//shoot only one
			railgunActive = false;
			
			Vector3 forw = (indexTipPosition - indexProximalPosition).normalized;
			GameObject railbeam = Instantiate(railbeamPrefab, indexTipPosition + 100f * forw, Quaternion.LookRotation(forw, Vector3.up));
		}
	}
	
	public void ActivateFireball() { fireballActive = true; }
	public void DeactivateFireball() { fireballActive = false; }
	
	public void ActivateRailgun() { railgunActive = true; }
	public void DeactivateRailgun() { railgunActive = false; }
}

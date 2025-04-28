using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class Spellcasting : MonoBehaviour {
	public bool left;
	public GameObject xrOrigin, camera, fireballPrefab, railbeamPrefab, lightningPrefab;
	public float minimumVelocity = 0.03f;
	
	
	//spell details
	bool fireballActive = false,
		 railgunActive = false,
		 lightningActive = false;
	GameObject lightning = null;
	
	//joint positions
	Pose xrOriginPose;
	Vector3 palmPosition, indexTipPosition, indexProximalPosition, littleProximalPosition, wristPosition, middleProximalPosition, middleDistalPosition;
	
	//joint velocities
	const int averageWindow = 180;
	Vector3 palmVelocity = new Vector3(0f, 0f, 0f);
	Vector3[] palmVelocities = new Vector3[averageWindow];
	int pvi = 0;
	
	
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
		
		//joint positions
		var palmJoint = hand.GetJoint(XRHandJointID.Palm);
		var indexTipJoint = hand.GetJoint(XRHandJointID.IndexTip);
		var indexProximalJoint = hand.GetJoint(XRHandJointID.IndexProximal);
		var littleProximalJoint = hand.GetJoint(XRHandJointID.LittleProximal);
		var wristJoint = hand.GetJoint(XRHandJointID.Wrist);
		var middleProximalJoint = hand.GetJoint(XRHandJointID.MiddleProximal);
		var middleDistalJoint = hand.GetJoint(XRHandJointID.MiddleDistal);
		
		if(palmJoint.TryGetPose(out Pose palmPose)) {
			palmPosition = palmPose.GetTransformedBy(xrOriginPose).position;
		}
		if(indexTipJoint.TryGetPose(out Pose indexTipPose)) {
			indexTipPosition = indexTipPose.GetTransformedBy(xrOriginPose).position;
		}
		if(indexProximalJoint.TryGetPose(out Pose indexProximalPose)) {
			indexProximalPosition = indexProximalPose.GetTransformedBy(xrOriginPose).position;
		}
		if(littleProximalJoint.TryGetPose(out Pose littleProximalPose)) {
			littleProximalPosition = littleProximalPose.GetTransformedBy(xrOriginPose).position;
		}
		if(wristJoint.TryGetPose(out Pose wristPose)) {
			wristPosition = wristPose.GetTransformedBy(xrOriginPose).position;
		}
		if(middleProximalJoint.TryGetPose(out Pose middleProximalPose)) {
			middleProximalPosition = middleProximalPose.GetTransformedBy(xrOriginPose).position;
		}
		if(middleDistalJoint.TryGetPose(out Pose middleDistalPose)) {
			middleDistalPosition = middleDistalPose.GetTransformedBy(xrOriginPose).position;
		}
		
		//joint velocities
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
			
			//spawn fireball
			GameObject fireball = Instantiate(fireballPrefab, palmPosition, Quaternion.LookRotation(palmVelocity, Vector3.up));
			
			//get palm normal vector
			Vector3 one = indexProximalPosition - palmPosition,
					two = littleProximalPosition - palmPosition;
			Vector3 palmNormal = (left ? Vector3.Cross(one, two) : Vector3.Cross(two, one)).normalized,
					wristToPalm = (palmPosition - wristPosition).normalized;
			Vector3 shootDirection = /*Quaternion.AngleAxis(10, Vector3.Cross(palmNormal, wristToPalm)) * */ (wristToPalm + palmNormal).normalized;
			
			//set fireball speed
			float baseSpeed = fireball.GetComponent<FireballScript>().baseSpeed;
			Rigidbody rb = fireball.GetComponent<Rigidbody>();
			
			rb.AddForce(baseSpeed * shootDirection, ForceMode.VelocityChange);
			//rb.AddForce(baseSpeed * palmVelocity.normalized, ForceMode.VelocityChange);
		}
		
		//railgun check
		if(railgunActive) {
			//shoot only one
			railgunActive = false;
			
			Vector3 forw = (indexTipPosition - indexProximalPosition).normalized;
			GameObject railbeam = Instantiate(railbeamPrefab, indexTipPosition + 100f * forw, Quaternion.LookRotation(forw, Vector3.up));
		}
		
		//lightning check
		if(lightningActive) {
			Vector3 one = middleDistalPosition - middleProximalPosition,
					two = littleProximalPosition - middleProximalPosition;
			Vector3 forw = (left ? Vector3.Cross(two, one) : Vector3.Cross(one, two)).normalized;
			
			if(lightning != null) {
				//update position if spawned
				lightning.transform.position = middleProximalPosition + 2.5f * forw;
			}
			else {
				//spawn if unspawned
				lightning = Instantiate(lightningPrefab, middleProximalPosition + 2.5f * forw, Quaternion.LookRotation(forw, Vector3.up));
			}
		}
	}
	
	public void ActivateFireball() { fireballActive = true; }
	public void DeactivateFireball() { fireballActive = false; }
	
	public void ActivateRailgun() { railgunActive = true; }
	public void DeactivateRailgun() { railgunActive = false; }
	
	public void ActivateLightning() { lightningActive = true; }
	public void DeactivateLightning() {
		lightningActive = false;
		Destroy(lightning);
		lightning = null;
	}
}

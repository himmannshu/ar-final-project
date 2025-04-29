using TMPro;
using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

public class PlayerStats : MonoBehaviour
{
    public Canvas canvas;
    public TextMeshProUGUI ScoreText;     
    public TextMeshProUGUI HealthText;
    public GameObject XROrigin;
    public PlayerHealth Player;
    private Vector3 wristPosition;
    private Pose xrOriginPose;
    
    void Start()
    {
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
		xrOriginPose = new Pose(XROrigin.transform.position, XROrigin.transform.rotation);    
        
        Player.OnHealthChanged.AddListener(UpdateHealthUI);     
        GameManager.Instance.OnScoreUpdated.AddListener(UpdateScoreUI);
    }


    void OnHandUpdate(XRHandSubsystem subsystem,
                  XRHandSubsystem.UpdateSuccessFlags updateSuccessFlags,
                  XRHandSubsystem.UpdateType updateType)
    {
        switch(updateType) {
            case XRHandSubsystem.UpdateType.Dynamic:
                //Update game logic that uses hand data
                
                UpdatePlayerStats(subsystem.leftHand);
                
                break;
            case XRHandSubsystem.UpdateType.BeforeRender: 
                //Update visual objects that use hand data
                break;
        }
    }

    void UpdatePlayerStats(XRHand hand)
    {
        var wristJoint = hand.GetJoint(XRHandJointID.Wrist);

        if(wristJoint.TryGetPose(out Pose wristPose)) {
			wristPosition = wristPose.GetTransformedBy(xrOriginPose).position;
		}
       
        Vector3 worldWristPosition = xrOriginPose.rotation * wristPosition + xrOriginPose.position;
        canvas.transform.position = worldWristPosition;
    }

    private void UpdateScoreUI()
    {
        if (ScoreText != null)
        {
            ScoreText.text = $"Score: {GameManager.Instance.Score}";
        }
    }
    private void UpdateHealthUI(float current, float max)
    {
        if (HealthText != null)
        {
            HealthText.text = $"Health: {Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}"; 
        }
    }
}

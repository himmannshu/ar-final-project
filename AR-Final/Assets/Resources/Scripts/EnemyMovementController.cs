using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    private OVRCameraRig cameraRig;

    // Start is called before the first frame update
    void Start()
    {
        cameraRig = FindObjectOfType<OVRCameraRig>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = cameraRig.centerEyeAnchor.position;
        targetPosition.y = transform.position.y; // Keep the enemy at the same height
        agent.SetDestination(targetPosition);
        agent.speed = .75f; // Set the speed of the NavMeshAgent
    }
}

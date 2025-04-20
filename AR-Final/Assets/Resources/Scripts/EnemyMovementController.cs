using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    private OVRCameraRig cameraRig;

    // Start is called before the first frame update
    void Start()
    {
        cameraRig = FindObjectOfType<OVRCameraRig>();
        Debug.Log("EnemyMovementController started"); // Log message for debugging
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("EnemyMovementController working!!!"); // Log message for debugging
        Vector3 targetPosition = cameraRig.transform.position;

        agent.SetDestination(targetPosition);
        agent.speed = .75f; // Set the speed of the NavMeshAgent
    }
}

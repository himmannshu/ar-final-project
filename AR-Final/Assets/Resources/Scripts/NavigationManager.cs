using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    private SceneNavigation sceneNavigation;
    // Start is called before the first frame update
    void Start()
    {
        sceneNavigation = FindObjectOfType<SceneNavigation>();
        if (sceneNavigation == null)
        {
            Debug.LogError("SceneNavigation component not found in the scene.");
            return;
        }
        MRUKRoom currentRoom = MRUK.Instance.GetCurrentRoom();
        if (currentRoom == null)
        {
            Debug.LogError("Current room not found.");
            return;
        }
        sceneNavigation.BuildSceneNavMeshForRoom(currentRoom);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

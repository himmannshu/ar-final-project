using System.Collections;
using UnityEngine;
using Meta.XR.MRUtilityKit;  

public class FloorSpawnner : MonoBehaviour
{
    public FindSpawnPositions spawner;
    public float spawnInterval = 10f;
    public float riseHeight = 1f;
    public float riseDuration = 1f;

    private void Start()
    {
        StartCoroutine(WaitForMRUKReady());
    }

    private IEnumerator WaitForMRUKReady()
    {
        while (MRUK.Instance.Rooms == null || MRUK.Instance.Rooms.Count == 0)
        {
            yield return null;
        }

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            SpawnOne();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnOne()
    {
        spawner.SpawnAmount = 1;
        spawner.StartSpawn();
        spawner.SpawnAmount = 0; 
        if (spawner.transform.childCount > 0)
        {
            Transform spawned = spawner.transform.GetChild(spawner.transform.childCount - 1);
            StartCoroutine(RiseFromFloor(spawned.gameObject));
        }
    }

    private IEnumerator RiseFromFloor(GameObject obj)
    {
        Vector3 startPos = obj.transform.position - Vector3.up * riseHeight;
        obj.transform.position = startPos; // Set the initial position below the floor
        Vector3 targetPos = startPos + Vector3.up * riseHeight;
        float elapsed = 0f;

        while (elapsed < riseDuration)
        {
            obj.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / riseDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        obj.transform.position = targetPos;
    }
}

using System.Collections;
using UnityEngine;
using Meta.XR.MRUtilityKit;  

public class FloorSpawnner : MonoBehaviour
{
    public FindSpawnPositions spawner;
    public FindSpawnPositions RollingBallSpawner;
    public FindSpawnPositions splitterSpawner;
    public float spawnInterval = 10f;
    public float riseHeight = 1f;
    public float riseDuration = 1f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSecondsRealtime(1f);

        while (true)
        {
            SpawnOne();
            yield return new WaitForSecondsRealtime(spawnInterval);
        }
    }

    private void SpawnOne()
    {
        spawner.SpawnAmount = 1;
        spawner.StartSpawn();
        spawner.SpawnAmount = 0;

        RollingBallSpawner.SpawnAmount = 1;
        RollingBallSpawner.StartSpawn();
        RollingBallSpawner.SpawnAmount = 0;
		
        splitterSpawner.SpawnAmount = 1;
        splitterSpawner.StartSpawn();
        splitterSpawner.SpawnAmount = 0;

        if (spawner.transform.childCount > 0)
        {
            Transform spawned = spawner.transform.GetChild(spawner.transform.childCount - 1);
            StartCoroutine(RiseFromFloor(spawned.gameObject));
        }

        if (RollingBallSpawner.transform.childCount > 0)
        {

            Transform spawned = RollingBallSpawner.transform.GetChild(RollingBallSpawner.transform.childCount - 1);
            StartCoroutine(RiseFromFloor(spawned.gameObject));
        }
		
        if (splitterSpawner.transform.childCount > 0)
        {
            Transform spawned = splitterSpawner.transform.GetChild(splitterSpawner.transform.childCount - 1);
            StartCoroutine(RiseFromFloor(spawned.gameObject));
        }
    }

    private IEnumerator RiseFromFloor(GameObject obj)
    {
        Vector3 startPos = obj.transform.position - 1.1f * Vector3.up * riseHeight;
        obj.transform.position = startPos;
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

using UnityEngine;

public class TestScript : MonoBehaviour
{
	public GameObject fireballPrefab;
	
	public void OnGesturePerformed()
	{
		for(int i = 0; i < 10; i++) Instantiate(fireballPrefab, new Vector3(0f, i * 0.1f, 0f), Quaternion.identity);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningLines : MonoBehaviour {
	public int points = 33;
	
	LineRenderer lr;
	Vector3 start, toEnd;
	
	void Start() {
		lr = gameObject.GetComponent<LineRenderer>();
		
		start = lr.GetPosition(0);
		toEnd = (lr.GetPosition(1) - start).normalized;
		
		lr.positionCount = points;
	}
	
	void Update() {
		toEnd = gameObject.transform.rotation * Vector3.forward;
		start = gameObject.transform.position;
		
		Vector3 point = start;
		for(int i = 0; i < points; i++) {
			lr.SetPosition(i, point);
			
			point = start + (5f / points) * (i * toEnd) + (new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1))).normalized * Random.Range(0.01f, 0.3f);
		}
	}
}

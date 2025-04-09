using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour {
	public GameObject bulletPrefab;
	
	//Start is called before the first frame update
	void Start() {
	}

	//Update is called once per frame
	//https://developers.meta.com/horizon/documentation/unity/unity-tutorial-basic-controller-input#reference-script
	void Update() {
		if(OVRInput.Get(OVRInput.Button.Two)) {
			Instantiate(bulletPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		}
	}
}

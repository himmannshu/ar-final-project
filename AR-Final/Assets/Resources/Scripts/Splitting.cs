using System.Collections;
using UnityEngine;

public class Splitting : DeathEffect {
	public int splitsRemaining = 2;
	//public EnemyDamage eds;
	
	void Start() {
		//eds.splitEvent.AddListener(go);
	}
	
	public void setSplitsRemaining(int x) {
		splitsRemaining = x;
	}

	public override void go() {
		if(splitsRemaining-- <= 0) return;
		
		StartCoroutine(split());
	}
	
	IEnumerator split() {
		yield return new WaitForSeconds(1);
		
		Vector3 leftward = (gameObject.transform.localRotation * Vector3.forward).normalized;
		GameObject left  = Instantiate(gameObject, gameObject.transform.localPosition +  0.15f * leftward, gameObject.transform.localRotation);
		GameObject right = Instantiate(gameObject, gameObject.transform.localPosition + -0.15f * leftward, gameObject.transform.localRotation);
		left.GetComponent<Splitting>().setSplitsRemaining(splitsRemaining);
		right.GetComponent<Splitting>().setSplitsRemaining(splitsRemaining);
	}
	
	/*
	public void go(int x) {
		splitsRemaining = x - 1;
		if(splitsRemaining <= 0) return;
		
		Vector3 leftward = (gameObject.transform.localRotation * Vector3.forward).normalized;
		GameObject left  = Instantiate(gameObject, gameObject.transform.localPosition +  0.15f * leftward, gameObject.transform.localRotation);
		GameObject right = Instantiate(gameObject, gameObject.transform.localPosition + -0.15f * leftward, gameObject.transform.localRotation);
		left.GetComponent<Splitting>().setSplitsRemaining(splitsRemaining);
		right.GetComponent<Splitting>().setSplitsRemaining(splitsRemaining);
	}
	*/
}

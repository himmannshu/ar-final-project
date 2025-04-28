using System.Collections;
using UnityEngine;

public class Splitting : DeathEffect {
	public int splitsRemaining = 2;
	
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
}

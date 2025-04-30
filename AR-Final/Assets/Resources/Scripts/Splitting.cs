using System.Collections;
using UnityEngine;

public class Splitting : DeathEffect {
	public int splitsRemaining = 1;
	
	public void setSplitsRemaining(int x) {
		splitsRemaining = x;
	}

	public override void go() {
		if(splitsRemaining-- <= 0) return;
		
		Vector3 leftward = (gameObject.transform.rotation * Vector3.left).normalized;
		GameObject left  = Instantiate(gameObject, gameObject.transform.position +  0.5f * leftward, gameObject.transform.rotation);
		GameObject right = Instantiate(gameObject, gameObject.transform.position + -0.5f * leftward, gameObject.transform.rotation);
		left.GetComponent<Splitting>().setSplitsRemaining(splitsRemaining);
		right.GetComponent<Splitting>().setSplitsRemaining(splitsRemaining);
	}
}

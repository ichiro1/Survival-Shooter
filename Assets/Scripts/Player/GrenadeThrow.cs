using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour {
	public float throwForce = 10f;
	public GameObject grenade;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("g")) {
			ThrowGrenade ();
		}
	}
	void ThrowGrenade() {
		GameObject grenadethrow = Instantiate (grenade, transform.position, transform.rotation);
			
		grenadethrow.GetComponent<Exploder> ().init();
			Rigidbody rb = grenadethrow.GetComponent<Rigidbody> ();
			rb.AddForce (transform.forward * throwForce, ForceMode.VelocityChange);

	}
}

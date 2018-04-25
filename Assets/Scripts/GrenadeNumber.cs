using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrenadeNumber : MonoBehaviour {
	public int i = 3;

	public GrenadeThrow grenadeThrow;


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("g")&& i >0) {
			i -= 1;
			grenadeThrow.ThrowGrenade ();
		}

	}
}

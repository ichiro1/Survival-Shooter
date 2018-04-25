using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeCount : MonoBehaviour {
	public Text count;
	public GrenadeNumber grenadeNumber;
	// Use this for initialization
	void Start () {
		count = GetComponent<Text> ();

	}
	
	// Update is called once per frame
	void Update () {
		
		count.text = grenadeNumber.i.ToString() + " Grenade(s)";
		if (grenadeNumber.i == 0) {
			count.color = Color.red;
		}
	}
}

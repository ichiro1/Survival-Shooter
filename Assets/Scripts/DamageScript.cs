using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour {
	public int DamagePerGrenade = 300;
	public float timeBetweenBullets = 0.15f;        // The time between each shot.
	public float range = 100f;                      // The distance the gun can fire.

	float timer;                                    // A timer to determine when to fire.
	Ray shootRay;                                   // A ray from the gun end forwards.
	RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
	int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
	ParticleSystem gunParticles;                    // Reference to the particle system.
	LineRenderer gunLine;                           // Reference to the line renderer.
	AudioSource gunAudio;                           // Reference to the audio source.
	Light gunLight;                                 // Reference to the light component.
	float effectsDisplayTime = 0.2f;
	 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	
	if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
	{
		// Try and find an EnemyHealth script on the gameobject hit.
		EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();

		// If the EnemyHealth component exist...
		if(enemyHealth != null)
		{
			// ... the enemy should take damage.
			enemyHealth.TakeDamage (DamagePerGrenade, shootHit.point);
		}

		// Set the second position of the line renderer to the point the raycast hit.
		gunLine.SetPosition (1, shootHit.point);
	}
	}
}

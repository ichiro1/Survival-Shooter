using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Exploder : MonoBehaviour {

	public float explosionTime = 0;
	public float randomizeExplosionTime = 0;
	public float radius = 15;
	public float power = 1;
	public int probeCount = 150;
	public float explodeDuration = 0.5f;

	private float timeTillExplosion;

	protected bool exploded = false;

	protected bool wasTrigger;
	public virtual void disableCollider() {
		if (GetComponent<Collider>()) {
			wasTrigger = GetComponent<Collider>().isTrigger;
			GetComponent<Collider>().isTrigger = true;
		}
	}

	public virtual void enableCollider() {
		if (GetComponent<Collider>()) {
			GetComponent<Collider>().isTrigger = wasTrigger;
		}
	}

	
	public virtual void init() {
		power *= 500000;
		exploded = false;
		if (randomizeExplosionTime > 0.01f) {
			explosionTime += Random.Range(0.0f, randomizeExplosionTime);
		}

		timeTillExplosion = explosionTime;
	}
	Collider capsule;
	void Start() {
		init();
		capsule = GetComponent<Collider> ();
	}
	void Update()
	{
		

	}
	void FixedUpdate() {

		CountDownTimeToExplosion ();


		if (timeTillExplosion <= 0.0f && !exploded) {
			
			exploded = true;
			Debug.Log ("boolean exploded: " + exploded);
			StartCoroutine("explode");
			Destroy (gameObject, 30f);
		}
	}

	private void CountDownTimeToExplosion()
	{
		timeTillExplosion -= Time.deltaTime;
	}

	public virtual IEnumerator explode() {
		Debug.Log ("Boom!");

		ExploderComponent[] components = GetComponents<ExploderComponent>(); 
		foreach (ExploderComponent component in components) {
			if (component.enabled) {
				component.onExplosionStarted(this);
			}
		}		
		while (explodeDuration > Time.time - explosionTime) {
			disableCollider();
			for (int i = 0; i < probeCount; i++) {
				shootFromCurrentPosition();
			}
			enableCollider();
			yield return new WaitForFixedUpdate();
		}
	}

	protected virtual void shootFromCurrentPosition() {
		Vector3 probeDir = Random.onUnitSphere;
		Ray testRay = new Ray(transform.position, probeDir);
		shootRay(testRay, radius);
	}

	private void shootRay(Ray testRay, float estimatedRadius) {
		RaycastHit hit;
		if (Physics.Raycast(testRay, out hit, estimatedRadius)) {
			if (hit.rigidbody != null) {
				hit.rigidbody.AddForceAtPosition(power * Time.deltaTime * testRay.direction / probeCount, hit.point);
				estimatedRadius /= 2;
			} else {
				Vector3 reflectVec = Random.onUnitSphere;
				if (Vector3.Dot(reflectVec, hit.normal) < 0) {
					reflectVec *= -1;
				}
				Ray emittedRay = new Ray(hit.point, reflectVec);
				shootRay(emittedRay, estimatedRadius - hit.distance);
			}
		}
	}

	void Damage() {
		
	}
}

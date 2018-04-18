using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Exploder : MonoBehaviour {

	Vector3 velocity = new Vector3 (0.0f,1.0f,0.0f);
	public float explosionTime = 0;
	public float randomizeExplosionTime = 0;
	public float radius = 15;
	public float power = 1;
	public int probeCount = 150;
	public float explodeDuration = 0.5f;
	public float sinkSpeed = 2.5f;

	public LayerMask hitLayers;

	private float timeTillExplosion;

	protected bool exploded = false;
	bool isSinking;

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
		DamageEnemiesInSphere ();
		disappear ();

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

	public void DamageEnemiesInSphere()
	{
		RaycastHit[] rayColliders = Physics.SphereCastAll (transform.position, 10.0f, Vector3.up, hitLayers.value);
		for (int i = 0; i < rayColliders.Length; i++) 
		{
			GameObject hitObject = rayColliders [i].collider.gameObject;
			if (hitObject.GetComponent<EnemyHealth> ()) 
			{
				hitObject.GetComponent<EnemyHealth> ().TakeDamage (100, hitObject.transform.position);
			}

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
	public void disappear () {
		MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer> ();
		for(int i = 0; i< renderers.Length; i++) {
			renderers [i].enabled = false;
		}
	}
}

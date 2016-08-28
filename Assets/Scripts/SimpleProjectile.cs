using UnityEngine;
using System.Collections;
using System;

public class SimpleProjectile : Projectile {
	public ParticleSystem tail;
	Vector2 velocity;
	float angularVelocity;

	public override void Launch ( Vector2 initialVelocity, float angularVelocity = 0f) {
		velocity = initialVelocity;
		this.angularVelocity = angularVelocity;
		StartCoroutine (TailRoutine());
	}

	IEnumerator TailRoutine() {
		yield return null;
		yield return null;
		yield return null;
		var emmision = tail.emission;
		emmision.enabled = true;
		tail.startLifetime = 32f / Mathf.Max( Mathf.Abs( velocity.x ), Mathf.Abs( velocity.y ) );
	}

	protected override void Update() {
		base.Update();
		if (angularVelocity != 0f) {
			velocity = Quaternion.Euler (0, 0, angularVelocity * Time.deltaTime) * velocity;
		}
		MoveTo( transform.position + ( Vector3 ) velocity * Time.deltaTime, false );
	}

	public override void Reflect ( Collider2D reflector ) {
		reflected = true;
		velocity = ( transform.position - reflector.transform.position ).normalized * velocity.magnitude;
		Engine.HitPause();
		/*Vector2 n = ( reflector.transform.position - transform.position ).normalized;
		float d = Vector2.Dot( n, speed );
		speed = new Vector2( speed.x - ( 2 * d * n.x ), speed.y - ( 2 * d * n.y ) );*/
	}

	public override void OnPool () {
		tail.Clear ();
		var emmision = tail.emission;
		emmision.enabled = false;
	}
}

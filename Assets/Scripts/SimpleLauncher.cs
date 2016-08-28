using UnityEngine;
using System.Collections;

public class SimpleLauncher : MonoBehaviour {
	public Projectile projectilePrefab;
	public float startup, cooldown, speed = 32, turn = 0;

	void Start () {
		StartCoroutine( RunRoutine() );
	}
	
	IEnumerator RunRoutine() {
		yield return new WaitForSeconds( startup );

		while ( true ) {
			var projectile = Pool.Get<Projectile> (projectilePrefab);
			projectile.transform.position = transform.position;
			projectile.Launch( transform.TransformDirection( Vector2.right * speed ), turn );

			yield return new WaitForSeconds( cooldown );
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere( transform.position, 8f );
		Gizmos.DrawLine( transform.position, transform.position + ( transform.TransformDirection( Vector2.right ) * speed ) );
	}
}

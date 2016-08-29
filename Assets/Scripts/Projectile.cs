using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour, Poolable {
	public abstract void Launch ( Vector2 initialVelocity, float angularVelocity = 0f );
	public abstract void Reflect ( Collider2D reflector );
	public float radius = 32f;

	[System.NonSerialized]
	public bool reflected;

	private static readonly RaycastHit2D[] _circleCastArray = new RaycastHit2D[ 10 ];

	#region Poolable implementation

	public virtual void OnPool () {
		
	}

	public virtual void OnSpawn () {
		reflected = false;
	}

	#endregion

	public virtual void WallCollision () {
		Pool.PoolSelf( this );
	}

	protected virtual void Update() {
		int collisions = MoveTo( ( Vector2 ) transform.position, true );

		if ( !reflected ) {
			for ( int i = 0; i < collisions; i++ ) {
				if ( _circleCastArray[ i ].collider.gameObject.layer == Constants.RelectorLayer ) {
					Reflect( _circleCastArray[ i ].collider );
				}
			}
		} else {
			for ( int i = 0; i < collisions; i++ ) {
				if ( _circleCastArray[ i ].collider.gameObject.layer == Constants.EnemyLayer ) {
					_circleCastArray[ i ].collider.gameObject.GetComponent<BossScarab>().TakeDamage();
                    WallCollision();
					return;
				}
			}
		}

		if ( !reflected ) {
			for ( int i = 0; i < collisions; i++ ) {
				if ( _circleCastArray[ i ].collider.gameObject.layer == Constants.PlayerLayer ) {
					Engine.KillPlayer ();
				}
			}
		}


		for ( int i = 0; i < collisions; i++ ) {
			if ( _circleCastArray[ i ].collider.gameObject.layer == Constants.CollisionLayer ) {
				WallCollision();
			}
		}
	}

	public virtual int MoveTo( Vector2 vector, bool teleport ) {
		transform.position = vector;
		if ( teleport ) {
			return Physics2D.CircleCastNonAlloc( transform.position, radius, Vector2.zero, _circleCastArray );
		} else {
			return Physics2D.CircleCastNonAlloc( transform.position, radius, vector - ( Vector2 ) transform.position, _circleCastArray, ( vector - ( Vector2 ) transform.position ).magnitude );
		}
	}

	protected virtual void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere( transform.position, radius );
	}
}

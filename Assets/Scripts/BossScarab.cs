using UnityEngine;
using System.Collections;

public class BossScarab : MonoBehaviour {
	public int health = 10;
	public Beetle beetlePrefab;
	public Animator animator;
	public AudioClip hurtSound;

	public void TakeDamage() {
		health--;
		Engine.PlaySound( hurtSound );
		if ( health == 0 ) {
			var beetle = Instantiate<Beetle>( beetlePrefab );
			beetle.transform.position = transform.position;
			animator.SetTrigger( "death" );
		} else {
			animator.SetTrigger( "hurt" );
		}
	}
}

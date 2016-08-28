using UnityEngine;
using System.Collections;

public class Beetle : MonoBehaviour {
	public AudioClip collectSound;
	public bool continueTrigger;
	void OnTriggerEnter2D ( Collider2D c ) {
		if (continueTrigger) {
			Engine.Continue ();
		} else {
			Engine.NextLevel ();
		}
		GetComponent<Collider2D>().enabled = false;
		Engine.PlaySound( collectSound );
	}
}

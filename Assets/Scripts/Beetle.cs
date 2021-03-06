﻿using UnityEngine;
using System.Collections;

public class Beetle : MonoBehaviour {
	public AudioClip collectSound;
	public bool continueTrigger;
	public bool reflectTrigger;
	void OnTriggerEnter2D ( Collider2D c ) {
		if ( continueTrigger ) {
			Engine.Continue();
		} else if ( reflectTrigger ) {
			Engine.AllowReflect();
		} else {
			Engine.NextLevel();
		}
		GetComponent<Collider2D>().enabled = false;
		Engine.PlaySound( collectSound );

		if ( reflectTrigger ) {
			var emission = GetComponentInChildren<ParticleSystem>().emission;
			emission.enabled = false;
		}
	}
}

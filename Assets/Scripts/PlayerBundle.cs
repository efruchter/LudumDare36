using UnityEngine;
using System.Collections;

public class PlayerBundle : MonoBehaviour {
	public PlayerMovement player;
	public ParticleSystem deathParticles;

	public void OnSpawn() {
		player.gameObject.SetActive (true);
		player.OnRespawn ();
	}

	public void OnDeath() {
		player.OnDeath ();
		deathParticles.transform.position = player.transform.position;
		deathParticles.Emit (8);
	}
}

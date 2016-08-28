using UnityEngine;
using System.Collections;

public class GroundSensor : MonoBehaviour {
	[SerializeField]
	private LayerMask canJumpOn;
	public bool OnGround { private set; get; }
	
	// Update is called once per frame
	void FixedUpdate () {
		OnGround = false;
	}

	void OnTriggerStay2D(Collider2D collider) {
		OnGround = OnGround || (((1 << collider.gameObject.layer) & canJumpOn.value) != 0);
	}
}

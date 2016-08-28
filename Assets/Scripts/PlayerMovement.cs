using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public Rigidbody2D body;
	public GroundSensor jumpSensor;
	public GameObject reflector;
	public Animator animator;
	public SpriteMirror spriteMirror;
	public ParticleSystem featherParticle;
	public AudioClip jumpClip, deathClip;

	Collider2D[] colliders;

	float maxJumpTime = 2f, maxReflectTime = 0.2f;
	float jumpTimeRemaining;
	bool jumping, reflecting, frozen;
	bool wasOnGround;
	public bool Alive {private set; get;}

	public bool AllowControl {
		set {
			frozen = !value;
		}
	}

	void Awake () {
		Application.targetFrameRate = 60;
		reflector.SetActive( false );
		colliders = GetComponents<Collider2D> ();
	}

	public void OnRespawn() {
		frozen = false;
		jumping = false;
		reflecting = false;
		Alive = true;

		body.isKinematic = true;
		transform.position = ( Vector2 ) GameObject.FindObjectOfType<StartingPosition>().transform.position + new Vector2(0, 8);
		body.isKinematic = false;
		body.velocity = Vector2.zero;

		for (int i = 0; i < colliders.Length; i++) {
			colliders [i].enabled = true;
		}

		animator.SetBool ("dead", false);
	}

	public void OnDeath() {
		StopAllCoroutines ();

		Alive = false;
		frozen = true;
		jumping = false;
		reflecting = false;

		reflector.SetActive( false );
		animator.SetBool ("dead", true);

		body.velocity = new Vector2 (0f, 128);

		Engine.PlaySound( deathClip );
	}

	public void OnBeetle () {
		StopAllCoroutines();

		Alive = false;
		frozen = true;
		jumping = false;
		reflecting = false;

		reflector.SetActive( false );
		body.velocity = Vector2.zero;
	}

	// Update is called once per frame
	void Update () {
		if ( frozen || !Alive ) {
			return;
		}

		if ( !jumping && !reflecting ) {
			body.velocity = new Vector2( Input.GetAxisRaw( "Horizontal" ) * ( 10f * 5f ), body.velocity.y );
			animator.SetBool( "walking", jumpSensor.OnGround && ( Mathf.Abs( Input.GetAxisRaw( "Horizontal" ) ) > 0.01f ) && !jumping );
		}

		if ( !wasOnGround && jumpSensor.OnGround ) {
			animator.SetTrigger( "landing" );
		}

		if (jumpSensor.OnGround) {
			jumpTimeRemaining = maxJumpTime;
		}

		if ( Mathf.Abs( Input.GetAxisRaw( "Horizontal" ) ) > 0.01f ) {
			spriteMirror.SetFacing( Input.GetAxisRaw( "Horizontal" ) > 0 );
		}

		/*if ( Input.GetButtonDown( "Fire2" ) && !reflecting ) {
			StartCoroutine( ReflectRoutine() );
		}*/

		if ( Input.GetButtonDown( "Jump" ) && !jumping ) {
			StartCoroutine( JumpRoutine() );
		}

		wasOnGround = jumpSensor.OnGround;
	}

	IEnumerator ReflectRoutine () {
		reflecting = true;
		reflector.SetActive( true );
		float reflectTimeRemaining = maxReflectTime;
		while ( reflectTimeRemaining >= 0 ) {
			reflectTimeRemaining -= Time.deltaTime;
			body.velocity = Vector2.zero;
			yield return null;
		}
		reflecting = false;
		reflector.SetActive( false );
	}

	IEnumerator JumpRoutine () {
		Engine.PlaySound( jumpClip, Mathf.Clamp01( jumpTimeRemaining / maxJumpTime ) );
		featherParticle.startColor = Color.Lerp (Color.grey, Color.white, Mathf.Clamp01 (jumpTimeRemaining / maxJumpTime)); 
		animator.SetTrigger( "jump" );
		featherParticle.Emit( 1 );
		jumping = true;
		animator.SetBool( "jumping", true );
		while ( jumpTimeRemaining >= 0 ) {
			jumpTimeRemaining -= Time.deltaTime;
			Vector2 controlDelta = new Vector2( Input.GetAxisRaw( "Horizontal" ), Input.GetAxisRaw( "Vertical" ) );
			body.velocity = controlDelta * 150f * Mathf.Clamp01( jumpTimeRemaining / maxJumpTime );
			if ( !Input.GetButton( "Jump" ) || controlDelta.sqrMagnitude < 0.01f ) {
				break;
			}
			yield return null;
		}
		jumping = false;
		animator.SetBool( "jumping", false );
	}
}

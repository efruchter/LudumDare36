using UnityEngine;
using System.Collections;

public class SpriteMirror : MonoBehaviour {
	public bool startsRight = true;

	bool facingRight;

	void Awake() {
		facingRight = startsRight;
	}

	public void SetFacing ( bool facingRight ) {
		if (this.facingRight == facingRight) {
			return;
		}

		this.facingRight = facingRight;

		transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
	}
}

using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private Vector2 virtualPosition, offset;

	[Range(0,1)]
	public float alpha = 1;

	public Material material;

	void LateUpdate () {
		offset = Vector2.Lerp( offset, Vector2.zero, Time.unscaledDeltaTime );
		virtualPosition = Vector2.Lerp (virtualPosition, Engine.Player.transform.position + new Vector3(0f, -8f, 0f), Time.unscaledDeltaTime * 10f);
		transform.position = new Vector3(
			virtualPosition.x + offset.x,
			virtualPosition.y + offset.y,
			-200f );
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (alpha == 1)
		{
			Graphics.Blit (source, destination);
			return;
		}

		material.SetFloat ("_Alpha", Mathf.Clamp01(alpha));
		Graphics.Blit (source, destination, material);
	}

	public void Shake () {
		StartCoroutine( ShakeRoutine() );
	}

	IEnumerator ShakeRoutine() {
		for (int i = 0; i < 7; i++) {
			offset = Random.insideUnitCircle * 4;
			yield return new WaitForSecondsRealtime( 1f / 30f );
		}
	}
}

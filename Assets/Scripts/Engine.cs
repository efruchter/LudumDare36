using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Engine : MonoBehaviour {
	private static Engine I;

	public static bool Exists {
		get {
			return I != null;
		}
	}

	public static PlayerMovement Player {
		get {
			return I.playerBundle.player;
		}
	}

	public PlayerBundle playerBundle;
	public CameraController cameraController;
	public AudioSource oneShotPlayer;

	void Awake () {
		I = this;
		DontDestroyOnLoad( gameObject );
		Application.targetFrameRate = 60;
	}

	void Start () {
		StartCoroutine( I.OnSpawnRoutine() );
	}

	void Update () {
		if ( Input.GetKeyDown( KeyCode.Escape ) ) {
			Application.Quit();
		}
	}

	IEnumerator OnSpawnRoutine () {
		cameraController.alpha = 0;
		I.playerBundle.OnSpawn();
		I.playerBundle.player.gameObject.SetActive( true );
		I.playerBundle.player.AllowControl = false;
		cameraController.alpha = 0;
		float countDown = 0.25f;
		while ( ( countDown -= Time.unscaledDeltaTime ) > 0 ) {
			yield return null;
			cameraController.alpha = 1f - ( countDown / 0.25f );
		}
		cameraController.alpha = 1;
		I.playerBundle.player.AllowControl = true;
	}

	public static void HitPause () {
		I.StartCoroutine( I.HitPauseRoutine() );
	}

	IEnumerator HitPauseRoutine () {
		Time.timeScale = 0;
		yield return new WaitForSecondsRealtime( 4f / 60f );
		Time.timeScale = 1;
	}

	public static void KillPlayer () {
		if ( I.playerBundle.player.Alive ) {
			I.StartCoroutine( I.KillPlayerRoutine() );
		}
	}

	IEnumerator KillPlayerRoutine () {
		HitPause();
		cameraController.Shake();
		I.playerBundle.OnDeath();
		yield return new WaitForSecondsRealtime( 1 );
		cameraController.alpha = 1;
		float countDown = 0.25f;
		while ( ( countDown -= Time.unscaledDeltaTime ) > 0 ) {
			yield return null;
			cameraController.alpha = countDown / 0.25f;
		}
		cameraController.alpha = 0;
		StartCoroutine( I.OnSpawnRoutine() );
	}

	public static void NextLevel () {
		int level = SceneManager.GetActiveScene ().buildIndex + 1;
		I.StartCoroutine (I.NextLevelRoutine (level));
		PlayerPrefs.SetInt ("level", level);
	}

	public static void Continue() {
		I.StartCoroutine( I.NextLevelRoutine(PlayerPrefs.GetInt("level", SceneManager.GetActiveScene().buildIndex + 1)));
	}

	IEnumerator NextLevelRoutine (int level) {
		I.playerBundle.player.OnBeetle();

		yield return new WaitForSecondsRealtime( 1 );
		cameraController.alpha = 1;
		float countDown = 0.25f;
		while ( ( countDown -= Time.unscaledDeltaTime ) > 0 ) {
			yield return null;
			cameraController.alpha = countDown / 0.25f;
		}
		cameraController.alpha = 0;

		I.playerBundle.player.gameObject.SetActive( false );

		SceneManager.LoadScene(level , LoadSceneMode.Single );
		yield return null;

		PlayerPrefs.SetInt( "level", SceneManager.GetActiveScene().buildIndex );

		Resources.UnloadUnusedAssets();
		System.GC.Collect();

		StartCoroutine( I.OnSpawnRoutine() );
	}

	public static void PlaySound ( AudioClip clip, float volume = 1f ) {
		I.oneShotPlayer.PlayOneShot( clip, volume );
	}
}

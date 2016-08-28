using UnityEngine;
using System.Collections;

public class StartingPosition : MonoBehaviour {
	public Engine enginePrefab;
	void Awake() {
		if (!Engine.Exists) {
			Instantiate( enginePrefab );
		}
	}
}

using UnityEngine;
using System.Collections;

public class ARControllerButtons : MonoBehaviour {

	private ARController myARController;
	private ARMarker[] markers;

	// Use this for initialization
	public void onClickPlay () {
		markers = FindObjectsOfType(typeof(ARMarker)) as ARMarker[];
		foreach (ARMarker m in markers) {
			m.OnEnable();
		}
	}

	public void onClickStop () {
		markers = FindObjectsOfType(typeof(ARMarker)) as ARMarker[];
		foreach (ARMarker m in markers) {
			m.OnDisable();
		}
	}
}

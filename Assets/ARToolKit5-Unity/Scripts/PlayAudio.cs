using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public class PlayAudio : MonoBehaviour {

	private AROrigin arOrigin;

	private AudioSource audio1;
	private AudioSource audio2;

	private ARMarker[] markers;

	public String[] markersTags;
	public GameObject[] markersFX;
	public GameObject FXCamera;

	private GameObject myARObject;
	private ARController myARController;

	Thread oThread;

	private int baseDelay = 1500;
	private int Delay = 1500;

	private bool playAudio = false;
	private bool firstPlayAudio = false;

	// Use this for initialization
	void Start () {
		arOrigin = this.gameObject.GetComponentInParent<AROrigin>();
		audio1 = GameObject.FindGameObjectWithTag("arAudio1").GetComponent<AudioSource>();
		audio2 = GameObject.FindGameObjectWithTag("arAudio2").GetComponent<AudioSource>();


		markers = FindObjectsOfType(typeof(ARMarker)) as ARMarker[];
		foreach (ARMarker m in markers) {
			m.OnDisable ();
		}

		myARController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ARController>();
		FXCamera.SetActive (false);
	}

	public void activateCamera(){
		FXCamera.SetActive (true);
	}

	//These are our new methods
	void OnMarkerFound(ARMarker marker){

		AudioSource theAudio = null; 
		if (marker.Tag == markersTags[0]) {
			audio1.loop = true;
			theAudio = audio1;
		} else if (marker.Tag == markersTags[1]) {
			audio2.loop = true;
			theAudio = audio2;
		} else if (marker.Tag == markersTags[2]) {
			audio1.GetComponent<SEF_lowpass> ().enabled = true;
			audio2.GetComponent<SEF_lowpass> ().enabled = true;
		} else if (marker.Tag == markersTags[3]) {
			audio1.GetComponent<SEF_highpass> ().enabled = true;
			audio2.GetComponent<SEF_highpass> ().enabled = true;
		}

		if (theAudio != null)
			theAudio.Play ();

	}

	void OnMarkerLost(ARMarker marker){
		if (marker.Tag == markersTags[0]) {
			markersFX[0].SetActive (false);
			audio1.Pause();
		} else if (marker.Tag == markersTags[1]) {
			markersFX[1].SetActive (false);
			audio2.Pause();
		} else if (marker.Tag == markersTags[2]) {
			audio1.GetComponent<SEF_lowpass> ().enabled = false;
			audio2.GetComponent<SEF_lowpass> ().enabled = false;
		} else if (marker.Tag == markersTags[3]) {
			audio1.GetComponent<SEF_highpass> ().enabled = false;
			audio2.GetComponent<SEF_highpass> ().enabled = false;
		}
			
	}

	void OnMarkerTracked(ARMarker marker){
			
		Vector3 positionTarget = ARUtilityFunctions.PositionFromMatrix(marker.TransformationMatrix);

		Vector3 normalPosition = new Vector3 (positionTarget.x / (0.5f * positionTarget.z), positionTarget.y / (0.5f * positionTarget.z), markersFX[0].transform.localPosition.z);

		int lowCutoffvalue = (int)(((normalPosition.x + 0.9) / 1.8) * 4900) + 100;
		int highCutoffvalue = (int)(((normalPosition.x + 0.9) / 1.8) * 1495) + 5;
		AudioSource theAudio = null; 
		if (marker.Tag == markersTags[0]) {
			theAudio = audio1;
			markersFX[0].SetActive (true);
			markersFX[0].transform.localPosition = normalPosition;
		} else if (marker.Tag == markersTags[1]) {
			theAudio = audio2;
			markersFX[1].SetActive (true);
			markersFX[1].transform.localPosition = normalPosition;
		} else if (marker.Tag == markersTags[2]) {
			audio1.GetComponent<SEF_lowpass> ().cutoffFrequency = lowCutoffvalue;
			audio2.GetComponent<SEF_lowpass> ().cutoffFrequency = lowCutoffvalue;
		} else if (marker.Tag == markersTags[3]) {
			audio1.GetComponent<SEF_highpass> ().cutoffFrequency = highCutoffvalue;
			audio2.GetComponent<SEF_highpass> ().cutoffFrequency = highCutoffvalue;
		}



		if (theAudio != null) {
			
			if (normalPosition.x > -0.3f && normalPosition.x < 0.3f) {
				theAudio.pitch = 1f;
			} else if (normalPosition.x > -0.9f && normalPosition.x < -0.3f) {
				theAudio.pitch = 0.7f;
			} else if (normalPosition.x > 0.3f && normalPosition.x < 0.9f) {
				theAudio.pitch = 1.4f;
			}

			if (normalPosition.y > -0.2f && normalPosition.y < 0.2f) {
				int a = 0;
			} else if (normalPosition.y > -0.6f && normalPosition.y < -0.2f) {
				int a = 0;
			} else if (normalPosition.y > 0.2f && normalPosition.y < 0.6f) {
				int a = 0;
			}
		}

	}




	// Update is called once per frame
	void Update () {   
	}
		
		
}



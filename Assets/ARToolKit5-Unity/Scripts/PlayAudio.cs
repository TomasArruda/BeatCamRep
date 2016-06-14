using UnityEngine;
using System.Collections;
using System;
using System.Threading;

public class PlayAudio : MonoBehaviour {

	private AROrigin arOrigin;

	private AudioSource audio1;
	private AudioSource audio2;
	private AudioSource audio3;
	private AudioSource audio4;
	private AudioSource audio5;
	private AudioSource audio6;

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
		audio3 = GameObject.FindGameObjectWithTag("arAudio3").GetComponent<AudioSource>();
		audio4 = GameObject.FindGameObjectWithTag("arAudio4").GetComponent<AudioSource>();
		audio5 = GameObject.FindGameObjectWithTag("arAudio5").GetComponent<AudioSource>();
		audio6 = GameObject.FindGameObjectWithTag("arAudio6").GetComponent<AudioSource>();

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
			audio5.loop = true;
			theAudio = audio5;
		} else if (marker.Tag == markersTags[1]) {
			audio5.loop = true;
			theAudio = audio6;
		} else if (marker.Tag == markersTags[2]) {
			audio3.loop = true;
			theAudio = audio3;
		} else if (marker.Tag == markersTags[3]) {
			audio4.loop = true;
			theAudio = audio4;
		}

		//firstPlayAudio = true;
		theAudio.Play ();

		//oThread = new Thread(new ThreadStart(() => baseAudioDelay(Delay + delay)));

		///// teste /////
		//theAudio.UnPause();
		//playAudio = false;
		//firstPlayAudio = false;
		////////

		//oThread.Start();
		//while (!oThread.IsAlive);
	}

	void OnMarkerLost(ARMarker marker){
		if (marker.Tag == markersTags[0]) {
			markersFX[0].SetActive (false);
			audio5.Pause();
		} else if (marker.Tag == markersTags[1]) {
			markersFX[1].SetActive (false);
			audio6.Pause();
		} else if (marker.Tag == markersTags[2]) {
			markersFX[2].SetActive (false);
			audio3.Pause();
		} else if (marker.Tag == markersTags[3]) {
			markersFX[3].SetActive (false);
			audio4.Pause();
		}

		//if (oThread != null) {
			//oThread.Abort ();
			//while (oThread.IsAlive);
		//}
	}

	void OnMarkerTracked(ARMarker marker){
			
		Vector3 positionTarget = ARUtilityFunctions.PositionFromMatrix(marker.TransformationMatrix);

		Vector3 normalPosition = new Vector3 (positionTarget.x / (0.5f * positionTarget.z), positionTarget.y / (0.5f * positionTarget.z), markersFX[0].transform.localPosition.z);


		AudioSource theAudio = null; 
		if (marker.Tag == markersTags[0]) {
			theAudio = audio5;
			markersFX[0].SetActive (true);
			markersFX[0].transform.localPosition = normalPosition;
		} else if (marker.Tag == markersTags[1]) {
			theAudio = audio6;
			markersFX[1].SetActive (true);
			markersFX[1].transform.localPosition = normalPosition;
		} else if (marker.Tag == markersTags[2]) {
			markersFX[2].SetActive (true);
			markersFX[2].transform.localPosition = normalPosition;
		} else if (marker.Tag == markersTags[3]) {
			markersFX[3].SetActive (true);
			markersFX[3].transform.localPosition = normalPosition;
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

	public void baseAudioDelay(int firstDelay){
		while (true) {
			if (firstPlayAudio) {
				wait(firstDelay);
				firstPlayAudio = false;
				playAudio = true;
			}
			wait(Delay);
			playAudio = true;
		}
	}

	private static void wait(int milliseconds)
	{
		DateTime dt = DateTime.Now + TimeSpan.FromMilliseconds(milliseconds);

		do { } while (DateTime.Now < dt);
	}

	private int calcDelay(float pos, float max)
	{
		return (int)Math.Round(((Delay)*pos)/max);
	}	
		
}



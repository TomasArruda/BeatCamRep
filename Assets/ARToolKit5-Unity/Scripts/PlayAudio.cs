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

	private ARMarker[] markers;

	private GameObject myARObject;
	private ARController myARController;

	Thread oThread;

	private int baseDelay = 1800;
	private bool playAudio = false;
	private bool firstPlayAudio = false;

	// Use this for initialization
	void Start () {
		arOrigin = this.gameObject.GetComponentInParent<AROrigin>();
		audio1 = GameObject.FindGameObjectWithTag("arAudio1").GetComponent<AudioSource>();
		audio2 = GameObject.FindGameObjectWithTag("arAudio2").GetComponent<AudioSource>();
		audio3 = GameObject.FindGameObjectWithTag("arAudio3").GetComponent<AudioSource>();
		audio4 = GameObject.FindGameObjectWithTag("arAudio4").GetComponent<AudioSource>();

		markers = FindObjectsOfType(typeof(ARMarker)) as ARMarker[];
		foreach (ARMarker m in markers) {
			m.OnDisable ();
		}

		myARController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ARController>();
	}

	//These are our new methods
	void OnMarkerFound(ARMarker marker){
		Vector3 positionTarget = ARUtilityFunctions.PositionFromMatrix(marker.TransformationMatrix);

		float positionMax = positionTarget.z;
		float positionSum = positionMax / 2;
		float position = positionTarget.x + positionSum;


		int delay = calcDelay (position, positionMax);

		AudioSource theAudio = null; 
		if (marker.Tag == "hiro") {
			theAudio = audio1;
		} else if (marker.Tag == "kanji") {
			theAudio = audio2;
		} else if (marker.Tag == "pat1") {
			theAudio = audio3;
		} else if (marker.Tag == "pat2") {
			theAudio = audio4;
		}
			
		firstPlayAudio = true;

		theAudio.PlayDelayed(((float)delay)/1000);

		oThread = new Thread(new ThreadStart(() => baseAudioDelay(baseDelay + delay)));

		oThread.Start();
		while (!oThread.IsAlive);
	}
	void OnMarkerLost(ARMarker marker){
		if (oThread != null) {
			oThread.Abort ();
			while (oThread.IsAlive);
		}
	}
	void OnMarkerTracked(ARMarker marker){
			
		AudioSource theAudio = null; 
		if (marker.Tag == "hiro") {
			theAudio = audio1;
		} else if (marker.Tag == "kanji") {
			theAudio = audio2;
		} else if (marker.Tag == "pat1") {
			theAudio = audio3;
		} else if (marker.Tag == "pat2") {
			theAudio = audio4;
		}
			
		if (playAudio == true) {
			theAudio.Play ();
			playAudio = false;
			firstPlayAudio = false;
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
			wait(baseDelay);
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
		return (int)Math.Round(((baseDelay)*pos)/max);
	}	
		
}



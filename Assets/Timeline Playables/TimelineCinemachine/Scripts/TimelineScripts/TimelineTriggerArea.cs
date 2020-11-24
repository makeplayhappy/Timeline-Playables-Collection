using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineTriggerArea : MonoBehaviour {

	[Header("Component References")]
	public TimelinePlaybackManager timelinePlaybackManager;

	[Header("Settings")]
	public string playerString = "Player";


	void OnTriggerEnter(Collider theCollision){
		timelinePlaybackManager.PlayerEnteredZone ();
	}

	void OnTriggerExit(Collider theCollision){
		timelinePlaybackManager.PlayerExitedZone ();
	}
}

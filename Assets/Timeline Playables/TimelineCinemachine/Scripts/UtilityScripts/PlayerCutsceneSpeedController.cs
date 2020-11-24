using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCutsceneSpeedController : MonoBehaviour {

	public Animator playerAnimator;

	public void SetPlayerSpeed(){
		playerAnimator.SetFloat("Forward", 0);
	}

}

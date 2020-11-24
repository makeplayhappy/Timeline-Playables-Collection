using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSkyboxShaderValuesAtStart : MonoBehaviour {

	public Material skyboxMat;

	void Start(){
		skyboxMat.SetFloat ("_Exposure", 1.3f);
	}

	void OnApplicationQuit(){
		skyboxMat.SetFloat ("_Exposure", 1.3f);
	}
}

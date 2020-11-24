using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class ShaderFloatControlPlayable: BasicPlayableBehaviour
{

	public string shaderPropertyFloatName;
	private int shaderPropertyID;

	public float startValue;
	public float endValue;

	public override void OnGraphStart(Playable playable){
		shaderPropertyID = Shader.PropertyToID (shaderPropertyFloatName);
	}

	public override void ProcessFrame(Playable playable, FrameData info, object userData)
	{
		//userData is the object set in the track's binding
		var bindedMaterial = userData as Material;



		if (bindedMaterial != null)
		{
			float currentFloatValue = Mathf.Lerp (startValue, endValue, (float)(playable.GetTime() / playable.GetDuration()));

			bindedMaterial.SetFloat(shaderPropertyFloatName, currentFloatValue);
			//Debug.Log(bindedMaterial.name);
		}
	}
}
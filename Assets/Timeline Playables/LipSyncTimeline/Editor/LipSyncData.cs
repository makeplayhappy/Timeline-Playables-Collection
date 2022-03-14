using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ScriptableObject definition for holding lip sync timings, imported from Rhubarb - then converted to this format, and this can then be "sent" to an active timeline track
namespace DH.Lipsync
{
		
	[System.Serializable]
	public struct LipSyncTiming{
		[SerializeField]
		public LipSwitcherPhoneme phoneme;
//		[SerializeField]
//		public byte phonemeNumber;
		[SerializeField]
		public float start;
		[SerializeField]
		public float end;
	}



public class LipSyncData : ScriptableObject{
		[SerializeField]
		public string assetName;
		[SerializeField]
		public float length;
		[SerializeField]
  		public List<LipSyncTiming> lipSyncTiming;

}

}//namespace
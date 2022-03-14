using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class LipMorphBehaviour : PlayableBehaviour {

    public string viseme = "X"; // probably wont need this, was used to reset to a zero state - we will move the morphs to zero here
    //public List<int> usedMorphKeys;

    public override void OnPlayableCreate (Playable playable) {
    //run through all the clips and build a list of all modified blend shape indexes
    // !place holder code
        //usedMorphKeys = new List<int>();
        //usedMorphKeys.Add(2);
    }
}

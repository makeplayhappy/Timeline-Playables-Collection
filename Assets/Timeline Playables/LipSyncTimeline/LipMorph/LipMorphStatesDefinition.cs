using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// May need to be able to rebuild the morphIndexes - can add an editor inspector to do this

// Maybe add a button to automatically set up RhubarbLipMorphPhoneme as string keys - if they're not there
// dont want to enforce them as this will be used to drive non phoneme "emotions" / eyebrows / nose morphs

[CreateAssetMenu(fileName = "new LipMorphStatesDefinition", menuName = "DH/LipMorphStatesDefinition", order = 91)]
public class LipMorphStatesDefinition : ScriptableObject{
    // Start is called before the first frame update
    public SkinnedMeshRenderer skinReference;
    public List<LipMorphStates> states;

    public int[] GetUsedBlendKeys(){
        List<int> usedKeys = new List<int>();
        for(int s = 0; s < states.Count; s++){
            for(int m = 0; m < states[s].morphSettings.Count; m++ ){
                usedKeys.Add( states[s].morphSettings[m].morphIndex );
            }
        }
        return usedKeys.ToArray();
    }

    // blendshapeIndex, blendshapeAmount
    public Dictionary<int, float> GetMorphSettings(string morphState){
        Dictionary<int, float> m_dict = new Dictionary<int, float>(); 
        for(int s = 0; s < states.Count; s++){
            if( states[s].morphState == morphState ){
                for(int m = 0; m < states[s].morphSettings.Count; m++ ){
                    m_dict.Add( states[s].morphSettings[m].morphIndex, states[s].morphSettings[m].amount);
                }


            }
        }
        return m_dict; 

    }

/*
    public Vector2 getOffset(LipMorphPhoneme phoneme){
        for(byte i = 0;i < offsets.Count; i++){
            if( phoneme == offsets[i].phoneme){
                return offsets[i].offset;
            }
        }
        return Vector2.zero;
    }
*/
}


[System.Serializable]
public struct LipMorphStates{
    [SerializeField]
    public string morphState;
    [SerializeField]
    public List<LipMorphSettings> morphSettings;

}

[System.Serializable]
public struct LipMorphSettings{
    [SerializeField]
    public string morphName;
    [SerializeField]
    public int morphIndex;
    [SerializeField]
    [Range(0f, 100f)]
    public float amount;
}

//these indexes need to match the numbers from Rhubarb XML
// https://github.com/DanielSWolf/rhubarb-lip-sync
//A = Closed mouth for the “P”, “B”, and “M” sounds.
//B = Slightly open mouth with clenched teeth consonants (“K”, “S”, “T”, etc.)
//C = Open mouth. This mouth shape is used for vowels like “EH” as in men and “AE” as in bat
//D = Wide open mouth. This mouth shapes is used for vowels like “AA” as in father.
//E = Slightly rounded mouth. This mouth shape is used for vowels like “AO” as in off and “ER” 
//F = Puckered lips. This mouth shape is used for “UW” as in you, “OW” as in show, and “W” 
//G = Upper teeth touching the lower lip for “F” as in for and “V” as in very.
//H = Used for long “L” sounds, with the tongue raised behind the upper teeth. 
//X = Idle position. This mouth shape is used for pauses in speech. 

[System.Serializable]
public enum RhubarbLipMorphPhoneme{
    A = 0,
    B = 1,
    C = 2,
    D = 3,
    E = 4,
    F = 5,
    G = 6,
    H = 7,
    X = 8
}
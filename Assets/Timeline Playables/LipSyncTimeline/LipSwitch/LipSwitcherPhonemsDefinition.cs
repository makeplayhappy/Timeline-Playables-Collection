using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new LipSwitcherPhonemsDefinition", menuName = "DH/LipSwitcherPhonemsDefinition", order = 90)]
public class LipSwitcherPhonemsDefinition : ScriptableObject{
    // Start is called before the first frame update

    public List<LipSwitcherOffsets> offsets;

    public Vector2 getOffset(LipSwitcherPhoneme phoneme){
        for(byte i = 0;i < offsets.Count; i++){
            if( phoneme == offsets[i].phoneme){
                return offsets[i].offset;
            }
        }
        return Vector2.zero;
    }

}


[System.Serializable]
public struct LipSwitcherOffsets{
    [SerializeField]
    public LipSwitcherPhoneme phoneme;
    [SerializeField]
    public byte phonemeNumber;
    [SerializeField]
    public Vector2 offset;
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
public enum LipSwitcherPhoneme{
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
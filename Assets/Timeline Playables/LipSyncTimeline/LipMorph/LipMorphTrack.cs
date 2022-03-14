using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.45f, 0.98f, 0.47f)]
[TrackClipType(typeof(LipMorphClip))]
[TrackBindingType(typeof(SkinnedMeshRenderer))]
public class LipMorphTrack : TrackAsset
{
    //public string mystring = "oh";
    //public ExposedReference<string> mtv = "takemy"; //for attacks

    public LipMorphStatesDefinition definitions;
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        //Debug.Log("LipMorphTrack CreateTrackMixer");
        //Debug.Log("Definitions state count:" + definitions.states.Count);
        var playable = ScriptPlayable<LipMorphMixerBehaviour>.Create (graph, inputCount);
        playable.GetBehaviour().definitions = definitions;
        return playable;
       // return ScriptPlayable<LipMorphMixerBehaviour>.Create (graph, inputCount);
    }
}

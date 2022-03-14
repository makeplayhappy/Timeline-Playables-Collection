using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.9056604f, 0.6832226f, 0.2605909f)]
[TrackClipType(typeof(LipSwitcherClip))]
[TrackBindingType(typeof(Material))]
public class LipSwitcherTrack : TrackAsset
{
    //public string mystring = "oh";
    //public ExposedReference<string> mtv = "takemy"; //for attacks

    public LipSwitcherPhonemsDefinition definitions;
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var playable = ScriptPlayable<LipSwitcherMixerBehaviour>.Create (graph, inputCount);
        playable.GetBehaviour().definitions = definitions;
        return playable;
       // return ScriptPlayable<LipSwitcherMixerBehaviour>.Create (graph, inputCount);
    }
}

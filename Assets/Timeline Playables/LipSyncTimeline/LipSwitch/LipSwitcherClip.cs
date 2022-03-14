using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class LipSwitcherClip : PlayableAsset, ITimelineClipAsset
{
    public LipSwitcherBehaviour template = new LipSwitcherBehaviour ();
    //public ExposedReference<Vector2> m_offset;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<LipSwitcherBehaviour>.Create (graph, template);
    //    LipSwitcherBehaviour clone = playable.GetBehaviour ();
    //    clone.offset = m_offset.Resolve (graph.GetResolver ());
        return playable;
    }
}

using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class LipMorphClip : PlayableAsset, ITimelineClipAsset
{
    public LipMorphBehaviour template = new LipMorphBehaviour ();
    //public ExposedReference<Vector2> m_offset;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<LipMorphBehaviour>.Create (graph, template);
    //    LipMorphBehaviour clone = playable.GetBehaviour ();
    //    clone.offset = m_offset.Resolve (graph.GetResolver ());
        return playable;
    }
}

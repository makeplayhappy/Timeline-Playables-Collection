using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Samples
{
    [TrackColor(177f/255f,253f/255f, 89f/255f)]
    [TrackBindingType(typeof(GameObject))]
    [TrackClipType(typeof(ColliderClipBase))]
    public class ColliderTrack : TrackAsset, ILayerable
    {
        Playable ILayerable.CreateLayerMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return Playable.Null;
        }
    }
}

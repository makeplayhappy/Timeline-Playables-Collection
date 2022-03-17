using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Sample
{
    // A Clip that has a animatable intensity
    public class LightIntensityClip : LightFXClipBase
    {
        [Serializable]
        public class ClipBehaviour : LightFXBehaviourBase, IIntensityModifier
        {
            public float LightIntensity = 1.0f;
            float IIntensityModifier.Intensity
            {
                get { return LightIntensity; }
            }
        }

        public ClipBehaviour AnimatedProperties = new ClipBehaviour();
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<ClipBehaviour>.Create(graph, AnimatedProperties);
        }
    }
}


using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Sample
{
    // A Clip that has a animatable color
    public class LightColorClip : LightFXClipBase
    {
        [Serializable]
        public class ClipBehaviour : LightFXBehaviourBase, IColorModifier
        {
            public Color LightColor = Color.white;

            Color IColorModifier.Color
            {
                get { return LightColor; }
            }
        }

        public ClipBehaviour AnimatedProperties = new ClipBehaviour();
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<ClipBehaviour>.Create(graph, AnimatedProperties);
        }
    }
}

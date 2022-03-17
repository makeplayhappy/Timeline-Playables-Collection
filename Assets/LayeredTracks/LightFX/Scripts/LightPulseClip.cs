using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Sample
{
    // A clip that generates a flicker animation by varying the intensity
    public class LightPulseClip : LightFXClipBase
    {
        [UnityEngine.Range(0.01f,100)]
        [Tooltip("Use this to modify the randomness of the flicker")]
        public float Period = 2f;

        [Serializable]
        public class ClipBehaviour : LightFXBehaviourBase, IIntensityModifier
        {
            // non-animated properties
            public float Period { get; set; }

            // animated properties
            public Vector2 Range = new Vector2(0, 1);

            // output value
            public float Intensity { get; private set; }

            public override void ProcessFrame(Playable playable, FrameData info, object playerData)
            {
                float min = Mathf.Min(Range.x, Range.y);
                float offset = Mathf.Abs(Range.y - Range.x);

                Intensity = Mathf.Cos((float) (playable.GetTime()/Period * 2 * Math.PI)) * 0.5f + 0.5f;
                Intensity = min + Intensity * offset;
            }
        }

        public ClipBehaviour AnimatedProperties = new ClipBehaviour();
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<ClipBehaviour>.Create(graph, AnimatedProperties);
            playable.GetBehaviour().Period = Period;
            return playable;
        }

        public override double duration
        {
            get { return Period; }
        }

        public override ClipCaps clipCaps
        {
            get { return base.clipCaps | ClipCaps.Looping; }
        }
    }
}

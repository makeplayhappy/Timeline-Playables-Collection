using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Sample
{
    // A clip that generates a flicker animation by varying the intensity
    public class LightFlickerClip : LightFXClipBase
    {
        [UnityEngine.Range(0,1)]
        [Tooltip("Use this to modify the randomness of the flicker")]
        public float Shift = 0f;

        [Serializable]
        public class ClipBehaviour : LightFXBehaviourBase, IIntensityModifier
        {
            public float Shift { get; set; }

            // animated properties
            [Range(0, 50)]
            public float Speed = 10;
            public Vector2 Range = new Vector2(0, 1);

            public float Intensity { get; private set; }

            public override void ProcessFrame(Playable playable, FrameData info, object playerData)
            {
                float min = Mathf.Min(Range.x, Range.y);
                float offset = Mathf.Abs(Range.y - Range.x);

                Intensity = Mathf.PerlinNoise((float) playable.GetTime() * Speed, Shift)  * offset + min;
            }
        }

        public ClipBehaviour AnimatedProperties = new ClipBehaviour();
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<ClipBehaviour>.Create(graph, AnimatedProperties);
            playable.GetBehaviour().Shift = Shift;
            return playable;
        }
    }
}

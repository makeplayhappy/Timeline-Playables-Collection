using System.Net;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Sample
{
    [TrackColor(.6f,.5f,.6f)]
    [TrackBindingType(typeof(Light))]
    [TrackClipType(typeof(LightFXClipBase))]
    public class LightFXTrack : TrackAsset, ILayerable
    {
        public bool Additive = false;

        class LayerMixer : PlayableBehaviour
        {
            public Color defaultColor { get; set; }
            public float defaultIntensity { get; set; }

            private Light m_Binding;

            public override void ProcessFrame(Playable playable, FrameData info, object playerData)
            {
                var color = defaultColor;
                var intensity = defaultIntensity;

                int inputCount = playable.GetInputCount();
                for (int i = 0; i < inputCount; i++)
                {
                    var input = playable.GetInput(i);
                    var trackMixer = ((ScriptPlayable<TrackMixer>) input).GetBehaviour();
                    if (trackMixer.ColorWeight <= float.Epsilon && trackMixer.IntensityWeight <= 0)
                        continue;

                    if (trackMixer.Additive)
                    {
                        color += trackMixer.Color * trackMixer.ColorWeight;
                        intensity += trackMixer.Intensity * trackMixer.IntensityWeight;
                    }
                    else
                    {
                        color = Color.Lerp(color, trackMixer.Color, trackMixer.ColorWeight);
                        intensity = Mathf.Lerp(intensity, trackMixer.Intensity, trackMixer.IntensityWeight);
                    }
                }

                color.a = 1;

                var light = playerData as Light;
                if (light != null)
                {
                    light.color = color;
                    light.intensity = intensity;
                }

                m_Binding = light;
            }

            public override void OnPlayableDestroy(Playable playable)
            {
                if (m_Binding != null)
                {
                    m_Binding.color = defaultColor;
                    m_Binding.intensity = defaultIntensity;
                }
            }
        }


        class TrackMixer : PlayableBehaviour
        {
            public bool Additive;

            public float ColorWeight { get; private set; }
            public float IntensityWeight { get; private set; }
            public Color Color { get; private set; }
            public float Intensity { get; private set; }


            public override void ProcessFrame(Playable playable, FrameData info, object playerData)
            {
                ColorWeight = 0;
                IntensityWeight = 0;
                Color = Color.clear;
                Intensity = 0;

                int inputCount = playable.GetInputCount();
                for (int i = 0; i < inputCount; i++)
                {
                    var weight = playable.GetInputWeight(i);
                    if (weight <= float.Epsilon)
                        continue;
                    var input = playable.GetInput(i);
                    if (input.GetPlayState() != PlayState.Playing)
                        continue;

                    var type = input.GetPlayableType();
                    if (!typeof(LightFXBehaviourBase).IsAssignableFrom(type))
                        continue;

                    var scriptPlayable = (ScriptPlayable<LightFXBehaviourBase>) input;
                    var behaviour = scriptPlayable.GetBehaviour();

                    var colorModifier = behaviour as IColorModifier;
                    if (colorModifier != null)
                    {
                        ColorWeight += weight;
                        Color += colorModifier.Color * weight;
                    }

                    var intensityModifier = behaviour as IIntensityModifier;
                    if (intensityModifier != null)
                    {
                        IntensityWeight += weight;
                        Intensity += intensityModifier.Intensity * weight;
                    }
                }
            }
        }


        Playable ILayerable.CreateLayerMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var defaultColor = Color.white;
            var defaultIntensity = 1.0f;

            if (go != null)
            {
                var director = go.GetComponent<PlayableDirector>();
                if (director != null)
                {
                    var light = director.GetGenericBinding(this) as Light;
                    if (light != null)
                    {
                        defaultColor = light.color;
                        defaultIntensity = light.intensity;
                    }

                }
            }

            var playable = ScriptPlayable<LayerMixer>.Create(graph, inputCount);
            var behaviour = playable.GetBehaviour();
            behaviour.defaultColor = defaultColor;
            behaviour.defaultIntensity = defaultIntensity;
            return playable;
        }

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var playable = ScriptPlayable<TrackMixer>.Create(graph, inputCount);
            var behaviour = playable.GetBehaviour();
            behaviour.Additive = Additive;
            return playable;
        }
    }
}

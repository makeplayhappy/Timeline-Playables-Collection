using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Sample
{
    public interface IColorModifier
    {
        Color Color { get; }
    }

    public interface IIntensityModifier
    {
        float Intensity { get; }
    }


    public class LightFXBehaviourBase : PlayableBehaviour
    {}

    public abstract class LightFXClipBase : PlayableAsset, ITimelineClipAsset, IPropertyPreview
    {
        // support ease-in/ease-out and mixing
        public virtual ClipCaps clipCaps
        {
            get { return ClipCaps.Blending; }
        }

        // make sure intensity and color properties are driven, so scene value changes are not saved
        public virtual void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
            driver.AddFromName<Light>("m_Color");
            driver.AddFromName<Light>("m_Intensity");
        }
    }
}

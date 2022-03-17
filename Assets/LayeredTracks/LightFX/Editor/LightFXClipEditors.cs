using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace Timeline.Sample.Editors
{
    [CustomTimelineEditor(typeof(LightFXClipBase))]

    public class LightFXClipEditorBase : ClipEditor
    {
        public override ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            var options = base.GetClipOptions(clip);
            options.highlightColor = Color.grey;
            return options;
        }

        public override void OnCreate(TimelineClip clip, TrackAsset track, TimelineClip clonedFrom)
        {
            clip.displayName = clip.displayName.Replace("Light", string.Empty).Replace("Clip", string.Empty);
        }

    }

    [CustomTimelineEditor(typeof(LightColorClip))]
    public class LightColorClipEditor : LightFXClipEditorBase
    {
        public override ClipDrawOptions GetClipOptions(TimelineClip clip)
        {
            var options = base.GetClipOptions(clip);
            options.highlightColor = (clip.asset as LightColorClip).AnimatedProperties.LightColor;
            return options;
        }
    }
}

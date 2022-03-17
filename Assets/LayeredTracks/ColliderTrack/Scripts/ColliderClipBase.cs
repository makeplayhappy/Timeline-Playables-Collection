using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Samples
{

    public abstract class ColliderBehaviourBase : PlayableBehaviour
    {
        public abstract void OnClipEnter();
        public abstract void OnClipExit();

        public abstract void ProcessClip();

        private bool m_InClip = false;
        
        protected GameObject binding { get; set; }

        public sealed override void OnBehaviourPlay(Playable playable, FrameData info)
        {
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            binding = playerData as GameObject;
            if (binding == null)
                return;

            if (!m_InClip)
            {
                OnClipEnter();
                m_InClip = true;
            }

            ProcessClip();
         }

        public sealed override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (!m_InClip)// skip initial calls
                return;

            if (info.effectivePlayState == PlayState.Playing)
                return;
            
            if (binding != null)
                OnClipExit();
            m_InClip = false;
        }

        public sealed override void OnPlayableDestroy(Playable playable)
        {
            if (m_InClip && binding != null)
                OnClipExit();
        }
    }
    
    
    public abstract class ColliderClipBase : PlayableAsset, ITimelineClipAsset
    {
        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }
    }
}

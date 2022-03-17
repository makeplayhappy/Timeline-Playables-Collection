using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.Samples
{
    public class BoxCollider : ColliderClipBase
    {
        [Serializable]
        public class BoxColliderBehaviour : ColliderBehaviourBase
        {
            public bool IsTrigger;
            public Vector3 Center = Vector3.zero;
            public Vector3 Size = Vector3.one;

            private UnityEngine.BoxCollider m_Collider;
            
            public override void OnClipEnter()
            {
                m_Collider = binding.AddComponent<UnityEngine.BoxCollider>();
            }

            public override void OnClipExit()
            {
                if (Application.isPlaying)
                    UnityEngine.Object.Destroy(m_Collider);
                else 
                    UnityEngine.Object.DestroyImmediate(m_Collider);
            }

            public override void ProcessClip()
            {
                m_Collider.isTrigger = IsTrigger;
                m_Collider.center = Center;
                m_Collider.size = Size;
            }
        }
        
        public BoxColliderBehaviour AnimatedProperties = new BoxColliderBehaviour(); 
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<BoxColliderBehaviour>.Create(graph, AnimatedProperties);
        }
    }
}

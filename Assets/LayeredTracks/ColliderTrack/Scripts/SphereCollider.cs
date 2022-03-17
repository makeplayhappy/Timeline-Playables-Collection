using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.Samples
{
    public class SphereCollider : ColliderClipBase
    {
        [Serializable]
        public class SphereColliderBehaviour : ColliderBehaviourBase
        {
            public bool IsTrigger;
            public Vector3 Center = Vector3.zero;
            public float Radius = 1;

            private UnityEngine.SphereCollider m_Collider;
            
            public override void OnClipEnter()
            {
                m_Collider = binding.AddComponent<UnityEngine.SphereCollider>();
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
                m_Collider.radius = Radius;
            }
        }
        
        public SphereColliderBehaviour AnimatedProperties = new SphereColliderBehaviour(); 
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<SphereColliderBehaviour>.Create(graph, AnimatedProperties);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline.Samples
{
    public class CapsuleCollider : ColliderClipBase
    {
        [Serializable]
        public class CapsuleColliderBehaviour : ColliderBehaviourBase
        {
            public bool IsTrigger;
            public Vector3 Center = Vector3.zero;
            public float Radius = 0.5f;
            [Range(0,2)]
            public int Direction = 1;
            public float Height = 1;
            
            private UnityEngine.CapsuleCollider m_Collider;
            
            public override void OnClipEnter()
            {
                m_Collider = binding.AddComponent<UnityEngine.CapsuleCollider>();
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
                m_Collider.direction = Direction;
                m_Collider.height = Height;
            }
        }
        
        public CapsuleColliderBehaviour AnimatedProperties = new CapsuleColliderBehaviour(); 
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<CapsuleColliderBehaviour>.Create(graph, AnimatedProperties);
        }
    }
}
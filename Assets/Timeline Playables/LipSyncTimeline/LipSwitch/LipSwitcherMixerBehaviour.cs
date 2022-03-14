using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class LipSwitcherMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    Material m_TrackBinding;
    Vector2 m_offset = Vector2.zero;

    public LipSwitcherPhonemsDefinition definitions;

    private LipSwitcherPhoneme m_lastPhoneme = LipSwitcherPhoneme.X;


    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        //Debug.Log(playerData);
        /*
        PlayableDirector director = (playable.GetGraph().GetResolver() as PlayableDirector);
        TimelineAsset ta = (TimelineAsset)director.playableAsset;

        //Debug.Log( nameof(playable.GetGraph().GetRootPlayable(0)) );
        //Debug.Log( typeof(playable.GetGraph().GetRootPlayable(0)).Name );
        Debug.Log( director.ToString() );
        Debug.Log( ta.GetRootTracks().ToString() );
        //LipSwitcherTrack lst = director.playableAsset as LipSwitcherTrack;
    */
        //Debug.Log( lst.mystring );

        m_TrackBinding = playerData as Material;

        if (!m_TrackBinding)
            return;

        int inputCount = playable.GetInputCount ();

        //Debug.Log("ProcessFrame" + info.frameId + " inputCount:" + inputCount);

        float greatestWeight = 0.3f; //using a float higher than 0 to avaoid any precicion issues - we only want playables at weight = 1 or at minum higher then .5 really


        //every frame it processes EVERY clip on the timeline....
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i); //get the weight for this "blend"

            //Debug.Log( definitions.name );
            if ( inputWeight > greatestWeight){
                
                greatestWeight = inputWeight;
                
                ScriptPlayable<LipSwitcherBehaviour> inputPlayable = (ScriptPlayable<LipSwitcherBehaviour>)playable.GetInput(i);
                LipSwitcherBehaviour input = inputPlayable.GetBehaviour ();
                
                // Use the above variables to process each frame of this playable.
                //Debug.Log("Loop" + i + " input.offset:" + input.offset.ToString("F1") + " inputWeight:" + inputWeight );
                
                
                //m_offset = input.offset;

                if( m_lastPhoneme != input.phoneme){

                    Vector2 m_offset = definitions.getOffset( input.phoneme );
                    m_TrackBinding.mainTextureOffset = m_offset; 
                    m_lastPhoneme = input.phoneme;
                }


            }
            
            
            
        }
    }

    //This function is called during the PrepareData phase of the PlayableGraph.
    //PrepareData is called as long as the playable is delayed.
    public override void PrepareData (Playable playable, FrameData info){

        Debug.Log("PrepareData" + info.frameId );

    }


    public override void OnPlayableDestroy (Playable playable)
    {
        if (m_TrackBinding == null)
            return;
        m_TrackBinding.mainTextureOffset = Vector2.zero;

    }

    //    OnBehaviourPause	This method is invoked when one of the following situations occurs: The effective play state during traversal is changed to 
    //PlayState.Paused. This state is indicated by FrameData.effectivePlayState. 
    //The PlayableGraph is stopped while the playable play state is Playing. This state is indicated by PlayableGraph.IsPlaying returning true.

    public override void OnBehaviourPause (Playable playable, FrameData info)
    {
        if (m_TrackBinding == null)
            return;
        m_TrackBinding.mainTextureOffset = Vector2.zero;
    }
    public override void OnBehaviourPlay (Playable playable, FrameData info)
    {
        if (m_TrackBinding == null)
            return;
        m_TrackBinding.mainTextureOffset = Vector2.zero;
    }

    //OnBehaviourPlay	This function is called when the Playable play state is changed to PlayState.Playing.
    //OnGraphStart	This function is called when the PlayableGraph that owns this PlayableBehaviour starts.
    public override void OnGraphStart (Playable playable)
    {
        if (m_TrackBinding == null)
            return;
        m_TrackBinding.mainTextureOffset = Vector2.zero;
    }

    public override void OnGraphStop (Playable playable)
    {
        if (m_TrackBinding == null)
            return;
        m_TrackBinding.mainTextureOffset = Vector2.zero;
    }


}

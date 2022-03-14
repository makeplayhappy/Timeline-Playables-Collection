using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class LipMorphMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    SkinnedMeshRenderer m_TrackBinding;

    public LipMorphStatesDefinition definitions;

    private Dictionary<int, float> blendKeyAmounts;


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
        //LipMorphTrack lst = director.playableAsset as LipMorphTrack;
    */
        //Debug.Log( lst.mystring );

        m_TrackBinding = playerData as SkinnedMeshRenderer;

        if (!m_TrackBinding)
            return;

        int inputCount = playable.GetInputCount ();
        resetAmountBlendKeys();

        //Debug.Log("ProcessFrame" + info.frameId + " inputCount:" + inputCount);
        //float greatestWeight = 0.3f; //using a float higher than 0 to avaoid any precicion issues - we only want playables at weight = 1 or at minum higher then .5 really
        //every frame it processes EVERY clip on the timeline....
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i); //get the weight for this "blend" from 0 to 1

            if( inputWeight > 0f){
                ScriptPlayable<LipMorphBehaviour> inputPlayable = (ScriptPlayable<LipMorphBehaviour>)playable.GetInput(i);
                LipMorphBehaviour input = inputPlayable.GetBehaviour();

                //Debug.Log("ProcessFrame " + i + " " + input.viseme + " weight:" + inputWeight);

                //get indexes and blend weights
                Dictionary<int, float> m_dict = definitions.GetMorphSettings( input.viseme );
                foreach( KeyValuePair<int, float> kvp in m_dict ){
                    blendKeyAmounts[ kvp.Key ] += kvp.Value * inputWeight;
                }

            }
            
        }
        //set the blendshape values
        foreach( KeyValuePair<int, float> kvp in blendKeyAmounts ){
            m_TrackBinding.SetBlendShapeWeight(kvp.Key, kvp.Value);
        }


    }

    //This function is called during the PrepareData phase of the PlayableGraph.
    //PrepareData is called as long as the playable is delayed.
    public override void PrepareData (Playable playable, FrameData info){

        //Debug.Log("PrepareData" + info.frameId );

    }

    public override void OnPlayableCreate(Playable playable){    
        //Debug.Log( "LipMorphMixerBehaviour OnPlayableCreate " );
    }


    public override void OnPlayableDestroy (Playable playable)
    {
        if (m_TrackBinding == null)
            return;
        //m_TrackBinding.mainTextureOffset = Vector2.zero;

    }

    //    OnBehaviourPause	This method is invoked when one of the following situations occurs: The effective play state during traversal is changed to 
    //PlayState.Paused. This state is indicated by FrameData.effectivePlayState. 
    //The PlayableGraph is stopped while the playable play state is Playing. This state is indicated by PlayableGraph.IsPlaying returning true.

    public override void OnBehaviourPause (Playable playable, FrameData info)
    {
        if (m_TrackBinding == null)
            return;
       //m_TrackBinding.mainTextureOffset = Vector2.zero;
    }
    public override void OnBehaviourPlay (Playable playable, FrameData info)
    {
        //Debug.Log("LipMorphMixerBehaviour OnBehaviourPlay");
        if (m_TrackBinding == null)
            return;
        //m_TrackBinding.mainTextureOffset = Vector2.zero;
    }

    //OnBehaviourPlay	This function is called when the Playable play state is changed to PlayState.Playing.
    //OnGraphStart	This function is called when the PlayableGraph that owns this PlayableBehaviour starts.
    public override void OnGraphStart (Playable playable)
    {
        //Debug.Log("LipMorphMixerBehaviour OnGraphStart");
        if( definitions == null){
            //Debug.Log( "LipMorphMixerBehaviour OnGraphStart definitions is null" );
            return;
        }

        blendKeyAmounts = new Dictionary<int, float>(); 
        //get all used blend key indexes
        int[] usedBlendKeys = definitions.GetUsedBlendKeys();
        for(int i = 0; i < usedBlendKeys.Length; i++){
            blendKeyAmounts.Add(usedBlendKeys[i] , 0f);
        }
        //Debug.Log("LipMorphMixerBehaviour usedBlendKeys from definition" +  usedBlendKeys.Length );


        if (m_TrackBinding == null)
            return;
        //m_TrackBinding.mainTextureOffset = Vector2.zero;
    }

    public override void OnGraphStop (Playable playable)
    {
        if (m_TrackBinding == null)
            return;
        //m_TrackBinding.mainTextureOffset = Vector2.zero;
    }

    private void resetAmountBlendKeys(){

        List<int> blendKeys = new List<int>(blendKeyAmounts.Keys);
        foreach( int indx in blendKeys ){
            blendKeyAmounts[indx] = 0f;
        }
        //Debug.Log();

        //blendKeyAmounts.Keys.ToList().ForEach(x => d[x] = 0f); //LINQ
       /*
       foreach( int indx in blendKeyAmounts.Keys ){
            blendKeyAmounts[ indx ] = 0f;// kvp.Value;
        }*/

    }

    public void ResetAllBlendShapes(){
        SetAllBlendShapes(0f);
    }
    public void SetAllBlendShapes(float amount){
        if (m_TrackBinding == null)
            return;

        int blendIndex = 0;
        m_TrackBinding.SetBlendShapeWeight(blendIndex, amount);

    }




}

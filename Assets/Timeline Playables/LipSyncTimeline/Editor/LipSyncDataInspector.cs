using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;
using System.IO;
using System.Xml;
namespace DH.Lipsync{



[CustomEditor(typeof(LipSyncData))]
public class LipSyncDataInspector : Editor{


    public override void OnInspectorGUI () {

		EditorGUILayout.LabelField("LipSync Data", EditorStyles.boldLabel);
		
		EditorGUILayout.HelpBox("Add this data as a track in the current timeline. You will need to manually assign the material that will be controlled",
		MessageType.Info, true
		);

		EditorGUILayout.Space();

		if(GUILayout.Button("Add Timeline Track")){

           LipSyncData data = (LipSyncData)target;

           Debug.Log("Add Timeline Track Button Pressed " );
           ToTimelineTrack( data );
        }

		EditorGUILayout.Space();

        DrawDefaultInspector();


        
        

	}

	private LipSwitcherPhonemsDefinition getLipSwitchDefinition(){
		string[] results;
        results = AssetDatabase.FindAssets("t:LipSwitcherPhonemsDefinition");
		foreach (var guid in results)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log(path);
			if( path.Contains("LipSwitcherPhonem") ){
				Debug.Log("found LipSwitcherPhonem");

				return (LipSwitcherPhonemsDefinition)AssetDatabase.LoadAssetAtPath(path, typeof(LipSwitcherPhonemsDefinition) );
			}
        }
		
		//Debug.Log( string.Join(",", results) );

		return null;
	}

    private void ToTimelineTrack(LipSyncData data){

        Debug.Log( "name: " + data.assetName + " length: " + data.length  );


		LipSwitcherPhonemsDefinition ls_d = getLipSwitchDefinition();
		
		if( ls_d == null){
			Debug.Log("Cant find a definitions Scriptable Object :: Searched for LipSwitcherPhonemsDefinition > LipSwitcherPhonems");
			return;
		}

		//Debug.Log("got SO definition " + ls_d.name);
		//return;

		if( data.lipSyncTiming.Count == 0 ){
			Debug.Log("No data to add to track :: lipSyncTiming.Count is 0");
			return;
		}


		if( TimelineEditor.masterAsset == null){
			Debug.Log("Open the timeline window to be able to add new tracks :: TimelineEditor.masterAsset is null");
			return;
		}
		//add a rest at the end - Rhubarb does this already

		//List<LipSyncTiming> LS_Timing = data.lipSyncTiming;
		/*
		LipSyncTiming endRest = new LipSyncTiming();
		endRest.start = LS_Timing[ LS_Timing.Count-1 ].end + 0.35f;
		endRest.end = endRest.start + 0.2f;
		endRest.phoneme = LipSwitcherPhoneme.X; 
		LS_Timing.Add(endRest);
		*/

		Debug.Log( "Using timeline: " + TimelineEditor.masterAsset.name );

		TimelineAsset masterTimeline = TimelineEditor.masterAsset;
		LipSwitcherTrack ls_track = masterTimeline.CreateTrack<LipSwitcherTrack>(null, "LS_" + data.assetName);
		ls_track.definitions = ls_d;

		//double playTime = playable.GetGraph().GetRootPlayable(0).GetTime();

		double playHeadTime = TimelineEditor.masterDirector.time;

		for(int i = 0;i < data.lipSyncTiming.Count; i++){
			
			
			/*
			double duration = 0.2; 
			if( i + 1 < data.lipSyncTiming.Count){

				double next_time = data.lipSyncTiming[i+1].time - data.lipSyncTiming[i].time;
				if( next_time < duration){
					duration = next_time;
				}

			}*/
			TimelineClip clip = ls_track.CreateClip<LipSwitcherClip>();
			clip.start = playHeadTime + data.lipSyncTiming[i].start;
			clip.duration = data.lipSyncTiming[i].end - data.lipSyncTiming[i].start;
			clip.displayName = "L_" + data.lipSyncTiming[i].phoneme.ToString();

			LipSwitcherClip l_clip = clip.asset as LipSwitcherClip;
			l_clip.template.phoneme = data.lipSyncTiming[i].phoneme;

		}
		
		//add a rest at the end 
		
		
				//Debug.Log( "clip name" + clip.asset.template.phoneme);
		// &lt;AnimationPlayableAsset&gt;();
		//LipSwitcherClip clip = ScriptableObject.CreateInstance<LipSwitcherClip>();
		// clip.asset.template.phoneme = LipSwitcherPhoneme.CDGKNRSThYZ;
		//clip.di

	//	timelineClip.duration = clip.averageDuration;
    //timelineClip.displayName


		TimelineEditor.Refresh( RefreshReason.ContentsAddedOrRemoved );


    }

}

}
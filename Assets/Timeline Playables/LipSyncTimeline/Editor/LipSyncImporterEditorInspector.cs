using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Xml;
namespace DH.Lipsync{



[CustomEditor(typeof(LipSyncImporterEditor))]
public class LipSyncImporterEditorInspector : Editor{


    public override void OnInspectorGUI () {

		EditorGUILayout.LabelField("Rhubarb XML Importer", EditorStyles.boldLabel);
		
		EditorGUILayout.LabelField("Use this to create timing objects from Rhubarb XML files");

		EditorGUILayout.Space();

        DrawDefaultInspector();

        LipSyncImporterEditor importer = (LipSyncImporterEditor)target;
		EditorGUILayout.Space();
		
        if(GUILayout.Button("Import XML File")){
           
           Debug.Log("Import XML File Button Pressed " );

           if( importer.LipSyncXmlFile != null ){

               ImportXML( importer );

           }else{
               Debug.Log("No XML file defined");
           }

           
        }

	}

    private void ImportXML(LipSyncImporterEditor importer){

        Debug.Log( importer.LipSyncXmlFile.name );

        XmlDocument document = new XmlDocument();
		document.LoadXml(importer.LipSyncXmlFile.text);

        
        //create new scripatable object data
        LipSyncData m_data;
        m_data = ScriptableObject.CreateInstance<LipSyncData>();
        m_data.assetName = importer.LipSyncXmlFile.name;

        m_data.lipSyncTiming = new List<LipSyncTiming>();

		

        try {

			XmlNode duration = document.SelectSingleNode("//rhubarbResult//metadata//duration");
			m_data.length = float.Parse(duration.InnerText);
			//Phonemes
			XmlNode phonemesNode = document.SelectSingleNode("//rhubarbResult//mouthCues");
			if (phonemesNode != null) {
				XmlNodeList phonemeNodes = phonemesNode.ChildNodes;

				for (int p = 0; p < phonemeNodes.Count; p++) {
					XmlNode node = phonemeNodes[p];

					if (node.LocalName == "mouthCue") {

                        LipSyncTiming lipSyncTiming = new LipSyncTiming();
                        lipSyncTiming.start = float.Parse(node.Attributes["start"].Value);
						lipSyncTiming.end = float.Parse(node.Attributes["end"].Value);
                        //lipSyncTiming.phonemeNumber = byte.Parse(node.Attributes["phonemeNumber"].Value);
						Debug.Log("casting " + node.InnerText);
                        lipSyncTiming.phoneme = (LipSwitcherPhoneme) Enum.Parse(typeof(LipSwitcherPhoneme), node.InnerText);
                        
                        m_data.lipSyncTiming.Add( lipSyncTiming );


					}
				}
                if( m_data.lipSyncTiming.Count > 0){
                    CreateAsset(m_data, importer.scriptableAssetOutputFolder);
                }
                
			}

        } catch (Exception e){
			Debug.LogError("[XML loading issue - " + importer.LipSyncXmlFile.name + "] Malformed XML file.");
			Debug.LogError("Exception caught." + e.ToString() );
		}

    }

    private void CreateAsset(LipSyncData data, string outputFolder ){

        string savefilepath = outputFolder + Path.DirectorySeparatorChar + data.assetName + ".asset";
        bool okayToCreateAsset = true;
        if( File.Exists(savefilepath)){
            if( !EditorUtility.DisplayDialog("Overwrite " + data.assetName + "?", "This will overwrite the current stored asset file", "Overwrite", "Cancel") ){
                okayToCreateAsset = false;
            }
        }

        //LipSyncData MBInspector = (MusicBlockInspector)ScriptableObject.CreateInstance("MusicBlockInspector");
        //ScriptableObject.CreateInstance
        //if( notePrefab != null){
        //    MBInspector.notePrefab = notePrefab;
        //}

        if(okayToCreateAsset){
            Debug.Log("Creating " + data.assetName + " at " + savefilepath);
            AssetDatabase.CreateAsset(data, savefilepath);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            
            //MBInspector.ProcessMusicBlock(block);
            Selection.activeObject = data;

        }else{
            Debug.Log("NOT Creating " + data.assetName);
        }
    }
/*
    private void LoadXML (TextAsset xmlFile){ //, AudioClip linkedClip) {

		XmlDocument document = new XmlDocument();
		document.LoadXml(xmlFile.text);

		// Clear/define marker lists, to overwrite any previous file
		phonemeMarkers = new List<PhonemeMarker>();

		try {
			//Phonemes
			XmlNode phonemesNode = document.SelectSingleNode("//LipSyncData//phonemes");
			if (phonemesNode != null) {
				XmlNodeList phonemeNodes = phonemesNode.ChildNodes;

				for (int p = 0; p < phonemeNodes.Count; p++) {
					XmlNode node = phonemeNodes[p];

					if (node.LocalName == "marker") {
						int phoneme = int.Parse(node.Attributes["phonemeNumber"].Value);
						float time = float.Parse(node.Attributes["time"].Value);

						phonemeMarkers.Add(new PhonemeMarker(phoneme, time));
					}
				}
			}

			//Gestures
			XmlNode gesturesNode = document.SelectSingleNode("//LipSyncData//gestures");
				if (gesturesNode != null) {
					XmlNodeList gestureNodes = gesturesNode.ChildNodes;

					for (int p = 0; p < gestureNodes.Count; p++) {
						XmlNode node = gestureNodes[p];

						if (node.LocalName == "marker") {
							//string gesture = node.Attributes["gesture"].Value;
							int phoneme = int.Parse(node.Attributes["phonemeNumber"].Value);
							float time = float.Parse(node.Attributes["time"].Value);
							phonemeMarkers.Add(new PhonemeMarker(phoneme, time));

							//gestureMarkers.Add(new GestureMarker(gesture, time));
						}
					}
				}

			
		} catch {
			Debug.LogError("[XML loading issue - " + gameObject.name + "] Malformed XML file. See console for details. \nFor the sake of simplicity, LipSync Pro is unable to handle errors in XML files. The clip editor often can, however. Import this XML file into the clip editor and re-export to fix.");
		}

		phonemeMarkers.Sort(SortTime);

		//for (int i = 0; i < phonemeMarkers.Count; i++){
		//	Debug.Log(phonemeMarkers[i].time);
		//}
	}
		// Sort PhonemeMarker by timestamp
	public static int SortTime (PhonemeMarker a, PhonemeMarker b) {
			float sa = a.time;
			float sb = b.time;

			return sa.CompareTo(sb);
	}
*/


}

}
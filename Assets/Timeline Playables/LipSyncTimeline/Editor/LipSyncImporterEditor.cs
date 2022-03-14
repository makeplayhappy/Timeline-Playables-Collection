using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This exposes functionality for importing a Rubarb XML file into a scriptable object
namespace DH.Lipsync{

[CreateAssetMenu(fileName = "new LipSyncImporterEditor", menuName = "DH/LipSyncImporterEditor", order = 99)]
public class LipSyncImporterEditor : ScriptableObject
{
    public TextAsset LipSyncXmlFile;
    public string scriptableAssetOutputFolder = "Assets/DH/LipSyncs";

}
}
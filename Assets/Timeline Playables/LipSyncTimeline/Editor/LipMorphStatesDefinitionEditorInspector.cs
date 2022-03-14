using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LipMorphStatesDefinition))]
public class LipMorphStatesDefinitionEditorInspector : Editor{

    string[] blendShapeCache;
    int _choiceIndex = 0;

    private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);

    LipMorphStatesDefinition stateDefinition;

    void OnEnable() {

        stateDefinition = (LipMorphStatesDefinition)target;

        if( stateDefinition.skinReference == null)
            return;

        Mesh sharedMesh = stateDefinition.skinReference.sharedMesh;
        int blendShapeCount = sharedMesh.blendShapeCount;
        //get blendshape names and cahce them
        blendShapeCache = new string[blendShapeCount];
        for(int i=0;i<blendShapeCount;i++){
            blendShapeCache[i] = sharedMesh.GetBlendShapeName(i);
        }
        Debug.Log(stateDefinition.skinReference.name + " " + blendShapeCount);
     
     }
/*
     public static void Show (SerializedProperty list) {
		EditorGUILayout.PropertyField(list);
		EditorGUI.indentLevel += 1;
		if (list.isExpanded) {
			for (int i = 0; i < list.arraySize; i++) {
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
			}
		}
		EditorGUI.indentLevel -= 1;
        
	}
*/
    public override void OnInspectorGUI () {
        GUILayout.Label ("Hello world");

        //DrawDefaultInspector();

        serializedObject.Update();

        EditorGUILayout.PropertyField( serializedObject.FindProperty("skinReference") );

        //Debug.Log( stateDefinition.states.Count );

        GUILayout.Label ("State List");

        GUIStyle buttongroupStyle = EditorStyles.miniButtonLeft;

        //buttongroupStyle.margin = new RectOffset(0, 0, 0, 0);
        //buttongroupStyle.padding = new RectOffset(0, 0, 0, 0); //RectOffset(int left, int right, int top, int bottom); 

        //buttongroupStyle.contentOffset = new Vector2(10,0);

        SerializedProperty allStatesSerialized = serializedObject.FindProperty("states");
        //Debug.Log(stateSerialized.arraySize);
        if( allStatesSerialized.isArray ){
            for(int s = 0;s < allStatesSerialized.arraySize; s++ ){

                SerializedProperty stateSerialized = allStatesSerialized.GetArrayElementAtIndex(s);
                EditorGUI.indentLevel += 2;
                //morph state name
                EditorGUILayout.PropertyField( stateSerialized.FindPropertyRelative("morphState") );

                //morph settings
                SerializedProperty allMorphsSerialized = stateSerialized.FindPropertyRelative("morphSettings");
                if( allMorphsSerialized.isArray ){

                    for(int m = 0;m < allMorphsSerialized.arraySize; m++ ){

                        SerializedProperty morphSerialized = allMorphsSerialized.GetArrayElementAtIndex(m);

                        SerializedProperty morphNameSerialized = morphSerialized.FindPropertyRelative("morphName");

                        SerializedProperty morphIndexSerialized = morphSerialized.FindPropertyRelative("morphIndex");

                        //Debug.Log( morphNameSerialized.stringValue );

                        GUILayout.Label ("Morph Name: " + morphNameSerialized.stringValue); 
                        /*
                        if (GUILayout.Button("+", EditorStyles.miniButtonMid, miniButtonWidth)) {
                            allMorphsSerialized.InsertArrayElementAtIndex(s);
                        }*/

                        int index = 0;
                        if( morphIndexSerialized.intValue > 0 ){
                            index = morphIndexSerialized.intValue;
                        }else{
                            index = Mathf.Max (0, Array.IndexOf (blendShapeCache, morphNameSerialized.stringValue));
                        }
                        index = EditorGUILayout.Popup("Blend Shapes", index, blendShapeCache);

                        morphNameSerialized.stringValue = blendShapeCache[index];
                        morphIndexSerialized.intValue = index;

                        EditorGUILayout.PropertyField( morphNameSerialized );
                        EditorGUILayout.PropertyField( morphIndexSerialized );
                        EditorGUILayout.PropertyField( morphSerialized.FindPropertyRelative("amount") );
 
                        GUILayout.BeginHorizontal("");
                        GUILayout.Space(40f);
                        //add / remove into state list
                        //  Button(string text, GUIStyle style, params GUILayoutOption[] options); 
                        if (GUILayout.Button("+", EditorStyles.miniButtonLeft, miniButtonWidth)) {
                            allMorphsSerialized.InsertArrayElementAtIndex(s);
                        }
                        if (GUILayout.Button("-", EditorStyles.miniButtonRight, miniButtonWidth)) {
                            int oldSize = allMorphsSerialized.arraySize;
                            allMorphsSerialized.DeleteArrayElementAtIndex(s);
                            if (allMorphsSerialized.arraySize == oldSize) {
                                allMorphsSerialized.DeleteArrayElementAtIndex(s);
                            }
                        }
                        GUILayout.EndHorizontal();

                    }
                }


                EditorGUI.indentLevel -= 2;
                //EditorGUILayout.PropertyField( stateSerialized.FindPropertyRelative("morphSettings") );

                GUILayout.BeginHorizontal();
                GUILayout.Label("State Add/Remove");
                //add / remove into state list
                if (GUILayout.Button(" + ", EditorStyles.miniButtonLeft, miniButtonWidth)) {
			        allStatesSerialized.InsertArrayElementAtIndex(s);
		        }
                if (GUILayout.Button(" - ", EditorStyles.miniButtonRight, miniButtonWidth)) {
                    int oldSize = allStatesSerialized.arraySize;
                    allStatesSerialized.DeleteArrayElementAtIndex(s);
                    if (allStatesSerialized.arraySize == oldSize) {
                        allStatesSerialized.DeleteArrayElementAtIndex(s);
                    }
                }
                GUILayout.EndHorizontal();

                var rect = EditorGUILayout.BeginHorizontal();
                Handles.color = Color.grey;
                Handles.DrawLine(new Vector2(rect.x , rect.y), new Vector2(rect.width , rect.y), 4f);
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();
                

                //EditorGUILayout.PropertyField( stateSerialized );

            }
        }


        //EditorGUILayout.PropertyField();

        //EditorList.Show();
        
		//EditorList.Show(serializedObject.FindProperty("states"));

        /*
		EditorList.Show(serializedObject.FindProperty("vectors"));
		EditorList.Show(serializedObject.FindProperty("colorPoints"), false);
		EditorList.Show(serializedObject.FindProperty("objects"), false);*/
		serializedObject.ApplyModifiedProperties();




        if(GUILayout.Button("Add Rhubarb Visemes")){
           
            //check against  RhubarbLipMorphPhoneme enum string
            foreach (string visemeName in Enum.GetNames(typeof(RhubarbLipMorphPhoneme))){
                bool hasViseme = false;

                for(int i = 0; i < stateDefinition.states.Count; i++){
                    
                    if( stateDefinition.states[i].morphState == visemeName){
                        hasViseme = true;
                    }
                }

                if( !hasViseme){
                    //Debug.Log("Missing " + visemeName + " viseme");
                    //add them in
                }
                
            }

        }


	}


    

/*
    DrawDefaultInspector();
 
             // Set the choice index to the previously selected index
             _choiceIndex = Array.IndexOf(_choices, sameClass.choice);
 
             // If the choice is not in the array then the _choiceIndex will be -1 so set back to 0
             if (_choiceIndex < 0)
                 _choiceIndex = 0;
 
             _choiceIndex = EditorGUILayout.Popup(_choiceIndex, _choices);
             
             var someClass = target as SomeClass;
             // Update the selected choice in the underlying object
             someClass.choice = _choices[_choiceIndex];
             // Save the changes back to the object
              EditorUtility.SetDirty(target);
*/
}


/*
// IngredientDrawer
[CustomPropertyDrawer(typeof(Ingredient))]
public class IngredientDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var amountRect = new Rect(position.x, position.y, 30, position.height);
        var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
        var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
        EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
*/


    [CustomPropertyDrawer(typeof(LipMorphSettings))]
    public class LipMorphSettings : PropertyDrawer{
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            EditorGUI.BeginProperty(position, label, property);
            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            EditorGUI.EndProperty();
        }

    }
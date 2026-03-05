using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneTransitionTrigger))]
public class SceneTransitionTriggerEditor : Editor
{
    SerializedProperty sceneToLoadProp;
    SerializedProperty targetDataProp;
    SerializedProperty selectedIndexProp;
    SerializedProperty spawnPointNameProp;

    private void OnEnable()
    {
        sceneToLoadProp = serializedObject.FindProperty("sceneToLoad");
        targetDataProp = serializedObject.FindProperty("targetSpawnPointData");
        selectedIndexProp = serializedObject.FindProperty("selectedSpawnPointIndex");
        spawnPointNameProp = serializedObject.FindProperty("spawnPointName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(sceneToLoadProp);
        EditorGUILayout.PropertyField(targetDataProp);

        SpawnPointData data = targetDataProp.objectReferenceValue as SpawnPointData;
        if(data != null && data.spawnPointIDs != null && data.spawnPointIDs.Count > 0)
        {
            //create a dropwdown using a list of IDs
            int currentIndex = selectedIndexProp.intValue;
            if(currentIndex < 0 || currentIndex >= data.spawnPointIDs.Count)
            {
                currentIndex = 0;
            }
            string[] options = data.spawnPointIDs.ToArray();
            int newIndex = EditorGUILayout.Popup("Spawn Point", currentIndex, options);
            selectedIndexProp.intValue = newIndex;

            //update the spawnPointname automatically (OnValidate will also do it)
            spawnPointNameProp.stringValue = data.spawnPointIDs[newIndex];
        }
        else if(data != null)
        {
            EditorGUILayout.HelpBox("No spawn point IDs defined in the selected data asset.", MessageType.Warning);
        }

        //display the final spawn point name (read-only)
        GUI.enabled = false;
        EditorGUILayout.PropertyField(spawnPointNameProp);
        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }
}

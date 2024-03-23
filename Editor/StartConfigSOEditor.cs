using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StartConfigSO))]
public class StartConfigSOEditor : Editor
{
        public override void OnInspectorGUI()
        {
            var config = target as StartConfigSO;
		    if(config == null) return;

            // To keep the Script field ReadOnly like it is with the default inspector
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
            GUI.enabled = true;
      
            EditorGUILayout.PropertyField(serializedObject.FindProperty("MainMenuSceneIndex"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("LoadSceneOnStartGame"));
            if (config.LoadSceneOnStartGame) 
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SceneIndexToLoadOnStart"));
            }
            
            serializedObject.ApplyModifiedProperties();
        }
}
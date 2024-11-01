using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(_SceneManager))]
public class SceneManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _SceneManager sceneManager = (_SceneManager)target;

        GUILayout.Space(5); 

        if (GUILayout.Button("Load Scene List"))
        {
            sceneManager.sceneNames.Clear();
            for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                sceneManager.sceneNames.Add(sceneName);
            }
        }

        GUILayout.Space(10); 

        if (sceneManager.sceneNames.Count > 0)
        {
            int selectedIndex = sceneManager.sceneNames.IndexOf(sceneManager.loadSceneName);
            if (selectedIndex < 0) selectedIndex = 0; 

            selectedIndex = EditorGUILayout.Popup("Scene Name", selectedIndex, sceneManager.sceneNames.ToArray());
            sceneManager.loadSceneName = sceneManager.sceneNames[selectedIndex];
        }
        else
        {
            EditorGUILayout.LabelField("No Scene loaded.");
        }
    }
}

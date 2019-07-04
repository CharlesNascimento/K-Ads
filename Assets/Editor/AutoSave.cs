using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// Auto-save scene on run.
/// </summary>
[InitializeOnLoad]
public class AutosaveOnRun
{
    static AutosaveOnRun()
    {
        EditorApplication.playModeStateChanged += SaveCurrentScene;
    }

    private static void SaveCurrentScene(PlayModeStateChange state)
    {
        Scene activeScene = EditorSceneManager.GetActiveScene();
        if (!EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPlaying || !activeScene.isDirty) return;
        Debug.Log("Auto-Saving scene before entering Play mode: " + activeScene.name);
        EditorSceneManager.SaveScene(activeScene);
        AssetDatabase.SaveAssets();
    }
}

/// <summary>
/// This script creates a new window in the editor with a autosave function. 
/// It is saving your current scene with an interval from 1 minute to 10 minutes.
/// The configuration window is on Window/AutoSave
/// </summary>
public class AutoSave : EditorWindow
{
    private bool autoSaveScene = true;
    private bool showMessage = true;
    private bool isStarted = false;
    private int intervalScene;
    private DateTime lastSaveTimeScene = DateTime.Now;

    private string projectPath;

    void OnEnable()
    {
        projectPath = Application.dataPath;
    }

    [MenuItem("Window/AutoSave")]
    static void Init()
    {
        AutoSave saveWindow = (AutoSave)EditorWindow.GetWindow(typeof(AutoSave));
        saveWindow.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Info:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Saving to:", "" + projectPath);
        EditorGUILayout.LabelField("Saving scene:", "" + EditorSceneManager.GetActiveScene().name);
        GUILayout.Label("Options:", EditorStyles.boldLabel);
        autoSaveScene = EditorGUILayout.BeginToggleGroup("Auto save", autoSaveScene);
        intervalScene = EditorGUILayout.IntSlider("Interval (minutes)", intervalScene, 1, 10);
        if (isStarted)
        {
            EditorGUILayout.LabelField("Last save:", "" + lastSaveTimeScene);
        }
        EditorGUILayout.EndToggleGroup();
        showMessage = EditorGUILayout.BeginToggleGroup("Show Message", showMessage);
        EditorGUILayout.EndToggleGroup();
    }


    void Update()
    {
        if (autoSaveScene)
        {
            if (DateTime.Now.Minute >= (lastSaveTimeScene.Minute + intervalScene) || DateTime.Now.Minute == 59 && DateTime.Now.Second == 59)
                SaveScene();
        }
        else
            isStarted = false;
    }

    void SaveScene()
    {
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        lastSaveTimeScene = DateTime.Now;
        isStarted = true;
        if (showMessage)
            Debug.Log("AutoSave saved: " + EditorSceneManager.GetActiveScene().name + " on " + lastSaveTimeScene);
        var repaintSaveWindow = (AutoSave)GetWindow(typeof(AutoSave));
        repaintSaveWindow.Repaint();
    }
}
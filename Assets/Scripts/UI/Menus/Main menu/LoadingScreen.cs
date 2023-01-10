using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Tom Cornelissen <br/>
/// Modified by:  <br/>
/// Description: This class handles loading and unloading of scenes
/// </summary>
public class LoadingScreen : MonoBehaviour
{
    private int _scenesLoaded;
    private int _scenesUnloaded;

    private List<int> _oldScenes;
    private List<int> _newScenes;
    
    /// <summary>
    /// Loads and unloads the given scenes
    /// </summary>
    /// <param name="oldScenes">A list of ids of scenes to unload</param>
    /// <param name="newScenes">A list of ids of scenes to load</param>
    public void StartLoading(List<int> oldScenes, List<int> newScenes)
    {
        _oldScenes = oldScenes;
        _newScenes = newScenes;
        
        _scenesUnloaded = 0;
        foreach (int oldScene in _oldScenes)
        {
            StartCoroutine(UnloadOldScene(oldScene));
        }
    }

    private IEnumerator UnloadOldScene(int id)
    {
        var sceneAsync = SceneManager.UnloadSceneAsync(id);
        
        while (!sceneAsync.isDone) yield return null;
        
        _scenesUnloaded++;
        
        if (_scenesUnloaded >= _oldScenes.Count - 1) LoadNewScenes();
    }

    private void LoadNewScenes()
    {
        _scenesLoaded = 0;
        foreach (int id in _newScenes)
        {
            StartCoroutine(LoadScene(id));
        }
    }

    private IEnumerator LoadScene(int id)
    {
        var sceneAsync = SceneManager.LoadSceneAsync(id, LoadSceneMode.Single);//HOTFIX EVERYTHING BROKE used to say LoadSceneMode.Additive

        while (!sceneAsync.isDone) yield return null;
        
        _scenesLoaded++;

        if (_scenesLoaded >= _newScenes.Count - 1) SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    private int _scenesLoaded;
    private int _scenesUnloaded;

    private List<int> _oldScenes;
    private List<int> _newScenes;
    
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
        Debug.Log("Started unloading scene " + id);
        var sceneAsync = SceneManager.UnloadSceneAsync(id);
        
        while (!sceneAsync.isDone) yield return null;

        Debug.Log("Finished unloading scene " + id);
        _scenesUnloaded++;
        
        if (_scenesUnloaded >= _oldScenes.Count - 1) LoadNewScenes();
    }

    private void LoadNewScenes()
    {
        Destroy(InputBehaviour.Instance.gameObject);
        
        _scenesLoaded = 0;
        foreach (int id in _newScenes)
        {
            StartCoroutine(LoadScene(id));
        }
    }

    private IEnumerator LoadScene(int id)
    {
        Debug.Log("Started loading scene " + id);
        var sceneAsync = SceneManager.LoadSceneAsync(id, LoadSceneMode.Additive);

        while (!sceneAsync.isDone) yield return null;

        Debug.Log("Finished loading scene " + id);
        _scenesLoaded++;

        if (_scenesLoaded >= _newScenes.Count - 1) SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}

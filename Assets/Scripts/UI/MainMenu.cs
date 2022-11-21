using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Start scene")]
    
    [SerializeField]
    private string _startSceneName;

    [Header("Loading screen")] 
    
    [SerializeField]
    private GameObject _loadingScreenPrefab;

    [Header("Buttons enabled")]
    
    [SerializeField] 
    private bool _startButtonEnabled = true;
    
    [SerializeField] 
    private bool _loadButtonEnabled = true;
    
    [SerializeField] 
    private bool _settingsButtonEnabled = true;
    
    [SerializeField] 
    private bool _exitButtonEnabled = true;

    [Header("Button references")]
    
    [SerializeField]
    private Button _startButton;

    [SerializeField] 
    private Button _loadButton;
    
    [SerializeField] 
    private Button _settingsButton;
    
    [SerializeField] 
    private Button _exitButton;

    private void Start()
    {
        _startButton.Select();
        _startButton.interactable = _startButtonEnabled;
        _loadButton.interactable = _loadButtonEnabled;
        _settingsButton.interactable = _settingsButtonEnabled;
        _exitButton.interactable = _exitButtonEnabled;
    }

    public void OnStart()
    {
        Instantiate(_loadingScreenPrefab);
        
        SceneManager.LoadSceneAsync(_startSceneName, LoadSceneMode.Single);
    }

    public void OnLoad()
    {
        
    }

    public void OnSettings()
    {
        
    }

    public void OnExit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
        Application.Quit();
    }
}

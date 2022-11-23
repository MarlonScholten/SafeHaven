using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Start scene")]
    
    [SerializeField]
    private int _startSceneId;

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
    
    [Header("Menu types")]

    [SerializeField] 
    private GameObject _mainMenu;
    
    [SerializeField] 
    private GameObject _settingsMenu;

    private bool _wantsToLeave;

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
        
        SceneManager.LoadSceneAsync(_startSceneId, LoadSceneMode.Single);
    }

    public void OnLoad()
    {
        
    }

    public void OnSettings()
    {
        Instantiate(_settingsMenu, transform).GetComponent<SettingsMenu>().SetPreviousMenu(_mainMenu);
        
        _mainMenu.SetActive(false);
    }

    public void OnExit()
    {
        if (_wantsToLeave)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        
            Application.Quit();
            return;
        }

        _exitButton.GetComponentInChildren<TMP_Text>().SetText("Are you sure?");
        _exitButton.GetComponentInChildren<TMP_Text>().color = Color.red;
        _wantsToLeave = true;

        StartCoroutine(ExitConfirmationTimeout());
    }

    private IEnumerator ExitConfirmationTimeout()
    {
        yield return new WaitForSeconds(2.0f);

        _exitButton.GetComponentInChildren<TMP_Text>().SetText("Exit");
        _exitButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        _wantsToLeave = false;
    }
}

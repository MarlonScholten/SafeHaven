using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Buttons enabled")]
    [SerializeField] private bool _continueButtonEnabled = true;
    [SerializeField] private bool _saveButtonEnabled = true;
    [SerializeField] private bool _settingsButtonEnabled = true;
    [SerializeField] private bool _mainMenuButtonEnabled = true;
    [SerializeField] private bool _exitButtonEnabled = true;
    
    [Header("ButtonReferences")]
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _exitButton;

    [Header("Main menu")] 
    [SerializeField] private int _mainMenuSceneId;
    [SerializeField] private GameObject _loadingScreen;

    [Header("Header types")] 
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _settingsMenu;

    private bool _wantsToReturnToMainMenu;
    private bool _wantsToExit;

    private void Start()
    {
        _continueButton.onClick.AddListener(OnContinue);
        _saveButton.onClick.AddListener(OnSave);
        _settingsButton.onClick.AddListener(OnSettings);
        _mainMenuButton.onClick.AddListener(OnMainMenu);
        _exitButton.onClick.AddListener(OnExit);

        _continueButton.interactable = _continueButtonEnabled;
        _saveButton.interactable = _saveButtonEnabled;
        _settingsButton.interactable = _settingsButtonEnabled;
        _mainMenuButton.interactable = _mainMenuButtonEnabled;
        _exitButton.interactable = _exitButtonEnabled;
        
        _continueButton.Select();
    }

    private void OnContinue()
    {
        FindObjectOfType<PauseManager>().UnpauseGame();
    }

    private void OnSave()
    {
    }

    private void OnSettings()
    {
        Instantiate(_settingsMenu, transform).GetComponent<SettingsMenu>().SetPreviousMenu(_pauseMenu);
        
        _pauseMenu.SetActive(false);
    }

    private void OnMainMenu()
    {
        if (_wantsToReturnToMainMenu)
        {
            Instantiate(_loadingScreen);
        
            SceneManager.LoadSceneAsync(_mainMenuSceneId, LoadSceneMode.Single);
            return;
        }
        
        _mainMenuButton.GetComponentInChildren<TMP_Text>().SetText("Are you sure?");
        _mainMenuButton.GetComponentInChildren<TMP_Text>().color = Color.red;
        _wantsToReturnToMainMenu = true;

        StartCoroutine(MainMenuConfirmationTimeout());
    }

    private void OnExit()
    {
        if (_wantsToExit)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        
            Application.Quit();
            return;
        }
        
        _exitButton.GetComponentInChildren<TMP_Text>().SetText("Are you sure?");
        _exitButton.GetComponentInChildren<TMP_Text>().color = Color.red;
        _wantsToExit = true;

        StartCoroutine(ExitConfirmationTimeout());
    }

    private IEnumerator MainMenuConfirmationTimeout()
    {
        yield return new WaitForSeconds(2.0f);

        _mainMenuButton.GetComponentInChildren<TMP_Text>().SetText("Main menu");
        _mainMenuButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        _wantsToReturnToMainMenu = false;
    }

    private IEnumerator ExitConfirmationTimeout()
    {
        yield return new WaitForSeconds(2.0f);

        _exitButton.GetComponentInChildren<TMP_Text>().SetText("Exit");
        _exitButton.GetComponentInChildren<TMP_Text>().color = Color.white;
        _wantsToExit = false;
    }
}

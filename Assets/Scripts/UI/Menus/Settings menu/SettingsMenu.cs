using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    private GameObject _previousMenu;
    
    public void SetPreviousMenu(GameObject previousMenu)
    {
        _previousMenu = previousMenu;
    }

    public void CloseMenu()
    {
        _previousMenu.SetActive(true);
        
        Destroy(gameObject);
    }
}

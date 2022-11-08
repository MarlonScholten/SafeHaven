using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugHeaderWidget : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI textComponent;

    public void SetHeaderText(string headerText)
    {
        textComponent.SetText(headerText);
    }
}

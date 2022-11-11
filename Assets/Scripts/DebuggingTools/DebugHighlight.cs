using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugHighlight : MonoBehaviour
{
    [SerializeField] private Material highlightMaterial;

    private Material[] _originalMaterials;

    void Start()
    {
        _originalMaterials = GetComponent<MeshRenderer>().materials;
        
        FindObjectOfType<DebugUI>().ToggleDebuggingTools.AddListener(ToggleHighlight);
    }

    private void ToggleHighlight(bool isActivated)
    {
        if (isActivated)
        {
            Material[] highlightedMaterials = Enumerable.Repeat(highlightMaterial, _originalMaterials.Length).ToArray();

            GetComponent<MeshRenderer>().materials = highlightedMaterials;
        }
        else
        {
            GetComponent<MeshRenderer>().materials = _originalMaterials;
        }
    }
}

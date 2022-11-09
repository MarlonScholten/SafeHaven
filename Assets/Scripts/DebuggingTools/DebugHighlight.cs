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
    }

    private void Highlight()
    {
        Material[] highlightedMaterials = Enumerable.Repeat(highlightMaterial, _originalMaterials.Length).ToArray();

        GetComponent<MeshRenderer>().materials = highlightedMaterials;
    }

    private void UnHighlight()
    {
        GetComponent<MeshRenderer>().materials = _originalMaterials;
    }
}

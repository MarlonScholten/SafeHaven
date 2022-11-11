using System.Linq;
using UnityEngine;

namespace DebuggingTools
{
    public class DebugHighlight : MonoBehaviour
    {
        [SerializeField] 
        private Material highlightMaterial;

        private Material[] _originalMaterials;

        private void Start()
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
}

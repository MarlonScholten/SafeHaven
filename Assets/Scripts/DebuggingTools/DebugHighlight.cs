using System.Linq;
using UnityEngine;

namespace DebuggingTools
{
    /// <summary>
    /// Class <c>DebugHighlight</c> will highlight <c>GameObject</c>s while debugging is active.
    /// </summary>
    public class DebugHighlight : MonoBehaviour
    {
        [SerializeField] 
        private Material highlightMaterial;

        private Material[] _originalMaterials;

        private void Start()
        {
            _originalMaterials = GetComponent<MeshRenderer>().materials;
        
            FindObjectOfType<DebugToggle>().ToggleDebuggingToolsEvent.AddListener(ToggleHighlight);
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

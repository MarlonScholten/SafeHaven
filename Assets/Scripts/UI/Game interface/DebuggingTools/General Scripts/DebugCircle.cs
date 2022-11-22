using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace DebuggingTools
{
    /// <summary>
    /// Class <c>DebugCircle</c> displays a circle around the object while in debugging mode.
    /// </summary>
    public class DebugCircle : MonoBehaviour
    {
        [SerializeField]
        private VisualEffectAsset _circleVFX;

        [SerializeField] 
        private float _radius = 10.0f;

        [SerializeField] 
        private Color _color = Color.red;

        [SerializeField] 
        private bool _update;

        private void Start()
        {
            VisualEffect visualEffectComponent = gameObject.AddComponent<VisualEffect>();
            visualEffectComponent.enabled = false;
        
            visualEffectComponent.visualEffectAsset = _circleVFX;
            visualEffectComponent.SetFloat("Radius", _radius);
            visualEffectComponent.SetVector4("Color", _color);
        
            FindObjectOfType<DebugToggle>().ToggleDebuggingToolsEvent.AddListener(ToggleDebuggingTools);
        }

        private void ToggleDebuggingTools(bool isActivated)
        {
            gameObject.GetComponent<VisualEffect>().enabled = isActivated;
        }

        private void Update()
        {
            if (_update)
            {
                UpdateVisualEffect();
                _update = false;
            }
        }

        private void UpdateVisualEffect()
        {
            VisualEffect visualEffectComponent = gameObject.GetComponent<VisualEffect>();
        
            visualEffectComponent.SetFloat("Radius", _radius);
            visualEffectComponent.SetVector4("Color", _color); 
        
            visualEffectComponent.Reinit();
        }
    }
}

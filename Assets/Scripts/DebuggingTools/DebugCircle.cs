using UnityEngine;
using UnityEngine.VFX;

namespace DebuggingTools
{
    public class DebugCircle : MonoBehaviour
    {
        [SerializeField]
        private VisualEffectAsset circleVFX;

        [SerializeField] 
        private float radius = 10.0f;

        [SerializeField] 
        private Color color = Color.red;

        [SerializeField] 
        private bool update;

        private void Start()
        {
            VisualEffect visualEffectComponent = gameObject.AddComponent<VisualEffect>();
        
            visualEffectComponent.visualEffectAsset = circleVFX;
            visualEffectComponent.SetFloat("Radius", radius);
            visualEffectComponent.SetVector4("Color", color);
        
            FindObjectOfType<DebugUI>().ToggleDebuggingTools.AddListener(ToggleDebuggingTools);
        }

        private void ToggleDebuggingTools(bool isActivated)
        {
            gameObject.GetComponent<VisualEffect>().enabled = isActivated;
        }

        private void Update()
        {
            if (update)
            {
                UpdateVisualEffect();
                update = false;
            }
        }

        private void UpdateVisualEffect()
        {
            VisualEffect visualEffectComponent = gameObject.GetComponent<VisualEffect>();
        
            visualEffectComponent.SetFloat("Radius", radius);
            visualEffectComponent.SetVector4("Color", color); 
        
            visualEffectComponent.Reinit();
        }
    }
}

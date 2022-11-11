using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DebugCircle : MonoBehaviour
{
    [SerializeField]
    private VisualEffectAsset circleVFX;

    [SerializeField] 
    private float radius = 10.0f;

    [SerializeField] 
    private Color color = Color.red;

    [SerializeField] 
    private bool update = false;

    void Start()
    {
        VisualEffect visualEffectComponent = gameObject.AddComponent<VisualEffect>();
        
        visualEffectComponent.visualEffectAsset = circleVFX;
        visualEffectComponent.SetFloat("Radius", radius);
        visualEffectComponent.SetVector4("Color", color);
    }
    
    void Update()
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

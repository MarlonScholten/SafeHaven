using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChasingVignette : MonoBehaviour
{
    private Volume _volume;
    public Vignette vignette;
    
    private float _startVignetteIntensity;
    
    public readonly float _chasedVignetteIntensity = 0.5f;
    private bool _vignettePulseIncrease = false;

    public int nrOfAgentsChasing = 0;
    
    public void Awake()
    {
        _volume = GetComponent<Volume>();
        _volume.profile.TryGet(out vignette);
        _startVignetteIntensity = vignette.intensity.value;
    }

    public void Update()
    {
        Debug.Log(nrOfAgentsChasing + " --- " + vignette.intensity.value);
        if (nrOfAgentsChasing > 0)
        {
            vignette.intensity.value =
                Mathf.SmoothStep(_chasedVignetteIntensity, _startVignetteIntensity, Time.deltaTime);
        }
        else
        {
            vignette.intensity.value =
                Mathf.SmoothStep(_startVignetteIntensity, _chasedVignetteIntensity, Time.deltaTime);
        }
    }

    public void Increase()
    {
        nrOfAgentsChasing++;
    }

    public void Decrease()
    {
        nrOfAgentsChasing--;
    }
}
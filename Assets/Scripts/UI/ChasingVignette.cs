using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChasingVignette : MonoBehaviour
{
    private Volume _volume;
    private Vignette vignette;
    
    [SerializeField]
    private float _startVignetteIntensity;
    
    [SerializeField]
    private float _chasedVignetteIntensity = 0.5f;
    private bool _vignettePulseIncrease = false;

    public int nrOfAgentsChasing = 0;
    
    public void Awake()
    {
        _volume = GetComponent<Volume>();
        _volume.profile.TryGet(out vignette);
    }

    void Start()
    {
        vignette.intensity.value = _startVignetteIntensity;

    }

    public void Update()
    {
        if (nrOfAgentsChasing > 0)
        {
            vignette.intensity.value =
                Mathf.SmoothStep(_chasedVignetteIntensity, _startVignetteIntensity,  Time.deltaTime);
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
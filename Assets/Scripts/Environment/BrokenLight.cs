using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Author: Tom Cornelissen<br/>
/// Modified by: <br/>
/// Description: This script makes the lights flicker
/// </summary>
///<list type="table">
///	    <listheader>
///         <term>On what GameObject</term>
///         <term>Type</term>
///         <term>Name of type</term>
///         <term>Description</term>
///     </listheader>
///	    <item>
///         <term>BrokenLight</term>
///		    <term>Prefab</term>
///         <term>BrokenLight</term>
///		    <term>This prefab handles everything for the broken light</term>
///	    </item>
/// </list>
public class BrokenLight : MonoBehaviour
{
    [SerializeField] private List<Light> _lights;
    [SerializeField] private Renderer _mesh;
    [SerializeField] private Material _materialOff;
    [SerializeField] private Material _MaterialOn;
    [SerializeField] private float _minimumIntensity = 1.0f;
    [SerializeField] private float _maximumIntensity = 40.0f;
    [SerializeField] private float _minimumInterval = 0.1f;
    [SerializeField] private float _maximumInterval = 1.0f;

    private bool _isOn = false;
    private void Start()
    {
        StartCoroutine(OnOffCoroutine());
    }

    private IEnumerator OnOffCoroutine()
    {
        while (true)
        {
            _isOn = !_isOn;

            if (_isOn)
            {
                foreach (Light l in _lights)
                {
                    l.gameObject.SetActive(true);
                }

                _mesh.material = _MaterialOn;
            }
            else
            {
                foreach (Light l in _lights)
                {
                    l.gameObject.SetActive(false);
                }

                _mesh.material = _materialOff;
            }

            yield return new WaitForSeconds(Random.Range(_minimumInterval, _maximumInterval));
        }
    }
    
    void Update()
    {
        foreach (Light l in _lights)
        {
            l.intensity = Mathf.Lerp(l.intensity, Random.Range(_minimumIntensity, _maximumIntensity), 5.0f * Time.deltaTime);
        }
    }
}

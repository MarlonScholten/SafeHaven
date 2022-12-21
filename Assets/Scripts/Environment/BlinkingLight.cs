using System.Collections;
using UnityEngine;

/// <summary>
/// Author: Tom Cornelissen<br/>
/// Modified by: <br/>
/// Description: This script makes a light blink
/// </summary>
public class BlinkingLight : MonoBehaviour
{
    [SerializeField] 
    [Tooltip("A reference to the mesh that contains the material that needs to be swapped")]
    private Renderer _mesh;
    
    [SerializeField] 
    [Tooltip("A reference to the material that should be used when the light is on")]
    private Material _onMaterial;
    
    [SerializeField] 
    [Tooltip("A reference to the material that should be used when the light is off")]
    private Material _offMaterial;

    [SerializeField] 
    [Tooltip("The rate at which the light is blinking")]
    private float _interval = 1.0f;

    private bool isOn = false;

    private void Start()
    {
        StartCoroutine(OnOffCoroutine());
    }

    private IEnumerator OnOffCoroutine()
    {
        while (true)
        {
            isOn = !isOn;

            if (isOn)
                _mesh.material = _onMaterial;
            else
                _mesh.material = _offMaterial;

            yield return new WaitForSeconds(_interval);
        }
    }
}

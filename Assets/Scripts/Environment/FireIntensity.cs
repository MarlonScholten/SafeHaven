using UnityEngine;

public class FireIntensity : MonoBehaviour
{
    [SerializeField]
    private Light _light;

    [SerializeField] 
    private float _minimumIntensity = 10.0f;
    
    [SerializeField] 
    private float _maximumIntensity = 20.0f;
    
    void Update()
    {
        _light.intensity = Mathf.Lerp(_light.intensity, Random.Range(_minimumIntensity, _maximumIntensity), 5.0f * Time.deltaTime);
    }
}

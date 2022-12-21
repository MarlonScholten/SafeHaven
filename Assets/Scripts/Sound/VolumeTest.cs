using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeTest : MonoBehaviour
{
    public AK.Wwise.RTPC TestVolume;

    [Range(0,100)]
    public float volumeValue = 50.0f;
    void Start()
    {
        Debug.Log(TestVolume.GetGlobalValue());
        TestVolume.SetGlobalValue(volumeValue);
    }

    // Update is called once per frame
    void Update()
    {
        TestVolume.SetGlobalValue(volumeValue);
        Debug.Log(TestVolume.GetGlobalValue());
    }
}

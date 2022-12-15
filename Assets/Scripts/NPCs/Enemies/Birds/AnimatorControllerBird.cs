using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControllerBird : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        switch (transform.localRotation.eulerAngles.x)
        {
            case > 2 and <= 90:
                // Debug.Log("travel down");
                break;
            case >= 280 and <= 358:
                // Debug.Log("travel up");
                break;
            case >= 0 and <= 2:
            case > 358 and <= 360:
                // Debug.Log("travel forward");
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DebuggingTools
{
    public class DebugVersion : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<TMP_Text>()?.SetText("v" + Application.version);
        }
    }
}

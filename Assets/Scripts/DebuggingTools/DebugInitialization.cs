using UnityEngine;

namespace DebuggingTools
{
    public class DebugInitialization : MonoBehaviour
    {
        [SerializeField]
        private GameObject debugUI;

        private void Awake()
        {
            Instantiate(debugUI);
        }
    }
}

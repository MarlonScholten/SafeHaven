using UnityEngine;
using UnityEngine.Serialization;

namespace DebuggingTools
{
    public class DebugInitialization : MonoBehaviour
    {
        [SerializeField]
        private GameObject _debugUI;

        private void Awake()
        {
            Instantiate(_debugUI);
        }
    }
}

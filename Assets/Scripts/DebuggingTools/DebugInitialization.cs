using UnityEngine;
using UnityEngine.Serialization;

namespace DebuggingTools
{
    /// <summary>
    /// Class <c>DebugInitialization</c> creates given debugging UI on game start.
    /// </summary>
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

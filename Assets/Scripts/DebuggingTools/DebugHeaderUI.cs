using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DebuggingTools
{
    /// <summary>
    /// Class <c>DebugHeaderUI</c> displays a header in the debug menu.
    /// <example>For example the name of a <c>GameObject</c></example>
    /// </summary>
    public class DebugHeaderUI : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _textComponent;

        /// <summary>
        /// Set the text displayed in the header
        /// </summary>
        /// <param name="headerText">The new text that the header will display</param>
        public void SetHeaderText(string headerText)
        {
            _textComponent.SetText(headerText);
        }
    }
}

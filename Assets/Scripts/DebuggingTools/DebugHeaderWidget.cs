using TMPro;
using UnityEngine;

namespace DebuggingTools
{
    public class DebugHeaderWidget : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI textComponent;

        public void SetHeaderText(string headerText)
        {
            textComponent.SetText(headerText);
        }
    }
}

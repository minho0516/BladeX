using TMPro;
using UnityEngine;

namespace Swift_Blade
{
    public class InvenScriptionTXT : MonoBehaviour
    {
        public static InvenScriptionTXT Instance;

        private TextMeshProUGUI descriptionText;
        private void Awake()
        {
            if(Instance == null) 
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            descriptionText = GetComponent<TextMeshProUGUI>();
        }

        public void SetText(string str)
        {
            descriptionText.text = str;
        }
    }
}

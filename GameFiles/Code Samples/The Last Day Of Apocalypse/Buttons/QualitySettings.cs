
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class QualitySettings : MonoBehaviour
    {
        public int quality;
        public Dropdown qualityDropdown;
        void Start()
        {
            qualityDropdown.onValueChanged.AddListener(delegate { ChangeQuality(qualityDropdown); });
            quality = UnityEngine.QualitySettings.GetQualityLevel();
            
            Debug.Log("Quality = " + quality);
            
            Debug.Log("Quality edited = " + quality);
            qualityDropdown.value = quality;
        }

        private void Update()
        {
            
        }

        void ChangeQuality(Dropdown change)
        {
            
            quality = change.value;
            qualityDropdown.value = change.value;
            Debug.Log("Value: " + change.value);
            UnityEngine.QualitySettings.SetQualityLevel(quality,true);
        }
        
    }
}
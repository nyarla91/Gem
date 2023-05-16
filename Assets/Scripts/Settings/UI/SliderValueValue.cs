using TMPro;
using UnityEngine;

namespace Settings.UI
{
    public class SliderValueValue : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmp;

        public void ApplyValue(float value) => _tmp.text = Mathf.RoundToInt(value).ToString();
    }
}
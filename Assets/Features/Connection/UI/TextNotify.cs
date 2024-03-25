using TMPro;
using UnityEngine;

namespace Features.Connection.UI
{
    public class TextNotify : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void UpdateText(string text)
        {
            _text.text = text;
        }
    }
}

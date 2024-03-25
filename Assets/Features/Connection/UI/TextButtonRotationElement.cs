using TMPro;
using UnityEngine;

namespace Features.Connection.UI
{
    public class TextButtonRotationElement : BaseButtonRotationElement
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField, TextArea(0, 2)] private string _activeString;
        [SerializeField, TextArea(0, 2)] private string _inactiveString;
        
        protected override void ActivateInternal(ButtonRotationManager buttonRotationManager)
        {
            _text.text = _activeString;
        }

        protected override void DeactivateInternal(ButtonRotationManager buttonRotationManager)
        {
            _text.text = _inactiveString;
        }
    }
}

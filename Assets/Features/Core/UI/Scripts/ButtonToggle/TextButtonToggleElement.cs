using TMPro;
using UnityEngine;

namespace Features.Connection.UI
{
    public class TextButtonToggleElement : BaseButtonToggleElement
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField, TextArea(0, 2)] private string _activeString;
        [SerializeField, TextArea(0, 2)] private string _inactiveString;
        
        protected override void ActivateInternal(ButtonToggleGroupManager buttonToggleGroupManager)
        {
            _text.text = _activeString;
        }

        protected override void DeactivateInternal(ButtonToggleGroupManager buttonToggleGroupManager)
        {
            _text.text = _inactiveString;
        }
    }
}

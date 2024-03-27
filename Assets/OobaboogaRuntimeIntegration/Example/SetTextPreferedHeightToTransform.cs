using TMPro;
using UnityEngine;

namespace OobaboogaRuntimeIntegration.Example
{
    public class SetMinTextHeight : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private RectTransform _rect;
    
        private void Update()
        {
            UpdateHeight();
        }

        public void UpdateHeight()
        {
            if (_text == null) return;
            
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>();
            }
            
            _rect.sizeDelta = new Vector2(_rect.sizeDelta.x, _text.preferredHeight);
            _text.UpdateVertexData();
        }
    }
}
